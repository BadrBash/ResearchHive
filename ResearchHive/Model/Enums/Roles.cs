using System.ComponentModel;

namespace Model.Enums
{
    public  enum Roles
    {
        [Description("Administrator")]
        Administrator = 1,
        [Description("Sub Administrator")]
        SubAdministrator,
        [Description("Supervisor")]
        Supervisor,
        [Description("Lecturer")]
        Lecturer,
        [Description("Student")]
        Student,
    }
}
