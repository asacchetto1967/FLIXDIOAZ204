using System.Collections.Generic;
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
            [CosmosDBInput(
                databaseName: "%DatabaseName%",
                containerName: "%ContainerNameMovies%",
                Connection = "CosmosDBConnectionString",
                SqlQuery = "SELECT * FROM c WHERE c.id = {id}")] IEnumerable<Movie> movies)
        {
            _logger.LogInformation("Retrieving movie details from CosmosDB.");

            var movie = movies.FirstOrDefault();

            if (movie == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(movie);
        }
    }
}
