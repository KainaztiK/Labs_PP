using AutoMapper;
using CompanyEmployess.ActionFilters;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployess.Controllers
{
    [Route("api/companies/{companyId}/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IDataShaper<EmployeeDto> _dataShaper;
        public EmployeesController(IRepositoryManager repository, ILoggerManager logger,
         IMapper mapper, IDataShaper<EmployeeDto> dataShaper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _dataShaper = dataShaper;
        }
        /// <summary>
        /// Получает список сотрутдников для компании
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="employeeParameters"></param>
        /// <returns>Получен список сотрутдников для компании</returns>
        [HttpGet]
        [HttpHead]
        public async Task<IActionResult> GetEmployeesForCompany(Guid companyId,
        [FromQuery] EmployeeParameters employeeParameters)
        {
            if (!employeeParameters.ValidAgeRange)
                return BadRequest("Max age can't be less than min age.");
            var company = _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return NotFound();
            }
            var employeesFromDb = await _repository.Employee.GetEmployeesAsync(companyId,
                employeeParameters, trackChanges: false);
            Response.Headers.Add("X-Pagination",
                JsonConvert.SerializeObject(employeesFromDb.MetaData));
            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);
            return Ok(_dataShaper.ShapeData(employeesDto, employeeParameters.Fields));
        }
        /// <summary>
        /// Получает сотрутдника по id
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="id"></param>
        /// <returns>Получен сотрутдник</returns>
        [HttpGet("{id}", Name = "GetEmployeeForCompany")]
        public IActionResult GetEmployeeForCompany(Guid companyId, Guid id)
        {
            var company = _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in thedatabase.");
                return NotFound();
            }
            var employeeDb = _repository.Employee.GetEmployeeAsync(companyId, id,
           trackChanges:
            false);
            if (employeeDb == null)
            {
                _logger.LogInfo($"Employee with id: {id} doesn't exist in thedatabase.");
                return NotFound();
            }
            var employee = _mapper.Map<EmployeeDto>(employeeDb);
            return Ok(employee);
        }
        /// <summary>
        /// Создает сотрутдника для компании
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="employee"></param>
        /// <returns>Создан сотрутдник для компании</returns>
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee)
        {
            var employeeEntity = _mapper.Map<Employee>(employee);
            _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
            _repository.SaveAsync();
            var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);
            return CreatedAtRoute("GetEmployeeForCompany", new
            {
                companyId,
                id = employeeToReturn.Id
            }, employeeToReturn);
        }
        /// <summary>
        /// Удаляет сотрудника
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="id"></param>
        /// <returns>Сотрудник удален</returns>
        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateEmployeeForCompanyExistsAttribute))]
        public async Task<IActionResult> DeleteEmployeeForCompanyAsync(Guid companyId, Guid id)
        {
            var employeeForCompany = HttpContext.Items["employee"] as Employee;
            _repository.Employee.DeleteEmployee(employeeForCompany);
            _repository.SaveAsync();
            return NoContent();
        }
        /// <summary>
        /// Обновляет данные о сотрутднике
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="id"></param>
        /// <param name="employee"></param>
        /// <returns>Данные о сотруднике обновлены</returns>
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateEmployeeForCompanyExistsAttribute))]
        public IActionResult UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employee)
        {
            var employeeEntity = HttpContext.Items["employee"] as Employee;
            _mapper.Map(employee, employeeEntity);
            _repository.SaveAsync();
            return NoContent();
        }
        /// <summary>
        /// Обновляет данные о сотрутднике
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns>Данные о сотруднике обновлены</returns>
        [HttpPatch("{id}")]
        [ServiceFilter(typeof(ValidateEmployeeForCompanyExistsAttribute))]
        public IActionResult PartiallyUpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }


            var employeeEntity = HttpContext.Items["employee"] as Employee;
            var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);
            patchDoc.ApplyTo(employeeToPatch, ModelState);
            TryValidateModel(employeeToPatch);
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }
            _mapper.Map(employeeToPatch, employeeEntity);
            _repository.SaveAsync();
            return NoContent();
        }
    }
}
