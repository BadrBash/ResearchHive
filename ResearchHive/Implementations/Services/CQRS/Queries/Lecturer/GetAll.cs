using Application.Abstractions.Messaging;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using ResearchHive.DTOs;
using ResearchHive.Wrapper;

namespace Application.Services.Queries.Lecturer;

public class GetAll
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
    public record Request() : IQuery<IEnumerable<LecturerDto>>;
    
    public record Handler : IQueryHandler<Request, IEnumerable<LecturerDto>>
    {
        private readonly ILecturerRepository _lecturerRepository;

        public Handler(ILecturerRepository lecturerRepository) =>
        (_lecturerRepository) = (lecturerRepository);

        public async Task<Result<IEnumerable<LecturerDto>>> Handle(Request request, CancellationToken cancellationToken)
        {
            var lecturers = await _lecturerRepository.Query()
                .Include(l => l.User)
                .Include(lc => lc.LecturerDepartments)
                .ThenInclude(st => st.Department)
                .ToListAsync();
            var lecturersDTO = lecturers.Select(lecturer => new LecturerDto
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
            });
            if(lecturers.Count() <= 0) return await  Result<IEnumerable<LecturerDto>>.FailAsync("Lecturers' retrieval returned empty data");
            return await  Result<IEnumerable<LecturerDto>>.SuccessAsync(lecturersDTO, "Lecturers' retrieval returned empty data");
           
        }
    }
}
