using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.CustomExceptions
{
    public class InvalidCommandException : BaseException
    {
        public InvalidCommandException() : base() { }
        public InvalidCommandException(string message) : base(message) { }
        
    }
}
