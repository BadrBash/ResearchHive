
using System.ComponentModel;

namespace Model.Enums
{
    public enum ProjectStage
    {
        Started = 1,
        [Description("In Progress")]
        InProgress,
        [Description("Awaiting Approval")]
        AwaitingApproval,
        [Description("Completed And Approved")]
        CompletedAndApproved
    }
}
