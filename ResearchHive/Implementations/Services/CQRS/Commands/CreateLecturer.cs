using Application.Abstractions.Data.Auth;
using Application.DTOs.UserDtos;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Model.Entities;
using Model.Enums;
using ResearchHive.Abstractions.Messaging;
using ResearchHive.Constants;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;
using System.Text.Json.Serialization;

namespace Application.Services
{
    public class CreateLecturerRequest
    {
        public record Request(string FirstName, string LastName,
            string EmailAddress, string PhoneNumber, List<Guid> DepartmentIds) : ICommand<Guid>
        {
            [JsonIgnore]
            public Guid CreatedBy { get; set; }
        }

        public record Handler : ICommandHandler<Request, Guid>
        {
            private readonly ILecturerRepository _lecturerRepository;
            private readonly IUserRepository _userRepository;
            private readonly IUserAuthenticationRepository _userAuthenticationRepository;
            private readonly IConfiguration _configuration;
            private readonly IDepartmentRepository _departmentRepository;
            public Handler(ILecturerRepository lecturerRepository, IUserRepository userRepository, IUserAuthenticationRepository userAuthenticationRepository, IDepartmentRepository departmentRepository, IConfiguration configuration)
            {
                _configuration = configuration;
                _lecturerRepository = lecturerRepository;
                _departmentRepository = departmentRepository;
                _userRepository = userRepository;
                _userAuthenticationRepository = userAuthenticationRepository;
            }

            public async Task<Result<Guid>> Handle(Request request, CancellationToken cancellationToken)
            {
                var userExists = (await _userRepository.ExistsAsync(us => us.Email == request.EmailAddress));
                var selectedDepartments = await _departmentRepository.Query(dept => request.DepartmentIds.Contains(dept.Id)).ToListAsync(cancellationToken);
                if (userExists)
                {
                    return await Result<Guid>.FailAsync(ResponseMessage.RecordExist);
                }
                var staffNumber = GenerateStaffNumber(_configuration);
                var userResult = await _userAuthenticationRepository.RegisterUserAsync(
                    new RegisterUserRequestModel
                    {
                        Email = request.EmailAddress,
                        PhoneNumber = request.PhoneNumber,
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Password = staffNumber,
                        Role = Roles.Lecturer,
                    });
                if (userResult.Succeeded)
                {

                    var lecturer = new Lecturer(userResult.Id, staffNumber)
                    {
                        CreatedBy = request.CreatedBy
                    };
                    foreach (var dept in selectedDepartments)
                    {
                        var lecturerDepartment = new LecturerDepartment(lecturer.Id, dept.Id)
                        {
                            CreatedBy = lecturer.Id,
                        };

                        lecturer.LecturerDepartments.Add(lecturerDepartment);
                    }
                    var studentResult = await _lecturerRepository.AddAsync(lecturer);
                    var saveChangesResult = await _lecturerRepository.SaveChangesAsync(cancellationToken);
                    if (saveChangesResult == 1)
                    {
                        return await Result<Guid>.SuccessAsync(ResponseMessage.CreationSuccesful + $" {staffNumber}");
                    }

                    return await Result<Guid>.FailAsync(ResponseMessage.RequestFailed);
                }

                return await Result<Guid>.FailAsync(ResponseMessage.RequestFailed);
            }




            private static string GenerateStaffNumber(IConfiguration configuration)
            {
                configuration = configuration.GetSection("SkC");
                string keyCode = configuration["staff"];
                return $"{keyCode}:{Guid.NewGuid().ToString().Substring(1, 8)}";
            }

        }


    }
}
