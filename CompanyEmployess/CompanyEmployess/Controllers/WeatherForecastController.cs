using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyEmployess.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IRepositoryManager repository_)
        {
            _repository = repository_;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {

            return new string[] { "value1", "value2" };
            _logger.LogInformation("Вот информационное сообщение от нашего контроллера значений.");


            _logger.LogDebug("Вот отладочное сообщение от нашего контроллера значений.");


            _logger.LogWarning("Вот сообщение предупреждения от нашего контроллера значений.");


            _logger.LogError("Вот сообщение об ошибке от нашего контроллера значений.");
            return new string[] { "value1", "value2" };
        }

    }
}
