using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.CustomExceptions
{
    public class InvalidSyntaxException : BaseException
    {
        public InvalidSyntaxException() : base()
        {
            
        }
        public InvalidSyntaxException(string message) : base(message) { }
        
    }
}
