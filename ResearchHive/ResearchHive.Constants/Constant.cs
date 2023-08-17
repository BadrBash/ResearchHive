namespace ResearchHive.Constants
{
    public class ResponseMessage
    {
        public const string OperationRetrievalSuccessful = "Record retrieved successfully";
        public const string RecordNotFound = "Record not found. Record returned empty data";
        public const string CreateSuccessful = "Record successfully created";
        public const string DeleteSuccessful = "Record successfully deleted";
        public const string UpdateSuccessful = "Record successfully updated";
        public const string ApprovalSuccessful = "Record successfully approved";
        public const string SignInValidationFailed = "Password or username incorrect. Try processing again";
        public const string SignInSuccessful =  "You have successfully logged in!";
        public const string AwaitingApproval = "Request awaiting approval";
        public const string OperationFailed = "Oops! Please try processing again";
        public const string RecordExist = "Record already exists";
        public const string NoRecordExist = "No record exist";
        public const string MaximumReached = "Maximum record allowed reached! update/delete from the existing record";
        public const string AlreadyApproved = "This request is already approved with this action";   
        public const string RequestFailed = "Request failed. Try processing again";
        public const string RequestSuccessful = "Request successfully made";
        public const string ApprovedSuccessfully = "This request is approved successfully";
        public const string PasswordChangeSuccessful = "Password changed successfully";
        public const string ContactAdminForAccess = "Access denied from this action. Try contacting admin for access";
        public const string CreationSuccesful = "Registration Successful. Your login key is:";
        public const string UploadSuccessful = "Document(s) successfully uploaded";
        public const string UploadFailed = "Upload Request Failed. Request contains empty file";
    }

    public class ExceptionConstants
    {
        public const string UnsupportedFileType = "File Type Uploaded is not supported. Upload a .docx or .pdf document";
        public const string FileSizeTooLarge = "The File uploaded is larger than the maximum size of 10mb.";
    }
}
