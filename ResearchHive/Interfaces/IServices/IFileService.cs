using Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.IO;
using System.Security.Policy;

namespace Application.Interfaces.Services
{
    public interface IFileService
    {
        Task<UploadResearchResponse> UploadResearchesAsync(string title, CancellationToken cancellationToken = default);
        Task<UploadProjectDocumentResponse> UploadProjectsAsync(ProjectDocumentUploadModel projectDocumentUpload, CancellationToken cancellationToken = default);
        public static bool DeleteFile(string directoryPath, string fileName)
        {

            string filePath = Path.Combine(directoryPath, fileName);

            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                return true;
                
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
