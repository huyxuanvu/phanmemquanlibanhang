using System;

namespace Controllers
{
    public class InvalidEmailException : Exception
    {
        public string InvalidEmail { get; set; }

        public InvalidEmailException() : base() { }

        public InvalidEmailException(string message) : base(message) { }    

        public InvalidEmailException(string message, Exception innerException) : base(message, innerException) { }

        public InvalidEmailException(string message, string invalidEmail) : base(message) {
            InvalidEmail = invalidEmail;
        }
    }
}
