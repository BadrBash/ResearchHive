using Application.Abstractions.Messaging;
using Application.Interfaces.Repositories;
using Model.Enums;
using Persistence.Repositories;
using ResearchHive.Wrapper;

namespace Application.Services.Queries.ProjectSubmissionWindow;

public class GetAllProjectSubmissionWindows
{
    public class ProjectSubmissionWindowDTO
    {
        public Guid Id { get; set; }
        public string Set { get; set; }
        public int SubmissionYear { get; set; }
        public string DepartmentName { get; set; }
        public string Level { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsClosed { get; set; }
        public int NumberOfChapters { get; set; }
        public int WeeksGrace { get; set; }
        public string AvailableForSubmission { get; set; }

    }
    public record Request() : IQuery<IEnumerable<ProjectSubmissionWindowDTO>>;

    public record Handler : IQueryHandler<Request, IEnumerable<ProjectSubmissionWindowDTO>>
    {
        readonly IProjectSubmissionWindowRepository _projectSubmissionWindowRepository;

        public Handler(IProjectSubmissionWindowRepository projectSubmissionWindowRepository) =>
        (_projectSubmissionWindowRepository) = (projectSubmissionWindowRepository);

        public async Task<Result<IEnumerable<ProjectSubmissionWindowDTO>>> Handle(Request request, CancellationToken cancellationToken)
        {
            var projectSubmissionWindows = await _projectSubmissionWindowRepository.GetAllAsync();
            if (projectSubmissionWindows.Count() <= 0) return await Result<IEnumerable<ProjectSubmissionWindowDTO>>.FailAsync("project Submission Windows' retrieval returned empty data");
            var projectSubmissionWindowsDTO = projectSubmissionWindows.Select(proj => new ProjectSubmissionWindowDTO
            {
                DepartmentName = proj.DepartmentName,
                EndDate = proj.EndDate,
                Id = proj.Id,
                StartDate = proj.StartDate,
                AvailableForSubmission = proj.IsClosed ? "Not Available For Submission" : "Available For Submission",
                WeeksGrace = proj.WeeksGrace,
                NumberOfChapters = proj.NumberOfChapters,
                IsClosed = proj.IsClosed,
                Level = proj.Level.ToString(),
                Set = proj.Set,
                SubmissionYear = proj.SubmissionYear
            });
            return await Result<IEnumerable<ProjectSubmissionWindowDTO>>.SuccessAsync(projectSubmissionWindowsDTO, "project Submission Windows' retrieval successful");

        }
    }
}
