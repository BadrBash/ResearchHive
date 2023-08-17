using Application.Abstractions.Data.Auth;
using Application.DTOs.UserDtos;
using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Model.Entities;
using Model.Enums;
using Model.Extensions;
using ResearchHive.Abstractions.Messaging;
using ResearchHive.Constants;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;
using System.Text.Json.Serialization;

namespace Application.Services
{
    public class CreateStudentRequest
    {
        public record Request(string FirstName, string LastName, string Password,
            string EmailAddress, string PhoneNumber, Level Level, string MatricNumber, Guid DepartmentId) : ICommand<Guid>
        {
            [JsonIgnore]
            public Guid CreatedBy { get; set; }
        }

        public record Handler : ICommandHandler<Request, Guid>
        {
            
            private readonly IStudentRepository _studentRepository;
            private readonly IDepartmentRepository _departmentRepository;
            private readonly IUserRepository _userRepository;
            private readonly IUserAuthenticationRepository _userAuthenticationRepository;

            public Handler( IStudentRepository studentRepository,
                IUserRepository userRepository, IUserAuthenticationRepository userAuthenticationRepository, IDepartmentRepository departmentRepository)
            {
               
                _studentRepository = studentRepository;
                _departmentRepository = departmentRepository;
                _userRepository = userRepository;
                _userAuthenticationRepository = userAuthenticationRepository;
            }
            public async Task<Result<Guid>> Handle(Request request, CancellationToken cancellationToken)
            {
                var userExists = (await _userRepository.ExistsAsync(us => us.Email == request.EmailAddress));
                var studentExists = (await _studentRepository.ExistsAsync(st => st.MatricNumber == request.MatricNumber && 
                st.IsActiveStudent));
                var studentExistsForUpdate = (await _studentRepository.Query(st => st.MatricNumber == request.MatricNumber && 
                (!st.IsActiveStudent) && st.Level != request.Level).SingleOrDefaultAsync(cancellationToken));
                var departmentExists = await _departmentRepository.ExistsAsync(dept => dept.Id == request.DepartmentId);
                if(studentExistsForUpdate != null)
                {
                    var student = studentExistsForUpdate.ActivateStudentship(request.Level);
                    await _studentRepository.UpdateAsync(student);
                    var updateResult = await _studentRepository.SaveChangesAsync(cancellationToken);
                    if(updateResult == 1)
                    {
                        return await Result<Guid>.SuccessAsync($"{ResponseMessage.RecordExist} but has been successfully updated to new Level: {request.Level.GetDescription()}");
                    }
                }
                if (userExists || studentExists)
                {
                    return await Result<Guid>.FailAsync(ResponseMessage.RecordExist);
                }
                if(!departmentExists)
                {
                    return await Result<Guid>.FailAsync(ResponseMessage.RecordNotFound);
                }
                var userResult = await _userAuthenticationRepository.RegisterUserAsync(
                    new RegisterUserRequestModel
                    {
                        Email = request.EmailAddress,
                        PhoneNumber = request.PhoneNumber,
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Password = request.Password,
                        Role = Roles.Student,
                    });
                if (userResult.Succeeded)
                {

                    var student = new Student(request.DepartmentId, userResult.Id, request.MatricNumber, request.Level)
                    {
                        CreatedBy = request.CreatedBy,
                    };

                    var studentResult = await _studentRepository.AddAsync(student);
                    var saveChangesResult = await _studentRepository.SaveChangesAsync();
                    if (saveChangesResult == 1)
                    {
                        return await Result<Guid>.SuccessAsync(ResponseMessage.CreateSuccessful + $" Id: {studentResult.Id}");
                    }

                    return await Result<Guid>.FailAsync(ResponseMessage.RequestFailed);
                }

                return await Result<Guid>.FailAsync(ResponseMessage.RequestFailed);
            }
        }
    }
}
