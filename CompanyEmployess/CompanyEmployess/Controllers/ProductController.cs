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
        public async Task<IActionResult> GetProductsForClient(Guid clientId,
        [FromQuery] ProductParameters productParameters)
        {
            if (!productParameters.ValidPriceRange)
                return BadRequest("Max price can't be less than min price.");
            var client = _repository.Client.GetClientAsync(clientId, trackChanges: false);
            if (client == null)
            {
                _logger.LogInfo($"Client with id: {clientId} doesn't exist in the database.");
                return NotFound();
            }
            var productsFromDb = await _repository.Product.GetProductsAsync(clientId,
                productParameters, trackChanges: false);
            Response.Headers.Add("X-Pagination",
                JsonConvert.SerializeObject(productsFromDb.MetaData));
            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(productsFromDb);
            return Ok(productsDto);
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
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult CreateProductForClient(Guid ClientId, [FromBody] ProductForCreationDto Product)
        {
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
        [ServiceFilter(typeof(ValidateProductForClientExistsAttribute))]
        public async Task<IActionResult> DeleteProductForClientAsync(Guid ClientId, Guid id)
        {
            var ProductForClient = HttpContext.Items["product"] as Product;
            _repository.Product.DeleteProduct(ProductForClient);
            _repository.SaveAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateProductForClientExistsAttribute))]
        public IActionResult UpdateProductForClient(Guid ClientId, Guid id, [FromBody] ProductForUpdateDto Product)
        {
            var ProductEntity = HttpContext.Items["product"] as Product;
            _mapper.Map(Product, ProductEntity);
            _repository.SaveAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        [ServiceFilter(typeof(ValidateProductForClientExistsAttribute))]
        public IActionResult PartiallyUpdateProductForClient(Guid ClientId, Guid id, [FromBody] JsonPatchDocument<ProductForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }

           
            var ProductEntity = HttpContext.Items["product"] as Product;
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
