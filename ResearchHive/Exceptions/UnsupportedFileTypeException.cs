using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{

    public class UnsupportedFileTypeException : Exception
    {
        public UnsupportedFileTypeException(string message) : base(message)
        {
        }
    }
}
