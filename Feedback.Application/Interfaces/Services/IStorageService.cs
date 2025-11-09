using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feedback.Application.Interfaces.Services
{
    public interface IStorageService
    {
        Task<string> UploadToS3Async(Stream fileStream, string fileName);

        
        Task<string> GetPresignedUrlAsync(string fileKey);

    }
}

