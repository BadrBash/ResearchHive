namespace Application.DTOs.UserDtos
{
    public class DeleteUserReponseModel : BaseResponse<string>
    {
        public bool IsDeleted { get; set; } = false;

    }
}