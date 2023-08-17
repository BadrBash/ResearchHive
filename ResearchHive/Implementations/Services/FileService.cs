using ResearchHive.Constants;
using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces.Services;
namespace Application.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileService(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UploadResearchResponse> UploadResearchesAsync(string title, CancellationToken cancellationToken = default!)
        {
            var files = _httpContextAccessor?.HttpContext?.Request.Form;
            if (files?.Count != 0 && files?.Count == 1)
            {
                if(files.Count == 1)
                {
                    string researchesDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "Researches");
                    if(!Directory.Exists(researchesDirectory))
                    {
                        Directory.CreateDirectory(researchesDirectory);
                    }
                    Directory.CreateDirectory(researchesDirectory);
                    foreach (var file in files.Files)
                    {
                        FileInfo fileInfo = new(file.FileName);
                        if (fileInfo.Extension != ".pdf" || fileInfo.Extension != ".docx")
                        {
                            throw new UnsupportedFileTypeException(ExceptionConstants.UnsupportedFileType);
                        }
                        if (fileInfo.Length > (10 * (1024 * 1024)))
                        {
                            throw new UnsupportedFileTypeException(ExceptionConstants.FileSizeTooLarge);
                        }
                        string researchDocPath = $"{title}/{Guid.NewGuid().ToString()[..7]}{fileInfo.Extension}";
                        string fullPath = Path.Combine(researchesDirectory, researchDocPath);
                        using var fileStream = new FileStream(fullPath, FileMode.Create);
                        await file.CopyToAsync(fileStream, cancellationToken);
                        return new UploadResearchResponse
                        {
                            DocumentPath = fullPath,
                            Message = ResponseMessage.UploadSuccessful,
                            Succeeded = true
                        };
                    }

                    if(files.Count > 1)
                    {
                        throw new Exception("Can't upload more than one document.");
                    }
                }
                

            }

            return new UploadResearchResponse
            {
                DocumentPath = "",
                Message = ResponseMessage.UploadFailed,
                Succeeded = false
            };
        }

        public async Task<UploadProjectDocumentResponse> UploadProjectsAsync(ProjectDocumentUploadModel projectDocumentUploadModel, 
            int maxNumberOfChapters, CancellationToken cancellationToken = default)
        {
                        var files = _httpContextAccessor?.HttpContext?.Request.Form;
                        var response = new UploadProjectDocumentResponse();
                            string studentProjectDirectory = Path.Combine(_webHostEnvironment.WebRootPath, $"{projectDocumentUploadModel.Author}:{projectDocumentUploadModel.ProjectTitle}");
                        if (!Directory.Exists(studentProjectDirectory))
                        {
                            Directory.CreateDirectory(studentProjectDirectory);
                        }
            
                     if(files?.Count != 0)
                     {
                        foreach (var file in files?.Files)
                        {
                            FileInfo fileInfo = new(file.FileName);
                            if (fileInfo.Extension != ".pdf" || fileInfo.Extension != ".docx")
                            {
                                throw new UnsupportedFileTypeException(ExceptionConstants.UnsupportedFileType);
                            }
                            if (fileInfo.Length > (10 * (1024 * 1024)))
                            {
                                throw new UnsupportedFileTypeException(ExceptionConstants.FileSizeTooLarge);
                            }
                            
                            var chapterNumber = int.Parse(fileInfo.Name.Split(".")[0]);
                            if(chapterNumber <= maxNumberOfChapters)
                            {
                                string projectDocPath = $"-{chapterNumber}/{Guid.NewGuid().ToString()[..3]}{fileInfo.Extension}";
                                string fullPath = Path.Combine(studentProjectDirectory, projectDocPath);
                                using var fileStream = new FileStream(fullPath, FileMode.Create);
                                await file.CopyToAsync(fileStream, cancellationToken);
                                response.DocumentPathsAndNumber.Add(new()
                                {
                                    ChapterNumber = chapterNumber,
                                    DocumentPath = fullPath,
                                });
                                response.Message = $"{response.DocumentPathsAndNumber.Count} {ResponseMessage.UploadSuccessful}";
                                response.Succeeded = true;
                            }
                          
                        }
                        if (response.DocumentPathsAndNumber.Count > 0)
                        {
                            return response;
                        }

                        else if(response.DocumentPathsAndNumber.Count == 0)
                        {
                            return new UploadProjectDocumentResponse
                            {
                                Message = ResponseMessage.OperationFailed
                            };
                        }
                        return response;
                     }

                    
                    return new UploadProjectDocumentResponse
                    {
                        Message = ResponseMessage.UploadFailed,
                        Succeeded = false
                    };
        }

    }
}
