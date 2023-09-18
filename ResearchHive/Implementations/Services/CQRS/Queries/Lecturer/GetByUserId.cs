
using Application.Abstractions.Messaging;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Persistence.Repositories;
using ResearchHive.Wrapper;
using System.Text.Json.Serialization;

namespace Application.Services.Queries.Lecturer;

public class GetByUserId
{
    public class LecturerDto
    {
        public Guid UserId { get; set; }
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string StaffNumber { get; set; }
        public ICollection<string> UserRoles { get; set; }

    }
    public record Request(Guid UserId) : IQuery<LecturerDto>
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
    }

    public record Handler : IQueryHandler<Request, LecturerDto>
    {
        private readonly ILecturerRepository _lecturerRepository;

        public Handler(ILecturerRepository lecturerRepository) =>
        (_lecturerRepository) = (lecturerRepository);

        public async Task<Result<LecturerDto>> Handle(Request request, CancellationToken cancellationToken)
        {
            var lecturer = await _lecturerRepository.GetAsync(lc => lc.UserId == request.UserId);
            if (lecturer is null) return await Result<LecturerDto>.FailAsync($"lecturer with user id: {request.UserId} not found");
            var lecturerReturned = new LecturerDto
            {
                Email = lecturer.User.Email,
                UserId = lecturer.UserId,
                Id = lecturer.Id,
                UserName = lecturer.User.UserName,
                FirstName = lecturer.User.FirstName,
                LastName = lecturer.User.LastName,
                PhoneNumber = lecturer.User.PhoneNumber,
                UserRoles = lecturer.User.UserRoles.Select(us => us.Role.Name).ToList(),
                StaffNumber = lecturer.StaffNumber
            };
            return await Result<LecturerDto>.SuccessAsync(lecturerReturned, $"lecturer with user id: {request.UserId} successfully retrieved");


        }
    }
}
