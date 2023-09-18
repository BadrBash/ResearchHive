using Application.Abstractions.Messaging;
using Application.DTOs;
using Application.Interfaces.Repositories;
using ResearchHive.DTOs;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;
using System.Text.Json.Serialization;

namespace Application.Services.Queries.Review;

public class GetReviewById
{
    
    public record Request() : IQuery<ReviewDTO>
    {
        [JsonIgnore] public Guid Id { get; set; }
    };

    public record Handler : IQueryHandler<Request, ReviewDTO>
    {
        private readonly IReviewRepository _reviewRepository;

        public Handler(IReviewRepository reviewRepository) =>
        (_reviewRepository) = (reviewRepository);

        public async Task<Result<ReviewDTO>> Handle(Request request, CancellationToken cancellationToken)
        {
            var rev = await _reviewRepository.GetAsync(request.Id);
            if (rev is null) return await Result<ReviewDTO>.FailAsync($"Review with id: {request.Id} not found");
            var ReviewReturned = new ReviewDTO
            {
                    Comment = rev.Comment,
                    Liked = rev.Liked,
                    ResearchId = rev.ResearchId,
                    ProjectId = rev.ProjectId
            };
            return await Result<ReviewDTO>.SuccessAsync(ReviewReturned, $"Review with id: {request.Id} successfully retrieved");


        }
    }
}
