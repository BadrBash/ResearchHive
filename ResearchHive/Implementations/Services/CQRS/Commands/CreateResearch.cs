
using ResearchHive.Abstractions.Messaging;
using ResearchHive.Constants;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using ResearchHive.Wrapper;
using Model.Entities;
using System.Text.Json.Serialization;
using ResearchHive.Interfaces.Repositories;

namespace Application.Services
{
    public class CreateResearchRequest
    {
        public record Request(string Title, string Description, Guid AuthorId, string Summary) 
            :ICommand<Guid> 
        {
            [JsonIgnore]
            public Guid CreatedBy { get; set; }
        }

        public record Handler : ICommandHandler<Request, Guid>
        {
            private readonly IResearchRepository _researchRepository;
            private readonly  IFileService _fileService;
            private readonly ILecturerRepository _lecturerRepository;
            public Handler(IResearchRepository researchRepository, IFileService fileService, 
                ILecturerRepository lecturerRepository)
            {
                _researchRepository = researchRepository;
                _fileService = fileService;
                _lecturerRepository = lecturerRepository;
            }

            public async Task<Result<Guid>> Handle(Request request, CancellationToken cancellationToken)
            {
                var researchExists = await _researchRepository.ExistsAsync(res => res.Title == request.Title);
                var authorId = (await _lecturerRepository.GetAsync(us => us.UserId == request.CreatedBy)).Id;
                if(researchExists)
                {
                    return await Result<Guid>.FailAsync(ResponseMessage.RecordExist);
                }

                var researchUpload = await _fileService.UploadResearchesAsync(request.Title, cancellationToken);
                if(researchUpload.Succeeded)
                {
                    var research = new Research(request.Title, request.Description, authorId, researchUpload.DocumentPath, request.Summary);
                    var researchCreate = await _researchRepository.AddAsync(research);
                    var uploadResult = await _researchRepository.SaveChangesAsync(cancellationToken);
                    if (uploadResult == 0)
                    {
                        if(File.Exists(researchUpload.DocumentPath))
                        {
                            File.Delete(researchUpload.DocumentPath);
                        }
                        return new Result<Guid>
                        {
                            Messages = new List<string> { ResponseMessage.OperationFailed},
                            Succeeded = false
                        };
                    }
                    return new Result<Guid>
                    {
                        Messages = new List<string> { ResponseMessage.CreateSuccessful, researchUpload.Message },
                        Succeeded = true,
                        Data = researchCreate.Id,
                    };

                }
                return new Result<Guid>
                {
                    Messages = new List<string> { ResponseMessage.OperationFailed, researchUpload.Message},
                    Succeeded = false
                };
            }
        }
    }
}
