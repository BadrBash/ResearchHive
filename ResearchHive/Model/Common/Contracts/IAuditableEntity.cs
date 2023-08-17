using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Common.Contracts
{
    public  interface IAuditableEntity
    {
         Guid? CreatedBy { get; set; }
         Guid? UpdatedBy { get; set;}
         DateTime CreatedDate { get; set; }
         DateTime LastModifiedDate { get; set;}
    }
}
