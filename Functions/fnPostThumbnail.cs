using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FlixDioAZ204.Functions
{
    public class fnPostThumbnail
    {
        private readonly ILogger<fnPostThumbnail> _logger;

        public fnPostThumbnail(ILogger<fnPostThumbnail> logger)
        {
            _logger = logger;
        }

        [Function("fnPostThumbnail")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "thumbnail")] HttpRequest req)
        {
            _logger.LogInformation("Processing thumbnail upload request.");

            try
            {
                if (!req.HasFormContentType)
                    return new BadRequestObjectResult("Invalid form content.");

                var form = await req.ReadFormAsync();
                var file = form.Files["file"];

                if (file == null || file.Length == 0)
                    return new BadRequestObjectResult("No file uploaded.");

                string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorageAccount");
                string containerName = Environment.GetEnvironmentVariable("ContainerNameThumbnails") ?? "thumbnails";

                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                await containerClient.CreateIfNotExistsAsync();

                string blobName = $"{Guid.NewGuid()}-{file.FileName}";
                BlobClient blobClient = containerClient.GetBlobClient(blobName);

                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, true);
                }

                _logger.LogInformation($"Thumbnail {blobName} uploaded successfully.");

                return new OkObjectResult(new { url = blobClient.Uri.ToString() });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading thumbnail: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
