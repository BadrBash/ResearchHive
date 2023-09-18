using Application.Abstractions.Messaging;
using Application.Interfaces.Repositories;
using Persistence.Repositories;
using ResearchHive.DTOs;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;

namespace Application.Services.Queries.Research;

public class GetAllResearches
{
    public class ResearchDTO
    {
        public Guid Id { get; set;}
        public string Title { get;  set; }
        public string Description { get;  set; }
        public string DocumentPath { get;  set; }
        public string Summary { get;  set; }
        public Guid AuthorId { get;  set; }
        public string ResearchStage { get;  set; }
        public string IsApproved { get;  set; }
        public string Author { get; set; }
        public ICollection<ReviewDTO> Reviews { get; set; } = new List<ReviewDTO>();
    }
    public record Request() : IQuery<IEnumerable<ResearchDTO>>;
    
    public record Handler : IQueryHandler<Request, IEnumerable<ResearchDTO>>
    {
         readonly IResearchRepository _ResearchRepository;

        public Handler(IResearchRepository ResearchRepository) =>
        (_ResearchRepository) = (ResearchRepository);

        public async Task<Result<IEnumerable<ResearchDTO>>> Handle(Request request, CancellationToken cancellationToken)
        {
            var Researchs = await _ResearchRepository.GetAllAsync();
            if(Researchs.Count() <= 0) return await  Result<IEnumerable<ResearchDTO>>.FailAsync("Researchs' retrieval returned empty data");
            var ResearchsDTO = Researchs.Select(proj => new ResearchDTO
            {
                Description = proj.Description,
                Id = proj.Id,
                IsApproved = proj.IsApproved ? "Research Is Approved" : "Research is  yet approved",
                Author = $"{proj.Lecturer.User.FirstName} {proj.Lecturer.User.LastName}",
                AuthorId = proj.AuthorId,
                DocumentPath = proj.DocumentPath,
                
                Summary = proj.Summary,
                Title = proj.Title,
                Reviews = proj.Reviews.Select(rev => new ReviewDTO
                {
                    Comment = rev.Comment,
                    Liked = rev.Liked,
                    ResearchId = rev.ResearchId
                }).ToList()
            });
            return await  Result<IEnumerable<ResearchDTO>>.SuccessAsync(ResearchsDTO, "Researches' retrieval successful");
           
        }
    }
}
