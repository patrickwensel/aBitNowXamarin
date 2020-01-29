using System;

namespace ABitNow.Exceptions
{
    public class ApplicationExceptionEx : Exception
    {
        public Exception ex { get; set; }
        public ApplicationExceptionEx(Exception exception)
        {
            ex = exception;
        }
    }
}
