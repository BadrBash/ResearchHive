
using Application.Abstractions.Messaging;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using ResearchHive.Constants;
using ResearchHive.Implementations.Repositories;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;

namespace Application.Services.Commands.Review;

public class DeleteReview
{

    public record Request(Guid Id) : IQuery<string>;

    public record Handler : IQueryHandler<Request, string>
    {
        private readonly IReviewRepository _ReviewRepository;
        public Handler(IReviewRepository ReviewRepository) =>
        (_ReviewRepository) = (ReviewRepository);

        public async Task<Result<string>> Handle(Request request, CancellationToken cancellationToken)
        {
            var Review = await _ReviewRepository.GetByExpressionAsync(pd => pd.Id == request.Id);

            if (Review is null) return await Result<string>.FailAsync($"Review with id: {request.Id} not found");
            await _ReviewRepository.DeleteAsync(Review);
                var saveChangesResult = await _ReviewRepository.SaveChangesAsync(cancellationToken);

                if (saveChangesResult == 1) return await Result<string>.SuccessAsync($" {ResponseMessage.DeleteSuccessful}");
                return await Result<string>.FailAsync(ResponseMessage.OperationFailed);
           
        }
    }
}
