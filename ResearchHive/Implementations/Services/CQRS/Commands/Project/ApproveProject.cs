
using Application.Abstractions.Messaging;
using Application.DTOs;
using Application.Interfaces.Repositories;
using ResearchHive.Constants;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;
using System.Text.Json.Serialization;

namespace Application.Services.Commands.Project;

public class ApproveProject
{

    public record Request() : IQuery<Guid>
    {
        [JsonIgnore] public Guid Id { get; set; }
        [JsonIgnore] public Guid UpdatedBy { get; set; }
    }
    public record Handler : IQueryHandler<Request, Guid>
    {
        private readonly IProjectRepository _ProjectRepository;

        public Handler(IProjectRepository ProjectRepository) =>
        (_ProjectRepository) = (ProjectRepository);

        public async Task<Result<Guid>> Handle(Request request, CancellationToken cancellationToken)
        {
            var Project = await _ProjectRepository.GetByExpressionAsync(proj => proj.Id == request.Id);
            if (Project is null) return await Result<Guid>.FailAsync($"Project with id: {request.Id} not found");
            var updatedproj = Project?.Approve();
            if(updatedproj.IsApproved) 
            {
                    updatedproj.UpdatedBy = request.UpdatedBy;
                var updateRequest = await _ProjectRepository.UpdateAsync(updatedproj);
                var saveChangesResult = await _ProjectRepository.SaveChangesAsync(cancellationToken);

                if (saveChangesResult == 1) return await Result<Guid>.SuccessAsync(updateRequest.Id, $" {ResponseMessage.UpdateSuccessful} for Project with id: {request.Id} successfully approved - Approved And Completed");
                return await Result<Guid>.FailAsync(ResponseMessage.OperationFailed);
            }
            return await Result<Guid>.FailAsync(ResponseMessage.OperationFailed + @"Previous Stage: 'Awaiting Approval' not achieved yet");
        }
    }
}
