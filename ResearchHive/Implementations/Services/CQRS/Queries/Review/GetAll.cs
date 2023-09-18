using Application.Abstractions.Messaging;
using Application.Interfaces.Repositories;
using ResearchHive.DTOs;
using ResearchHive.Wrapper;

namespace Application.Services.Queries.Research;

public class GetAllReviews
{
  
    public record Request() : IQuery<IEnumerable<ReviewDTO>>;
    
    public record Handler : IQueryHandler<Request, IEnumerable<ReviewDTO>>
    {
         readonly IReviewRepository _reviewRepository;

        public Handler(IReviewRepository reviewRepository) =>
        (_reviewRepository) = (reviewRepository);

        public async Task<Result<IEnumerable<ReviewDTO>>> Handle(Request request, CancellationToken cancellationToken)
        {
            var reviews = await _reviewRepository.GetAllAsync();
            if(reviews.Count() <= 0) return await  Result<IEnumerable<ReviewDTO>>.FailAsync("Reviews' retrieval returned empty data");
            var reviewsDTO = reviews.Select(rev => new ReviewDTO
            {
                Comment = rev.Comment,
                Liked = rev.Liked,
                ResearchId = rev.ResearchId,
                ProjectId = rev.ProjectId
            });
            return await  Result<IEnumerable<ReviewDTO>>.SuccessAsync(reviewsDTO, "Reviews' retrieval successful");
           
        }
    }
}
