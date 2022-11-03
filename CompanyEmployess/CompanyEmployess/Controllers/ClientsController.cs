using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClientsProduct.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public CompaniesController(IRepositoryManager repository, ILoggerManager
logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetClients()
        {
            var clients = _repository.Client.GetAllClients(trackChanges: false);
            var clientsDto = _mapper.Map<IEnumerable<ClientDto>>(clients);
            return Ok(clientsDto);
        }
        [HttpGet("{id}", Name = "ClientById")]
        public IActionResult GetClient(Guid id)
        {
            var client = _repository.Client.GetClient(id, trackChanges: false);
            if (client == null)
            {
                _logger.LogInfo($"Client with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            else
            {
                var clientDto = _mapper.Map<CompanyDto>(client);
                return Ok(clientDto);
            }
        }
        [HttpPost]
        public IActionResult CreateClient([FromBody] ClientForCreationDto client)
        {
            if (client == null)
            {
                _logger.LogError("ClientForCreationDto object sent from client is null.");
            return BadRequest("ClientForCreationDto object is null");
            }
            var clientEntity = _mapper.Map<Client>(client);
            _repository.Client.CreateClient(clientEntity);
            _repository.Save();
            var clientToReturn = _mapper.Map<ClientDto>(clientEntity);
            return CreatedAtRoute("ClientById", new { id = clientToReturn.Id },clientToReturn);
        }


        [HttpGet("collection/({ids})", Name = "ClientCollection")]
        public IActionResult GetClientCollection(IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }
            var clientEntities = _repository.Client.GetByIds(ids, trackChanges: false);
            if (ids.Count() != clientEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }
            var clientsToReturn =
           _mapper.Map<IEnumerable<ClientDto>>(clientEntities);
            return Ok(clientsToReturn);
        }


        [HttpPost("collection")]
        public IActionResult CreateClientCollection([FromBody] IEnumerable<ClientForCreationDto> clientCollection)
        {
            if (clientCollection == null)
            {
                _logger.LogError("Client collection sent from client is null.");
                return BadRequest("Client collection is null");
            }
            var clientEntities = _mapper.Map<IEnumerable<Client>>(clientCollection);
            foreach (var client in clientEntities)
            {
                _repository.Client.CreateClient(client);
            }
            _repository.Save();
            var clientCollectionToReturn = _mapper.Map<IEnumerable<ClientDto>>(clientEntities);
            return CreatedAtRoute("ClientCollection", clientCollectionToReturn);
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteClient(Guid id)
        {
            var client = _repository.Client.GetClient(id, trackChanges: false);
            if (client == null)
            {
                _logger.LogInfo($"Client with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _repository.Client.DeleteClient(client);
            _repository.Save();
            return NoContent();
        }
        [HttpPut("{id}")]
        public IActionResult UpdateClient(Guid id, [FromBody] ClientForUpdateDto client)
        {
            if (client == null)
            {
                _logger.LogError("ClientForUpdateDto object sent from client is null.");
                return BadRequest("ClientForUpdateDto object is null");
            }
            var clientEntity = _repository.Client.GetClient(id, trackChanges: true);
            if (clientEntity == null)
            {
                _logger.LogInfo($"Client with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _mapper.Map(client, clientEntity);
            _repository.Save();
            return NoContent();
        }
    }
}
