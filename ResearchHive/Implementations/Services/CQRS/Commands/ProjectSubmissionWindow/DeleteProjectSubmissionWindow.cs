
using Application.Abstractions.Messaging;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using ResearchHive.Constants;
using ResearchHive.Implementations.Repositories;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;

namespace Application.Services.Commands.ProjectSubmissionWindow;

public class DeleteProjectSubmissionWindow
{

    public record Request(Guid Id) : IQuery<string>;

    public record Handler : IQueryHandler<Request, string>
    {
        private readonly IProjectSubmissionWindowRepository _ProjectSubmissionWindowRepository;
        public Handler(IProjectSubmissionWindowRepository ProjectSubmissionWindowRepository) =>
        (_ProjectSubmissionWindowRepository) = (ProjectSubmissionWindowRepository);

        public async Task<Result<string>> Handle(Request request, CancellationToken cancellationToken)
        {
            var ProjectSubmissionWindow = await _ProjectSubmissionWindowRepository.GetByExpressionAsync(pd => pd.Id == request.Id);

            if (ProjectSubmissionWindow is null) return await Result<string>.FailAsync($"Project Submission Window with id: {request.Id} not found");
                await _ProjectSubmissionWindowRepository.DeleteAsync(ProjectSubmissionWindow);
                var saveChangesResult = await _ProjectSubmissionWindowRepository.SaveChangesAsync(cancellationToken);

                if (saveChangesResult == 1) return await Result<string>.SuccessAsync($" {ResponseMessage.DeleteSuccessful} for Project Submission Window with id: {request.Id}");
                return await Result<string>.FailAsync(ResponseMessage.OperationFailed);
           
        }
    }
}
