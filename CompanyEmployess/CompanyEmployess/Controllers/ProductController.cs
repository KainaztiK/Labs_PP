using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyEmployess.Controllers
{
    [Route("api/clients/{clientsId}/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public ProductController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetProductsForClient(Guid clientId)
        {
            var client = _repository.Client.GetClientAsync(clientId, trackChanges: false);
            if (client == null)
            {
                _logger.LogInfo($"Client with id: {clientId} doesn't exist in the database.");
                return NotFound();
            }
            var productFromDb = _repository.Product.GetProductsAsync(clientId, trackChanges: false);
            var productDto = _mapper.Map<IEnumerable<ProductDto>>(productFromDb);
            return Ok(productDto);
        }
        [HttpGet("{Id}", Name = "GetProductForClient")]
        public IActionResult GetProductsForClient(Guid clientId, Guid id)
        {
            var client = _repository.Client.GetClientAsync(clientId, trackChanges: false);
            if (client == null)
            {
                _logger.LogInfo($"Client with id: {clientId} doesn't exist in the database.");
                return NotFound();
            }
            var productDb = _repository.Product.GetProductAsync(clientId, id, trackChanges: false);
            if (productDb == null)
            {
                _logger.LogInfo($"Product with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var product = _mapper.Map<ProductDto>(productDb);
            return Ok(product);
        }
        [HttpPost]
        public IActionResult CreateProductForClient(Guid clientId, [FromBody] ProductForCreationDto product)
        {
            if (product == null)
            {
                _logger.LogError("ProductForCreationDto object sent from client is null.");
            return BadRequest("ProductForCreationDto object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the ProductForCreationDto object");
                return UnprocessableEntity(ModelState);
            }

            var client = _repository.Client.GetClientAsync(clientId, trackChanges: false);
            if (client == null)
            {
                _logger.LogInfo($"Client with id: {clientId} doesn't exist in the database.");
            return NotFound();
            }
            var productEntity = _mapper.Map<Product>(product);
            _repository.Product.CreateProductForClient(clientId, productEntity);
            _repository.SaveAsync();
            var productToReturn = _mapper.Map<ProductDto>(productEntity);
            return CreatedAtRoute("GetProductForClient", new
            {
                clientId,
                id = productToReturn.Id
            }, productToReturn);
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteProductForClient(Guid clientId, Guid id)
        {
            var client = _repository.Client.GetClientAsync(clientId, trackChanges: false);
            if (client == null)
            {
                _logger.LogInfo($"Client with id: {clientId} doesn't exist in the database.");
                return NotFound();
            }
            var productForClient = _repository.Product.GetProductAsync(clientId, id,
            trackChanges: false);
            if (productForClient == null)
            {
                _logger.LogInfo($"Product with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _repository.Product.DeleteProduct(productForClient);
            _repository.SaveAsync();
            return NoContent();
        }
        [HttpPut("{id}")]
        public IActionResult UpdateProductForClient(Guid clientId, Guid id, [FromBody]
ProductForUpdateDto product)
        {
            if (product == null)
            {
                _logger.LogError("ProductForUpdateDto object sent from client is null.");
            return BadRequest("ProductForUpdateDto object is null");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the ProductForUpdateDto object");
                return UnprocessableEntity(ModelState);
            }
            var client = _repository.Client.GetClientAsync(clientId, trackChanges: false);
            if (client == null)
            {
                _logger.LogInfo($"Client with id: {clientId} doesn't exist in the database.");
            return NotFound();
            }
            var productEntity = _repository.Product.GetProductAsync(clientId, id, trackChanges: true);
            if (productEntity == null)
            {
                _logger.LogInfo($"Product with id: {id} doesn't exist in the database.");
            return NotFound();
            }
            _mapper.Map(product, productEntity);
            _repository.SaveAsync();
            return NoContent();
        }
        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateProductForClient(Guid clientId, Guid id,
 [FromBody] JsonPatchDocument<ProductForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }
            var client = _repository.Client.GetClientAsync(clientId, trackChanges: false);
            if (client == null)
            {
                _logger.LogInfo($"Client with id: {clientId} doesn't exist in the database.");
            return NotFound();
            }
            var productEntity = _repository.Product.GetProductAsync(clientId, id, trackChanges: true);
            if (productEntity == null)
            {
                _logger.LogInfo($"Product with id: {id} doesn't exist in the database.");
            return NotFound();
            }
            var productToPatch = _mapper.Map<ProductForUpdateDto>(productEntity);
            patchDoc.ApplyTo(productToPatch, ModelState);
            TryValidateModel(productToPatch);
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }
            _mapper.Map(productToPatch, productEntity);
            _repository.SaveAsync();
            return NoContent();
        }
    }
}
