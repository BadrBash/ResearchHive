using Application.Abstractions.Messaging;
using Application.DTOs;
using Application.Interfaces.Repositories;
using ResearchHive.Wrapper;

namespace Application.Services.Queries.ProjectSubmissionWindow;


public class GetById
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
    public record Request(Guid id) : IQuery<ProjectSubmissionWindowDTO>;

    public record Handler : IQueryHandler<Request, ProjectSubmissionWindowDTO>
    {
        private readonly IProjectSubmissionWindowRepository _projectSubmissionWindowRepository;

        public Handler(IProjectSubmissionWindowRepository projectSubmissionWindowRepository) =>
        (_projectSubmissionWindowRepository) = (projectSubmissionWindowRepository);

        public async Task<Result<ProjectSubmissionWindowDTO>> Handle(Request request, CancellationToken cancellationToken)
        {
            var projectSubmissionWindow = await _projectSubmissionWindowRepository.GetByExpressionAsync(psw => psw.Id == request.id);
            if (projectSubmissionWindow is null) return await Result<ProjectSubmissionWindowDTO>.FailAsync($"project Submission Window with id: {request.id} not found");
            var projectSubmissionWindowReturned = new ProjectSubmissionWindowDTO
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
            };
            return await Result<ProjectSubmissionWindowDTO>.SuccessAsync(projectSubmissionWindowReturned, $"projectSubmissionWindow with id: {request.id} successfully retrieved");


        }
    }
}
