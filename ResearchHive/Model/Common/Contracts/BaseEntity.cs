using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Common.Contracts
{
    public class BaseEntity : AuditableEntity
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
    }
}
