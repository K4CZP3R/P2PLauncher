using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PLauncher.Exceptions
{
    class EnvironmentErrorException : Exception
    {
        public EnvironmentErrorException(string message) : base(message)
        { }
    }
}
