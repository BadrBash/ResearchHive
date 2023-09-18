
using Application.Abstractions.Messaging;
using Application.DTOs;
using Application.Interfaces.Repositories;
using ResearchHive.Constants;
using ResearchHive.Interfaces.Repositories;
using ResearchHive.Wrapper;

namespace Application.Services.Commands.Admin;

public class DeleteAdmin
{

    public record Request(Guid Id) : IQuery<string>;

    public record Handler : IQueryHandler<Request, string>
    {
        private readonly IAdministratorRepository _adminRepository;

        public Handler(IAdministratorRepository adminRepository) =>
        (_adminRepository) = (adminRepository);

        public async Task<Result<string>> Handle(Request request, CancellationToken cancellationToken)
        {
            var admin = await _adminRepository.GetAdministratorAsync(request.Id);
            if (admin is null) return await Result<string>.FailAsync($"admin with id: {request.Id} not found");
            await _adminRepository.DeleteAsync(admin);
            var saveChangesResult = await _adminRepository.SaveChangesAsync(cancellationToken);

            if (saveChangesResult != 0) return await Result<string>.SuccessAsync($" {ResponseMessage.DeleteSuccessful} for admin with id: {request.Id}");
            return await Result<string>.FailAsync(ResponseMessage.OperationFailed);



        }
    }
}
