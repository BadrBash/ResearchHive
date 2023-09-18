using Application.Abstractions.Messaging;
using Application.Interfaces.Repositories;
using ResearchHive.Constants;
using ResearchHive.Wrapper;
using System.Text.Json.Serialization;

namespace Application.Services.Commands.ProjectSubmissionWindow
{
    public class CloseProjectSubmission
    {

        public record Request(Guid Id) : IQuery<string>
        {
            [JsonIgnore] public Guid UpdatedBy { get; set; }
        }

        public record Handler : IQueryHandler<Request, string>
        {
            private readonly IProjectSubmissionWindowRepository _ProjectSubmissionWindowRepository;
            public Handler(IProjectSubmissionWindowRepository ProjectSubmissionWindowRepository) =>
            (_ProjectSubmissionWindowRepository) = (ProjectSubmissionWindowRepository);

            public async Task<Result<string>> Handle(Request request, CancellationToken cancellationToken)
            {
                var ProjectSubmissionWindow = await _ProjectSubmissionWindowRepository.GetByExpressionAsync(pd => pd.Id == request.Id);

                if (ProjectSubmissionWindow is null) return await Result<string>.FailAsync($"Project Submission Window with id: {request.Id} not found");
                ProjectSubmissionWindow = ProjectSubmissionWindow.CloseProject();
                ProjectSubmissionWindow.UpdatedBy = request.UpdatedBy;
                var updateRequest = await _ProjectSubmissionWindowRepository.UpdateAsync(ProjectSubmissionWindow);
                var saveChangesResult = await _ProjectSubmissionWindowRepository.SaveChangesAsync(cancellationToken);

                if (saveChangesResult == 1) return await Result<string>.SuccessAsync($" {ResponseMessage.UpdateSuccessful} for Project Submission Window with id: {request.Id}");
                return await Result<string>.FailAsync(ResponseMessage.OperationFailed);

            }
        }
    }
}
