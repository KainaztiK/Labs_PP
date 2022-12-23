using Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CompanyEmployess.Controllers
{
    [Route("api/companies")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v2")]
    public class CompaniesV2Controller : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        public CompaniesV2Controller(IRepositoryManager repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// Получает список всех компаний
        /// </summary>
        /// <returns> Список компаний</returns>
        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await
           _repository.Company.GetAllCompaniesAsync(trackChanges:
            false);
            return Ok(companies);
        }
    }
}
