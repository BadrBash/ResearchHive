using Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.Services
{
    public interface IFileService
    {
        Task<UploadResearchResponse> UploadResearchesAsync(string title, CancellationToken cancellationToken = default);
        Task<UploadProjectDocumentResponse> UploadProjectsAsync(ProjectDocumentUploadModel projectDocumentUpload, int maxNumberOfChapters, CancellationToken cancellationToken = default);
    }
}
