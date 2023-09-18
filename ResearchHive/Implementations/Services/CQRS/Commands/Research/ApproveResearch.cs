
using Application.Abstractions.Messaging;
using Application.DTOs;
using Application.Interfaces.Repositories;
using ResearchHive.Constants;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;
using System.Text.Json.Serialization;

namespace Application.Services.Commands.Research;

public class ApproveResearch
{

    public record Request() : IQuery<Guid>
    {
       [JsonIgnore] public Guid UpdatedBy { get; set; }
        [JsonIgnore] public Guid Id { get; set; }
    }

        public record Handler : IQueryHandler<Request, Guid>
    {
        private readonly IResearchRepository _ResearchRepository;

        public Handler(IResearchRepository ResearchRepository) =>
        (_ResearchRepository) = (ResearchRepository);

        public async Task<Result<Guid>> Handle(Request request, CancellationToken cancellationToken)
        {
            var Research = await _ResearchRepository.GetByExpressionAsync(res => res.Id == request.Id);
            if (Research is null) return await Result<Guid>.FailAsync($"Research with id: {request.Id} not found");
            var updatedres = Research?.Approve();
            updatedres.UpdatedBy = request.UpdatedBy;
            if(updatedres.IsApproved) 
            {
                var updateRequest = await _ResearchRepository.UpdateAsync(updatedres);
                var saveChangesResult = await _ResearchRepository.SaveChangesAsync(cancellationToken);

                if (saveChangesResult == 1) return await Result<Guid>.SuccessAsync(updateRequest.Id, $" {ResponseMessage.UpdateSuccessful} for Research with id: {request.Id} successfully approved");
                return await Result<Guid>.FailAsync(ResponseMessage.OperationFailed);
            }
            return await Result<Guid>.FailAsync(ResponseMessage.OperationFailed);
        }
    }
}
