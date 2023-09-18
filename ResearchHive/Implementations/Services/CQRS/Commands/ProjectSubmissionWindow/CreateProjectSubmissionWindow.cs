using Application.Abstractions.Messaging;
using Model.Constants;
using Application.Interfaces.Repositories;
using ResearchHive.Wrapper;
using Model.Entities;
using Model.Enums;
using Model.Extensions;
using Microsoft.EntityFrameworkCore;
using ResearchHive.Abstractions.Messaging;
using System.Text.Json.Serialization;
using ResearchHive.Constants;

namespace ResearchHive.Implementations.Services
{
    public class CreateProjectSubmissionWindowRequest
    {
        public record Request(Level Level, DateTime StartDate, DateTime EndDate, int SubmissionYear, int NumberOfChapters, int WeeksGrace, Guid DepartmentId) : ICommand<Guid>
        {
            [JsonIgnore]
            public Guid CreatedBy { get; set; }
        }

        public record Handler : ICommandHandler<Request, Guid>
        {
            private readonly IProjectSubmissionWindowRepository _projectSubmissionWindowRepository;
            private readonly IDepartmentRepository _departmentRepository;

            public Handler(IProjectSubmissionWindowRepository projectSubmissionWindowRepository, IDepartmentRepository deptRepository)
            {
                _projectSubmissionWindowRepository = projectSubmissionWindowRepository;
                _departmentRepository = deptRepository;
            }

            public async Task<Result<Guid>> Handle(Request request, CancellationToken cancellationToken)
            {
                var user = request.CreatedBy;
                var submissionWindowExists = await _projectSubmissionWindowRepository.ExistsAsync(psw => psw.Level == request.Level &&
                psw.SubmissionYear == request.SubmissionYear);
                if (submissionWindowExists)
                {
                    return await Result<Guid>.FailAsync($"Project Submission Window {ResponseMessage.RecordExist} For {request.Level.GetDescription()} for {request.SubmissionYear}");
                }
                var dept = await _departmentRepository.Query(dept => dept.Id == request.DepartmentId).SingleOrDefaultAsync(cancellationToken);
                if (dept != null)
                {
                    var projectSubmissionWindow = new ProjectSubmissionWindow(request.SubmissionYear, request.Level, request.StartDate, request.EndDate, request.WeeksGrace, request.NumberOfChapters, dept.Name);
                    var createResult = await _projectSubmissionWindowRepository.AddAsync(projectSubmissionWindow);
                    var departmentSaveResult = await _projectSubmissionWindowRepository.SaveChangesAsync(cancellationToken);
                    //ToDo: Notify all registered students in the department about the submission window created
                    if (departmentSaveResult == 0)
                    {
                        return await Result<Guid>.FailAsync(ResponseMessage.OperationFailed);
                    }
                    return await Result<Guid>.SuccessAsync(ResponseMessage.CreateSuccessful);

                }
                return await Result<Guid>.FailAsync($" Department is needed for project submission window but: {ResponseMessage.RecordNotFound}");

            }
        }
    }
}
