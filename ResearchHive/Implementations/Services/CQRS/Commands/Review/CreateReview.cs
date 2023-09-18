using Application.Abstractions.Messaging;
using Application.Interfaces.Repositories;
using Model.Entities;
using ResearchHive.Abstractions.Messaging;
using ResearchHive.Constants;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;
using System.Text.Json.Serialization;

namespace ResearchHive.Implementations.Services
{
    public class CreateReviewRequest
    {
        public record Request(string? Comment, bool Liked, Guid? ProjectId, Guid? ResearchId) : ICommand<Guid>
        {
            [JsonIgnore]
            public Guid CreatedBy { get; set; }
        }


        public record Handler : ICommandHandler<Request, Guid>
        {
            private readonly IReviewRepository _reviewRepository;
            private readonly IProjectRepository _projectRepository;
            private readonly IResearchRepository _researchRepository;

            public Handler(IReviewRepository reviewRepository, IProjectRepository projectRepository, IResearchRepository researchRepository)
            {
                _reviewRepository = reviewRepository;
                _projectRepository = projectRepository;
                _researchRepository = researchRepository;
            }

            public async Task<Result<Guid>> Handle(Request request, CancellationToken cancellationToken)
            {
                var userCreating = request.CreatedBy;
                var projectReviewed = await _projectRepository.GetByExpressionAsync(proj => proj.Id == request.ProjectId);
                var researchReviewed = await _researchRepository.GetByExpressionAsync(res => res.Id == request.ResearchId);
                var response = new Result<Guid>();
                if (projectReviewed != null || researchReviewed != null)
                {
                    var review = new Review(request.Comment, request.Liked, request.ProjectId, request.ResearchId)
                    {
                        CreatedBy = userCreating,
                    };
                    var saveResponse = await _reviewRepository.AddAsync(review);
                    var result = await _reviewRepository.SaveChangesAsync(cancellationToken);
                    return result == 1 ? response = new Result<Guid>
                    {
                        Data = saveResponse.Id,
                        Messages = new List<string> { ResponseMessage.CreateSuccessful },
                        Succeeded = true,
                    }
                    :
                    response = new Result<Guid>
                    {
                        Messages = new List<string> { ResponseMessage.OperationFailed },
                        Succeeded = false,
                    };
                }
                return response;
            }
        }
    }
}
