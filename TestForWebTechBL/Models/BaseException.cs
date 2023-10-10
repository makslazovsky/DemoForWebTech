using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestForWebTechBL.Models
{
    public class BaseException : Exception
    {
        public ErrorCodes ErrorCodes { get;}
        public BaseException(ErrorCodes errorCode)
        {
                ErrorCodes = errorCode;
        }
        public BaseException(Exception innerException) : base($"Error code: {ErrorCodes.Unknown}", innerException)
        {
            ErrorCodes = ErrorCodes.Unknown;
        }
    }
}
