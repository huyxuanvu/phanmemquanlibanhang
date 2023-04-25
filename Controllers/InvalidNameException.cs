using System;

namespace Controllers
{
    public class InvalidNameException : Exception
    {
        public string InvalidName { get; set; }

        public InvalidNameException() : base() { }

        public InvalidNameException(string message) : base(message) { }

        public InvalidNameException(string message, Exception innerException) : base(message, innerException) { }

        public InvalidNameException(string message, string invalidName) : base(message) { 
            InvalidName = invalidName;
        }
    }
}
