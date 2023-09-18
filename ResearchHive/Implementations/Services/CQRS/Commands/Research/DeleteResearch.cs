
using Application.Abstractions.Messaging;
using Application.DTOs;
using Application.Interfaces.Repositories;
using ResearchHive.Constants;
using ResearchHive.Implementations.Repositories;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;

namespace Application.Services.Commands.Research;

public class DeleteResearch
{

    public record Request(Guid Id) : IQuery<string>;

    public record Handler : IQueryHandler<Request, string>
    {
        private readonly IResearchRepository _ResearchRepository;

        public Handler(IResearchRepository ResearchRepository) =>
        (_ResearchRepository) = (ResearchRepository);

        public async Task<Result<string>> Handle(Request request, CancellationToken cancellationToken)
        {
            var Research = await _ResearchRepository.GetByExpressionAsync(dept => dept.Id == request.Id);

            if (Research is null) return await Result<string>.FailAsync($"Research with id: {request.Id} not found");
            Research.IsDeleted = true;
            await _ResearchRepository.DeleteAsync(Research);
            var saveChangesResult = await _ResearchRepository.SaveChangesAsync(cancellationToken);

            if (saveChangesResult == 1) return await Result<string>.SuccessAsync($" {ResponseMessage.DeleteSuccessful} for Research with id: {request.Id}");
            return await Result<string>.FailAsync(ResponseMessage.OperationFailed);



        }
    }
}
