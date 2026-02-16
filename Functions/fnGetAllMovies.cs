using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using FlixDioAZ204.Models;
using System.Linq;

namespace FlixDioAZ204.Functions
{
    public class fnGetAllMovies
    {
        private readonly ILogger<fnGetAllMovies> _logger;

        public fnGetAllMovies(ILogger<fnGetAllMovies> logger)
        {
            _logger = logger;
        }

        [Function("fnGetAllMovies")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "movies")] HttpRequest req,
            [CosmosDBInput(
                databaseName: "%DatabaseName%",
                containerName: "%ContainerNameMovies%",
                Connection = "CosmosDBConnectionString",
                SqlQuery = "SELECT * FROM c")] IEnumerable<Movie> movies)
        {
            _logger.LogInformation("Retrieving all movies from CosmosDB.");

            return new OkObjectResult(movies);
        }
    }
}
