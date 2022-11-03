using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
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
            var client = _repository.Client.GetClient(clientId, trackChanges: false);
            if (client == null)
            {
                _logger.LogInfo($"Client with id: {clientId} doesn't exist in the database.");
                return NotFound();
            }
            var productFromDb = _repository.Product.GetProducts(clientId, trackChanges: false);
            var productDto = _mapper.Map<IEnumerable<ProductDto>>(productFromDb);
            return Ok(productDto);
        }
        [HttpGet("{Id}", Name = "GetProductForClient")]
        public IActionResult GetProductsForClient(Guid clientId, Guid id)
        {
            var client = _repository.Client.GetClient(clientId, trackChanges: false);
            if (client == null)
            {
                _logger.LogInfo($"Client with id: {clientId} doesn't exist in the database.");
                return NotFound();
            }
            var productDb = _repository.Product.GetProduct(clientId, id, trackChanges: false);
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
            var client = _repository.Client.GetClient(clientId, trackChanges: false);
            if (client == null)
            {
                _logger.LogInfo($"Client with id: {clientId} doesn't exist in the database.");
            return NotFound();
            }
            var productEntity = _mapper.Map<Product>(product);
            _repository.Product.CreateProductForClient(clientId, productEntity);
            _repository.Save();
            var productToReturn = _mapper.Map<ProductDto>(productEntity);
            return CreatedAtRoute("GetProductForClient", new
            {
                clientId,
                id = productToReturn.Id
            }, productToReturn);
        }
    }
}
