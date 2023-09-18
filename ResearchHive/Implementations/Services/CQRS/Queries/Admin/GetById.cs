
using Application.Abstractions.Messaging;
using Application.DTOs;
using Application.Interfaces.Repositories;
using ResearchHive.Wrapper;
using System.Text.Json.Serialization;

namespace Application.Services.Queries.Admin;

public class GetById
{
    public class AdminDto
    {
        public Guid UserId { get; set; }
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<string> UserRoles { get; set; }

    }
    public record Request() : IQuery<AdminDto>
    {
        [JsonIgnore] public Guid Id { get; set;}
    }

    public record Handler : IQueryHandler<Request, AdminDto>
    {
        private readonly IAdministratorRepository _adminRepository;

        public Handler(IAdministratorRepository adminRepository) =>
        (_adminRepository) = (adminRepository);

        public async Task<Result<AdminDto>> Handle(Request request, CancellationToken cancellationToken)
        {
            var admin = await _adminRepository.GetAdministratorAsync(request.Id);
            if (admin is null) return await Result<AdminDto>.FailAsync($"admin with id: {request.Id} not found");
            var adminReturned = new AdminDto
            {
                Email = admin.User.Email,
                UserId = admin.UserId,
                Id = admin.Id,
                UserName = admin.User.UserName,
                FirstName = admin.User.FirstName,
                LastName = admin.User.LastName,
                PhoneNumber = admin.User.PhoneNumber,
                UserRoles = admin.User.UserRoles.Select(us => us.Role.Name).ToList()
            };
            return await Result<AdminDto>.SuccessAsync(adminReturned, $"admin with id: {request.Id} successfully retrieved");


        }
    }
}
