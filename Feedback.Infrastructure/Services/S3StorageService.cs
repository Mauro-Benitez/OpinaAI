using Amazon.S3;
using Amazon.S3.Model;
using Feedback.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;


namespace Feedback.Infrastructure.Services
{
    public class S3StorageService : IStorageService
    {
        private readonly IConfiguration _configuration;
        private readonly string _bucketName;
        private readonly string _region;
        private readonly AmazonS3Client _client;

        public S3StorageService(IConfiguration configuration)
        {
            _configuration = configuration;
            var awsConfig = _configuration.GetSection("AWS:S3");
            var bucketName = awsConfig["BucketName"];
            var accessKey = awsConfig["AccessKey"];
            var secretKey = awsConfig["SecretKey"];
            _bucketName = awsConfig["BucketName"];
            _region = awsConfig["Region"];

            _client = new AmazonS3Client(accessKey, secretKey, Amazon.RegionEndpoint.GetBySystemName(_region));
        }

        public Task<string> GetPresignedUrlAsync(string fileKey)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = fileKey,
                Expires = DateTime.UtcNow.AddMinutes(10)
            };

            string url = _client.GetPreSignedURL(request);
            return Task.FromResult(url);
        }

        public async Task<string> UploadToS3Async(Stream fileStream, string fileName)
        {      
                       
               var request = new PutObjectRequest
                {
                    InputStream = fileStream,
                    BucketName = _bucketName,
                    Key = fileName,

                };

                await _client.PutObjectAsync(request);
                return fileName;            
        }
    }
}
