namespace ResearchHive.DTOs
{
    public class StudentDTO
    {
            public Guid UserId { get; set; }
            public string UserName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Level { get; set; }
            public string MatricNumber { get; set; }
            public string ActiveStatus { get; set; }
           public string Department { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public ICollection<string> UserRoles { get; set; }

    }
}
