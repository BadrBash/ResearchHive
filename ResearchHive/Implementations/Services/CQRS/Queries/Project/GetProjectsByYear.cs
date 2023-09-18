using Application.Abstractions.Messaging;
using Application.Interfaces.Repositories;
using Persistence.Repositories;
using ResearchHive.DTOs;
using ResearchHive.Wrapper;
using System.Text.Json.Serialization;

namespace Application.Services.Queries.Project;

public class GetProjectsByYear
{
    public class ProjectDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public Guid StudentId { get; set; }
        public string ProjectStage { get; set; }
        public Guid ProjectSubmissionWindowId { get; set; }
        public string IsApproved { get; set; }
        public string IsComplete { get; set; }
        public string StudentName { get; set; }
        public string DateCompleted { get; set; }
        public ICollection<ReviewDTO> Reviews { get; set; } = new List<ReviewDTO>();
    }
    public record Request(Guid submissionWindowId) : IQuery<IEnumerable<ProjectDTO>>
    {
        [JsonIgnore]
        public Guid SubmissionWindowId { get; set; }
    }

    public record Handler : IQueryHandler<Request, IEnumerable<ProjectDTO>>
    {
        readonly IProjectRepository _projectRepository;

        public Handler(IProjectRepository projectRepository) =>
        (_projectRepository) = (projectRepository);

        public async Task<Result<IEnumerable<ProjectDTO>>> Handle(Request request, CancellationToken cancellationToken)
        {
            var projects = await _projectRepository.GetAsync(proj => proj.ProjectSubmissionWindow.SubmissionYear == DateTime.UtcNow.Year && !proj.IsDeleted);
            if (projects.Count() <= 0) return await Result<IEnumerable<ProjectDTO>>.FailAsync("projects' retrieval returned empty data");
            var projectsDTO = projects.Select(proj => new ProjectDTO
            {
                DateCompleted = proj.DateCompleted.ToShortDateString(),
                Description = proj.Description,
                Id = proj.Id,
                IsApproved = proj.IsApproved ? "Project Is Approved" : "Project is  yet approved",
                IsComplete = proj.IsComplete ? "Project Is Complete" : "Project is  yet completed",
                ProjectStage = proj.ProjectStage.ToString(),
                ProjectSubmissionWindowId = proj.ProjectSubmissionWindowId,
                StudentName = $"{proj.Student.User.FirstName} {proj.Student.User.LastName}",
                StudentId = proj.StudentId,
                Summary = proj.Summary,
                Title = proj.Title,
                Reviews = proj.Reviews.Select(rev => new ReviewDTO
                {
                    Comment = rev.Comment,
                    Liked = rev.Liked,
                    ProjectId = rev.ProjectId
                }).ToList()
            });
            return await Result<IEnumerable<ProjectDTO>>.SuccessAsync(projectsDTO, "projects' retrieval successful");

        }
    }
}
