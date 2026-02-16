using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using FlixDioAZ204.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FlixDioAZ204.Functions
{
    public class fnPostDataBase
    {
        private readonly ILogger<fnPostDataBase> _logger;

        public fnPostDataBase(ILogger<fnPostDataBase> logger)
        {
            _logger = logger;
        }

        [Function("fnPostDataBase")]
        [CosmosDBOutput("%DatabaseName%", "%ContainerNameMovies%", Connection = "CosmosDBConnectionString", CreateIfNotExists = true)]
        public async Task<object> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "movie")] HttpRequest req)
        {
            _logger.LogInformation("Processing movie metadata persistence request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var movie = JsonSerializer.Deserialize<Movie>(requestBody);

            if (movie == null)
            {
                return new BadRequestObjectResult("Invalid movie data.");
            }

            _logger.LogInformation($"Movie {movie.Title} ready for CosmosDB persistence.");

            return movie;
        }
    }
}
