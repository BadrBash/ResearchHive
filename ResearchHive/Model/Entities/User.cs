

using Model.Common.Contracts;

namespace Model.Entities
{
    public class User : BaseEntity
    {
        public User(string userName, string email, string password, string firstName, string lastName, string phoneNumber)
        {
            UserName = userName;
            Email = email;
            Password = password;
            UserRoles = new HashSet<UserRole>();
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
        }

        public string UserName {get; private set;}
        public string FirstName {get; private set;}
        public string LastName {get; private set;}
        public string Email{ get; private set;}
        public string Password {get; private set;}
        public string PhoneNumber { get; private set; }
        public ICollection<UserRole> UserRoles {get; private set;}
        public Student? Student { get; private set;}
        public Lecturer? Lecturer {get; private set;}
        public Administrator? Administrator { get; private set;}


        public User Update(string firstName, string lastName, string email, string phoneNumber)
        {

            PhoneNumber = phoneNumber ?? PhoneNumber;
            FirstName = firstName ?? FirstName;
            LastName = lastName ?? LastName;
           Email = email ?? Email;
            return this;
        }
    }
}