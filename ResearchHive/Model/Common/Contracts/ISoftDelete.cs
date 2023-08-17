using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Common.Contracts
{
    public interface ISoftDelete
    {
        public bool IsDeleted { get; set;  }
    }
}
