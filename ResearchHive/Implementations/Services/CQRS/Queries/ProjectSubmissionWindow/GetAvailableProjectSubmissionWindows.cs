using Application.Abstractions.Messaging;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Model.Entities;
using ResearchHive.Wrapper;

namespace Application.Services.Queries.ProjectSubmissionWindow;


public class GetAvailableProjectSubmissionWindows
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
        private readonly IProjectSubmissionWindowRepository _projectSubmissionWindowRepository;

        public Handler(IProjectSubmissionWindowRepository projectSubmissionWindowRepository) =>
        (_projectSubmissionWindowRepository) = (projectSubmissionWindowRepository);

        public async Task<Result<IEnumerable<ProjectSubmissionWindowDTO>>> Handle(Request request, CancellationToken cancellationToken)
        {
            var projectSubmissionWindows = await _projectSubmissionWindowRepository.GetAsync(psw => !psw.IsClosed);
            if (projectSubmissionWindows.Count() is 0) return await Result<IEnumerable<ProjectSubmissionWindowDTO>>.FailAsync($"available project Submission Windows returned empty data ");
            var projectSubmissionWindowsReturned = projectSubmissionWindows.Select(projectSubmissionWindow => new ProjectSubmissionWindowDTO
            {
                DepartmentName = projectSubmissionWindow.DepartmentName,
                EndDate = projectSubmissionWindow.EndDate,
                Id = projectSubmissionWindow.Id,
                StartDate = projectSubmissionWindow.StartDate,
                AvailableForSubmission = projectSubmissionWindow.IsClosed ? "Not Available For Submission" : "Available For Submission",
                WeeksGrace = projectSubmissionWindow.WeeksGrace,
                NumberOfChapters = projectSubmissionWindow.NumberOfChapters,
                IsClosed = projectSubmissionWindow.IsClosed,
                Level = projectSubmissionWindow.Level.ToString(),
                Set = projectSubmissionWindow.Set,
                SubmissionYear = projectSubmissionWindow.SubmissionYear
            });
            return await Result<IEnumerable<ProjectSubmissionWindowDTO>>.SuccessAsync(projectSubmissionWindowsReturned, $"Available project Submission Windows successfully retrieved");


        }
    }
}
