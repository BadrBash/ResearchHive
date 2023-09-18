
using Application.Abstractions.Messaging;
using Application.DTOs;
using Application.Interfaces.Repositories;
using ResearchHive.Constants;
using ResearchHive.Implementations.Repositories;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;

namespace Application.Services.Commands.Department;

public class DeleteDepartment
{

    public record Request(Guid Id) : IQuery<string>;

    public record Handler : IQueryHandler<Request, string>
    {
        private readonly IDepartmentRepository _DepartmentRepository;

        public Handler(IDepartmentRepository DepartmentRepository) =>
        (_DepartmentRepository) = (DepartmentRepository);

        public async Task<Result<string>> Handle(Request request, CancellationToken cancellationToken)
        {
            var Department = await _DepartmentRepository.GetAsync(dept => dept.Id == request.Id);

            if (Department is null) return await Result<string>.FailAsync($"Department with id: {request.Id} not found");
            await _DepartmentRepository.DeleteAsync(Department);
            var saveChangesResult = await _DepartmentRepository.SaveChangesAsync(cancellationToken);

            if (saveChangesResult == 1) return await Result<string>.SuccessAsync($" {ResponseMessage.DeleteSuccessful} for Department with id: {request.Id}");
            return await Result<string>.FailAsync(ResponseMessage.OperationFailed);
        }
    }
}
