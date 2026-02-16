using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using FlixDioAZ204.Models;

namespace FlixDioAZ204.Functions
{
    public class fnGetMovieDetails
    {
        private readonly ILogger<fnGetMovieDetails> _logger;

        public fnGetMovieDetails(ILogger<fnGetMovieDetails> logger)
        {
            _logger = logger;
        }

        [Function("fnGetMovieDetails")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "movie/{id}")] HttpRequest req,
            string id,
            [CosmosDBInput(
                databaseName: "%DatabaseName%",
                containerName: "%ContainerNameMovies%",
                Connection = "CosmosDBConnectionString",
                Id = "{id}",
                PartitionKey = "{id}")] Movie movie)
        {
            _logger.LogInformation($"Retrieving movie details for ID: {id}");

            if (movie == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(movie);
        }
    }
}
