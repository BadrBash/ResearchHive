using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Model.Entities;
using ResearchHive.Abstractions.Messaging;
using ResearchHive.Constants;
using ResearchHive.Wrapper;
using System.Text.Json.Serialization;

namespace Application.Services
{
    public class AssignSupervisorToStudent
    {
        public record Request(Guid SupervisorId, List<Guid> StudentIds) : ICommand<string> 
        {
            [JsonIgnore]
            public Guid CreatedBy { get; set; }
        }

        public record Handler : ICommandHandler<Request, string>
        {
            private readonly IStudentRepository _studentRepository;
            private readonly ILecturerRepository _supervisorRepository;

            public Handler(IStudentRepository studentRepository, ILecturerRepository supervisorRepsoitory)
            {
                _studentRepository = studentRepository;
                _supervisorRepository = supervisorRepsoitory;
            }

            public async Task<Result<string>> Handle(Request request, CancellationToken cancellationToken)
            {
                var students = await _studentRepository.Query(st => request.StudentIds.Contains(st.Id)).ToListAsync(cancellationToken);
                var supervisor = await _supervisorRepository.Query(su => su.Id == request.SupervisorId).SingleOrDefaultAsync(cancellationToken);
                if(students.Count != 0 )
                {
                    if(supervisor != null)
                    {
                        foreach (var student in students)
                        {
                            var studentSupervisor = new StudentSupervisor(student.Id, request.SupervisorId)
                            {
                                CreatedBy = request.CreatedBy
                            };
                            supervisor.StudentSupervisors.Add(studentSupervisor);
                            await _supervisorRepository.UpdateAsync(supervisor);
                            var saveChangesResult = await _supervisorRepository.SaveChangesAsync(cancellationToken);
                            if (saveChangesResult == 1) return await Result<string>.SuccessAsync(ResponseMessage.UpdateSuccessful);
                            return await Result<string>.FailAsync(ResponseMessage.OperationFailed);
                        }
                    }
                    return await Result<string>.FailAsync($"Supervisor {ResponseMessage.RecordNotFound}");

                }
                return await Result<string>.FailAsync($"Students {ResponseMessage.RecordNotFound}");
            }
        }
    }
}
