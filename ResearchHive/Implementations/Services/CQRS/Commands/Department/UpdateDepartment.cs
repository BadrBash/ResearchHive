
using Application.Abstractions.Messaging;
using Application.DTOs;
using Application.Interfaces.Repositories;
using ResearchHive.Constants;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;
using System.Text.Json.Serialization;

namespace Application.Services.Commands.Department;

public class UpdateDepartment
{
    public class RequestDto
    {
        public string Name { get;  set; }
        public string Description { get;  set; }
        [JsonIgnore]
        public Guid UpdatedBy { get; set; }

    }
    public record Request(Guid Id, RequestDto Model) : IQuery<Guid>;

    public record Handler : IQueryHandler<Request, Guid>
    {
        private readonly IDepartmentRepository _DepartmentRepository;

        public Handler(IDepartmentRepository DepartmentRepository) =>
        (_DepartmentRepository) = (DepartmentRepository);

        public async Task<Result<Guid>> Handle(Request request, CancellationToken cancellationToken)
        {
            var Department = await _DepartmentRepository.GetAsync(dept => dept.Id == request.Id);
            if (Department is null) return await Result<Guid>.FailAsync($"Department with id: {request.Id} not found");
            var updatedDept=  Department?.Update(request.Model.Name, request.Model.Description);
            updatedDept.UpdatedBy = request.Model.UpdatedBy;
            var updateRequest = await _DepartmentRepository.UpdateAsync(updatedDept);
            var saveChangesResult = await _DepartmentRepository.SaveChangesAsync(cancellationToken);

            if (saveChangesResult == 1) return await Result<Guid>.SuccessAsync(updateRequest.Id, $" {ResponseMessage.UpdateSuccessful} for Department with id: {request.Id}");
            return await Result<Guid>.FailAsync(ResponseMessage.OperationFailed);
           


        }
    }
}
