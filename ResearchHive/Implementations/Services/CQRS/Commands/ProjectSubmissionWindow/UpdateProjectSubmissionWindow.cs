
using Application.Abstractions.Messaging;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using ResearchHive.Constants;
using ResearchHive.Implementations.Repositories;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;

namespace Application.Services.Commands.ProjectSubmissionWindow;

public class UpdateProjectSubmissionWindow
{
    public class RequestDTO
    {

        public int NumberOfChapters { get;  set; }
        public int WeeksGrace { get;  set; }
        public int Year { get; set; }
        public Guid UpdatedBy { get; set; }
    }

    public record Request(Guid Id, RequestDTO Model) : IQuery<string>;

    public record Handler : IQueryHandler<Request, string>
    {
        private readonly IProjectSubmissionWindowRepository _ProjectSubmissionWindowRepository;
        public Handler(IProjectSubmissionWindowRepository ProjectSubmissionWindowRepository) =>
        (_ProjectSubmissionWindowRepository) = (ProjectSubmissionWindowRepository);

        public async Task<Result<string>> Handle(Request request, CancellationToken cancellationToken)
        {
            var ProjectSubmissionWindow = await _ProjectSubmissionWindowRepository.GetByExpressionAsync(pd => pd.Id == request.Id);

            if (ProjectSubmissionWindow is null) return await Result<string>.FailAsync($"Project Submission Window with id: {request.Id} not found");
            ProjectSubmissionWindow = ProjectSubmissionWindow.Update(request.Model.Year, request.Model.WeeksGrace, request.Model.NumberOfChapters);
            ProjectSubmissionWindow.UpdatedBy = request.Model.UpdatedBy;
            var updateRequest = await _ProjectSubmissionWindowRepository.UpdateAsync(ProjectSubmissionWindow);
            var saveChangesResult = await _ProjectSubmissionWindowRepository.SaveChangesAsync(cancellationToken);

            if (saveChangesResult == 1) return await Result<string>.SuccessAsync($" {ResponseMessage.UpdateSuccessful} for Project Submission Window with id: {request.Id}");
            return await Result<string>.FailAsync(ResponseMessage.OperationFailed);

        }
    }
}
