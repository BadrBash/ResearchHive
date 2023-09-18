
using Application.Abstractions.Messaging;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using ResearchHive.Constants;
using ResearchHive.Implementations.Repositories;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;
using System.Text.Json.Serialization;

namespace Application.Services.Commands.Research;

public class UpdateResearch
{
   
    public record Request() : IQuery<Guid>
    {
        [JsonIgnore] public Guid Id { get; set; }
        [JsonIgnore] public Guid UpdatedBy { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
    }


    public record Handler : IQueryHandler<Request, Guid>
    {
        private readonly IFileService _fileService;
        private readonly IResearchRepository _ResearchRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public Handler(IResearchRepository ResearchRepository, IFileService fileService, IWebHostEnvironment webHostEnvironment) =>
        (_ResearchRepository, _fileService, _webHostEnvironment) = (ResearchRepository, fileService, webHostEnvironment);

        public async Task<Result<Guid>> Handle(Request request, CancellationToken cancellationToken)
        {
            var Research = await _ResearchRepository.GetByExpressionAsync(usr => usr.Id == request.Id);
            if (Research is null) return await Result<Guid>.FailAsync($"Research with id: {request.Id} not found");
            var deleteResponse = IFileService.DeleteFile(Path.Combine(_webHostEnvironment.WebRootPath, "Researches"),Research.DocumentPath);
            if(deleteResponse)
            {
                var fileUploadResponse = await _fileService.UploadResearchesAsync(Research.Title);
                var updatedResearch = Research?.Update(request.Title, request.Description, request.Summary, fileUploadResponse.DocumentPath);
                updatedResearch.UpdatedBy = request.UpdatedBy;
                var updateRequest = await _ResearchRepository.UpdateAsync(updatedResearch);
                var saveChangesResult = await _ResearchRepository.SaveChangesAsync(cancellationToken);

                if (saveChangesResult == 1) return await Result<Guid>.SuccessAsync(updateRequest.Id, $" {ResponseMessage.UpdateSuccessful} for Research with id: {request.Id}");
                return await Result<Guid>.FailAsync(ResponseMessage.OperationFailed);
            }
            return await Result<Guid>.FailAsync(ResponseMessage.OperationFailed);
           


        }
    }
}
