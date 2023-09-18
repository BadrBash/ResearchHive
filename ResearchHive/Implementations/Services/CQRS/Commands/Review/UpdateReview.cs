
using Application.Abstractions.Messaging;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using ResearchHive.Constants;
using ResearchHive.Implementations.Repositories;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;
using System.Text.Json.Serialization;

namespace Application.Services.Commands.Review;

public class UpdateReview
{
    public class RequestDTO
    {
        public string Comment { get; set; }
        public bool Liked { get; set; }
    }
    public record Request( RequestDTO Model) : IQuery<string>
    {
        [JsonIgnore] public Guid Id { get; set; }
        [JsonIgnore] public Guid UpdatedBy { get; set; }
    }

    public record Handler : IQueryHandler<Request, string>
    {
        private readonly IReviewRepository _ReviewRepository;
        public Handler(IReviewRepository ReviewRepository) =>
        (_ReviewRepository) = (ReviewRepository);

        public async Task<Result<string>> Handle(Request request, CancellationToken cancellationToken)
        {
            var Review = await _ReviewRepository.GetByExpressionAsync(pd => pd.Id == request.Id);

            if (Review is null) return await Result<string>.FailAsync($"Review with id: {request.Id} not found");
            Review.UpdatedBy = request.UpdatedBy;
            Review.Update(request.Model.Comment, request.Model.Liked);
           
            var updateRequest = await _ReviewRepository.UpdateAsync(Review);
            var saveChangesResult = await _ReviewRepository.SaveChangesAsync(cancellationToken);

            if (saveChangesResult == 1) return await Result<string>.SuccessAsync($" {ResponseMessage.UpdateSuccessful}");
            return await Result<string>.FailAsync(ResponseMessage.OperationFailed);

        }
    }
}
