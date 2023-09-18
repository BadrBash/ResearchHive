
using Application.Abstractions.Messaging;
using Application.DTOs;
using Application.Interfaces.Repositories;
using ResearchHive.Constants;
using ResearchHive.Implementations.Repositories;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;

namespace Application.Services.Commands.Project;

public class DeleteProject
{

    public record Request(Guid Id) : IQuery<string>;

    public record Handler : IQueryHandler<Request, string>
    {
        private readonly IProjectRepository _ProjectRepository;

        public Handler(IProjectRepository ProjectRepository) =>
        (_ProjectRepository) = (ProjectRepository);

        public async Task<Result<string>> Handle(Request request, CancellationToken cancellationToken)
        {
            var Project = await _ProjectRepository.GetByExpressionAsync(dept => dept.Id == request.Id);

            if (Project is null) return await Result<string>.FailAsync($"Project with id: {request.Id} not found");
           await _ProjectRepository.DeleteAsync(Project);
            var saveChangesResult = await _ProjectRepository.SaveChangesAsync(cancellationToken);

            if (saveChangesResult == 1) return await Result<string>.SuccessAsync($" {ResponseMessage.DeleteSuccessful} for Project with id: {request.Id}");
            return await Result<string>.FailAsync(ResponseMessage.OperationFailed);



        }
    }
}
