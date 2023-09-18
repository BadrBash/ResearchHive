using Application.Abstractions.Messaging;
using Application.DTOs;
using Application.Interfaces.Repositories;
using ResearchHive.DTOs;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;
using System.Text.Json.Serialization;

namespace Application.Services.Queries.Research;

public class GetResearchById
{
    public class ResearchDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DocumentPath { get; set; }
        public string Summary { get; set; }
        public Guid AuthorId { get; set; }
        public string IsApproved { get; set; }
        public string Author { get; set; }
        public ICollection<ReviewDTO> Reviews { get; set; } = new List<ReviewDTO>();
    }
    public record Request() : IQuery<ResearchDTO>
    {
        [JsonIgnore] public Guid Id { get; set; }
    };

    public record Handler : IQueryHandler<Request, ResearchDTO>
    {
        private readonly IResearchRepository _ResearchRepository;

        public Handler(IResearchRepository ResearchRepository) =>
        (_ResearchRepository) = (ResearchRepository);

        public async Task<Result<ResearchDTO>> Handle(Request request, CancellationToken cancellationToken)
        {
            var proj = await _ResearchRepository.GetAsync(request.Id);
            if (proj is null) return await Result<ResearchDTO>.FailAsync($"Research with id: {request.Id} not found");
            var ResearchReturned = new ResearchDTO
            {
                Description = proj.Description,
                Id = proj.Id,
                IsApproved = proj.IsApproved ? "Research Is Approved" : "Research is  yet approved",
                AuthorId = proj.AuthorId,
                Author = $"{proj.Lecturer.User.FirstName} {proj.Lecturer.User.LastName}",
                Summary = proj.Summary,
                Title = proj.Title,
                Reviews = proj.Reviews.Select(rev => new ReviewDTO
                {
                    Comment = rev.Comment,
                    Liked = rev.Liked,
                    ResearchId = rev.ResearchId
                }).ToList()
            };
            return await Result<ResearchDTO>.SuccessAsync(ResearchReturned, $"Research with id: {request.Id} successfully retrieved");


        }
    }
}
