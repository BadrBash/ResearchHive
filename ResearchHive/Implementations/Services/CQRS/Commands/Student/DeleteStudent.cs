
using Application.Abstractions.Messaging;
using Application.DTOs;
using Application.Interfaces.Repositories;
using ResearchHive.Constants;
using ResearchHive.Implementations.Repositories;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;

namespace Application.Services.Commands.Student;

public class DeleteStudent
{
    public record Request(Guid Id) : IQuery<string>;

    public record Handler : IQueryHandler<Request, string>
    {
        private readonly IStudentRepository _studentRepository;

        public Handler(IStudentRepository studentRepository) =>
        (_studentRepository) = (studentRepository);

        public async Task<Result<string>> Handle(Request request, CancellationToken cancellationToken)
        {
            var student = await _studentRepository.GetByExpressionAsync(usr => usr.Id == request.Id);

            if (student is null) return await Result<string>.FailAsync($"student with id: {request.Id} not found");
            await _studentRepository.DeleteAsync(student);
            var saveChangesResult = await _studentRepository.SaveChangesAsync(cancellationToken);

            if (saveChangesResult == 1) return await Result<string>.SuccessAsync($" {ResponseMessage.DeleteSuccessful} for student with id: {request.Id}");
            return await Result<string>.FailAsync(ResponseMessage.OperationFailed);
        }
    }
}
