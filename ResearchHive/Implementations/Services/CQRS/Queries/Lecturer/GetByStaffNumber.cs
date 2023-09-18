
using Application.Abstractions.Messaging;
using Application.DTOs;
using Application.Interfaces.Repositories;
using Persistence.Repositories;
using ResearchHive.Wrapper;

namespace Application.Services.Queries.Lecturer;

public class GetByStaffNumber
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
    public record Request(string staffNumber) : IQuery<LecturerDto>;

    public record Handler : IQueryHandler<Request, LecturerDto>
    {
        private readonly ILecturerRepository _lecturerRepository;

        public Handler(ILecturerRepository lecturerRepository) =>
        (_lecturerRepository) = (lecturerRepository);

        public async Task<Result<LecturerDto>> Handle(Request request, CancellationToken cancellationToken)
        {
            var lecturer = await _lecturerRepository.GetAsync(lc => lc.StaffNumber == request.staffNumber && !lc.IsDeleted);
            if (lecturer is null) return await Result<LecturerDto>.FailAsync($"lecturer with Staff Number: {request.staffNumber} not found");
            var lecturerReturned = new LecturerDto
            {
                StaffNumber = lecturer.StaffNumber,
                Email = lecturer.User.Email,
                UserId = lecturer.UserId,
                Id = lecturer.Id,
                UserName = lecturer.User.UserName,
                FirstName = lecturer.User.FirstName,
                LastName = lecturer.User.LastName,
                PhoneNumber = lecturer.User.PhoneNumber,
                UserRoles = lecturer.User.UserRoles.Select(us => us.Role.Name).ToList()
            };
            return await Result<LecturerDto>.SuccessAsync(lecturerReturned, $"lecturer with Staff Number: {request.staffNumber} successfully retrieved");


        }
    }
}
