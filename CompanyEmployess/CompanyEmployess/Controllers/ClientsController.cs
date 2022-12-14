﻿using AutoMapper;
using CompanyEmployess.ModelBinders;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientsProduct.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public ClientsController(IRepositoryManager repository, ILoggerManager
logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetClients()
        {
            var Clients = await _repository.Client.GetAllClientsAsync(trackChanges: false);
            var ClientsDto = _mapper.Map<IEnumerable<ClientDto>>(Clients);
            return Ok(ClientsDto);
        }

        [HttpGet("{id}", Name = "ClientById")]
        public async Task<IActionResult> GetClient(Guid id)
        {
            var Client = await _repository.Client.GetClientAsync(id, trackChanges:
           false);
            if (Client == null)
            {
                _logger.LogInfo($"Client with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            else
            {
                var ClientDto = _mapper.Map<ClientDto>(Client);
                return Ok(ClientDto);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateClient([FromBody] ClientForCreationDto Client)
        {
            if (Client == null)
            {
                _logger.LogError("ClientForCreationDto object sent from client is null.");
                return BadRequest("ClientForCreationDto object is null");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the ClientForCreationDto object");
                return UnprocessableEntity(ModelState);
            }
            var ClientEntity = _mapper.Map<Client>(Client);
            _repository.Client.CreateClient(ClientEntity);
            await _repository.SaveAsync();
            var ClientToReturn = _mapper.Map<ClientDto>(ClientEntity);
            return CreatedAtRoute("ClientById", new { id = ClientToReturn.Id },
            ClientToReturn);
        }

        [HttpGet("collection/({ids})", Name = "ClientCollection")]
        public async Task<IActionResult> GetClientCollection(
        [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }
            var ClientEntities = await _repository.Client.GetByIdsAsync(ids,
            trackChanges: false);
            if (ids.Count() != ClientEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }
            var ClientToReturn =
           _mapper.Map<IEnumerable<ClientDto>>(ClientEntities);
            return Ok(ClientToReturn);
        }


        [HttpPost("collection")]
        public async Task<IActionResult> CreateClientCollection(
        [FromBody] IEnumerable<ClientForCreationDto> ClientCollection)
        {
            if (ClientCollection == null)
            {
                _logger.LogError("Client collection sent from client is null.");
                return BadRequest("Client collection is null");
            }
            var ClientEntities = _mapper.Map<IEnumerable<Client>>(ClientCollection);
            foreach (var Client in ClientEntities)
            {
                _repository.Client.CreateClient(Client);
            }
            await _repository.SaveAsync();
            var ClientCollectionToReturn =
            _mapper.Map<IEnumerable<ClientDto>>(ClientEntities);
            var ids = string.Join(",", ClientCollectionToReturn.Select(c => c.Id));
            return CreatedAtRoute("ClientCollection", new { ids },
            ClientCollectionToReturn);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(Guid id)
        {
            var Client = await _repository.Client.GetClientAsync(id, trackChanges:
           false);
            if (Client == null)
            {
                _logger.LogInfo($"Client with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _repository.Client.DeleteClient(Client);
            await _repository.SaveAsync();
            return NoContent();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(Guid id, [FromBody] ClientForUpdateDto Client)
        {
            if (Client == null)
            {
                _logger.LogError("ClientForUpdateDto object sent from client is null.");
                return BadRequest("ClientForUpdateDto object is null");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the EmployeeForUpdateDto object");
                return UnprocessableEntity(ModelState);
            }
            var ClientEntity = await _repository.Client.GetClientAsync(id, trackChanges: true);
            if (ClientEntity == null)
            {
                _logger.LogInfo($"Client with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _mapper.Map(Client, ClientEntity);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}
