using Application.Abstractions.Messaging;
using Application.Interfaces.Repositories;
using Model.Entities;
using ResearchHive.Abstractions.Messaging;
using ResearchHive.Constants;
using ResearchHive.Wrapper;
using System.Text.Json.Serialization;

namespace Application.Services
{
    public class CreateDepartmentRequest
    {
        public record Request(string Name, string Description) : ICommand<Guid>
        {
            [JsonIgnore]
            public Guid CreatedBy { get; set; }
        }

        public record Handler : ICommandHandler<Request, Guid>
        {
            private readonly IDepartmentRepository _departmentRepository;

            public Handler(IDepartmentRepository departmentRepository)
            {
                _departmentRepository = departmentRepository;
            }

            public async Task<Result<Guid>> Handle(Request request, CancellationToken cancellationToken)
            {
                var deptExists = await _departmentRepository.ExistsAsync(dep => dep.Name == request.Name);
                if(deptExists)
                {
                    return await Result<Guid>.FailAsync(ResponseMessage.RecordExist);
                }
                

                var department = new Department(request.Name, request.Description)
                {
                    CreatedBy = request.CreatedBy
                };
                var departmentCreate = await _departmentRepository.AddAsync(department);
                var saveChangesResult = await _departmentRepository.SaveChangesAsync();

                if(saveChangesResult == 1)
                {
                    return await Result<Guid>.SuccessAsync(ResponseMessage.CreateSuccessful);
                }

                return await Result<Guid>.FailAsync(ResponseMessage.OperationFailed);
            }
        }

    }
}
