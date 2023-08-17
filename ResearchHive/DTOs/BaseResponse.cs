namespace Application.DTOs
{
    public class BaseResponse<T>
    {
        public string Message { get; set; }

        public bool Status { get; set; } = false;

        public List<string> ValidationResults { get; set; } = new List<string>();

    }

    public class BaseResponse
    {
        public string Message { get; set; }

        public bool Status { get; set; } = false;

        public List<string> ValidationResults { get; set; } = new List<string>();

    }
   
}
