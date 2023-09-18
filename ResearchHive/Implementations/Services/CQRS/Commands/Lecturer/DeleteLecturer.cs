
using Application.Abstractions.Messaging;
using Application.DTOs;
using Application.Interfaces.Repositories;
using ResearchHive.Constants;
using ResearchHive.Implementations.Repositories;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;
using System.Text.Json.Serialization;

namespace Application.Services.Commands.Lecturer;

public class DeleteLecturer
{

    public record Request() : IQuery<string>
    {
        [JsonIgnore] public Guid Id { get; set; }
    }

    public record Handler : IQueryHandler<Request, string>
    {
        private readonly ILecturerRepository _LecturerRepository;

        public Handler(ILecturerRepository LecturerRepository) =>
        (_LecturerRepository) = (LecturerRepository);

        public async Task<Result<string>> Handle(Request request, CancellationToken cancellationToken)
        {
            var Lecturer = await _LecturerRepository.GetAsync(usr => usr.UserId == request.Id);

            if (Lecturer is null) return await Result<string>.FailAsync($"Lecturer with id: {request.Id} not found");
             await _LecturerRepository.DeleteAsync(Lecturer);
            var saveChangesResult = await _LecturerRepository.SaveChangesAsync(cancellationToken);

            if (saveChangesResult == 1) return await Result<string>.SuccessAsync($" {ResponseMessage.DeleteSuccessful} for Lecturer with id: {request.Id}");
            return await Result<string>.FailAsync(ResponseMessage.OperationFailed);



        }
    }
}
