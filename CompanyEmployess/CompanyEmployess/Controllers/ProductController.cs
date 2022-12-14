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
using System.Threading.Tasks;

namespace CompanyEmployess.Controllers
{
    [Route("api/clients/{clientsId}/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public ProductController(IRepositoryManager repository, ILoggerManager
       logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetProductsForClient(Guid ClientId, Guid id)
        {
            var Client = _repository.Client.GetClientAsync(ClientId, trackChanges: false);
            if (Client == null)
            {
                _logger.LogInfo($"Client with id: {ClientId} doesn't exist in the database.");
                return NotFound();
            }
            var ProductsFromDb = _repository.Product.GetProductsAsync(ClientId,
            trackChanges: false);
            var ProductsDto = _mapper.Map<IEnumerable<ProductDto>>(ProductsFromDb);
            return Ok(ProductsDto);
        }

        [HttpGet("{id}", Name = "GetProductForClient")]
        public IActionResult GetProductForClient(Guid ClientId, Guid id)
        {
            var Client = _repository.Client.GetClientAsync(ClientId, trackChanges: false);
            if (Client == null)
            {
                _logger.LogInfo($"Client with id: {ClientId} doesn't exist in thedatabase.");
                return NotFound();
            }
            var ProductDb = _repository.Product.GetProductAsync(ClientId, id,
           trackChanges:
            false);
            if (ProductDb == null)
            {
                _logger.LogInfo($"Product with id: {id} doesn't exist in thedatabase.");
                return NotFound();
            }
            var Product = _mapper.Map<ProductDto>(ProductDb);
            return Ok(Product);
        }

        [HttpPost]
        public IActionResult CreateProductForClient(Guid ClientId, [FromBody] ProductForCreationDto Product)
        {
            if (Product == null)
            {
                _logger.LogError("ProductForCreationDto object sent from client isnull.");
                return BadRequest("ProductForCreationDto object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the ProductForCreationDtoobject");
                return UnprocessableEntity(ModelState);
            }

            var Client = _repository.Client.GetClientAsync(ClientId, trackChanges: false);
            if (Client == null)
            {
                _logger.LogInfo($"Client with id: {ClientId} doesn't exist in thedatabase.");
                return NotFound();
            }
            var ProductEntity = _mapper.Map<Product>(Product);
            _repository.Product.CreateProductForClient(ClientId, ProductEntity);
            _repository.SaveAsync();
            var ProductToReturn = _mapper.Map<ProductDto>(ProductEntity);
            return CreatedAtRoute("GetProductForClient", new
            {
                ClientId,
                id = ProductToReturn.Id
            }, ProductToReturn);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductForClientAsync(Guid ClientId, Guid id)
        {
            var Client = await _repository.Client.GetClientAsync(ClientId, trackChanges: false);
            if (Client == null)
            {
                _logger.LogInfo($"Client with id: {ClientId} doesn't exist in thedatabase.");
                return NotFound();
            }
            var ProductForClient = await _repository.Product.GetProductAsync(ClientId, id,
            trackChanges: false);
            if (ProductForClient == null)
            {
                _logger.LogInfo($"Product with id: {id} doesn't exist in thedatabase.");
                return NotFound();
            }
            _repository.Product.DeleteProduct(ProductForClient);
            _repository.SaveAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProductForClient(Guid ClientId, Guid id, [FromBody] ProductForUpdateDto Product)
        {
            if (Product == null)
            {
                _logger.LogError("ProductForUpdateDto object sent from client isnull.");
                return BadRequest("ProductForUpdateDto object is null");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the ProductForUpdateDto object");
                return UnprocessableEntity(ModelState);
            }
            var Client = _repository.Client.GetClientAsync(ClientId, trackChanges: false);
            if (Client == null)
            {
                _logger.LogInfo($"Client with id: {ClientId} doesn't exist in thedatabase.");
                return NotFound();
            }
            var ProductEntity = _repository.Product.GetProductAsync(ClientId, id,
           trackChanges:
            true);
            if (ProductEntity == null)
            {
                _logger.LogInfo($"Product with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            _mapper.Map(Product, ProductEntity);
            _repository.SaveAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateProductForClient(Guid ClientId, Guid id, [FromBody] JsonPatchDocument<ProductForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }

            var Client = _repository.Client.GetClientAsync(ClientId, trackChanges: false);
            if (Client == null)
            {
                _logger.LogInfo($"Client with id: {ClientId} doesn't exist in the  database.");
                return NotFound();
            }
            var ProductEntity = _repository.Product.GetProductAsync(ClientId, id,
           trackChanges:
            true);
            if (ProductEntity == null)
            {
                _logger.LogInfo($"Product with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var ProductToPatch = _mapper.Map<ProductForUpdateDto>(ProductEntity);
            patchDoc.ApplyTo(ProductToPatch, ModelState);
            TryValidateModel(ProductToPatch);
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }
            _mapper.Map(ProductToPatch, ProductEntity);
            _repository.SaveAsync();
            return NoContent();
        }
    }
}
