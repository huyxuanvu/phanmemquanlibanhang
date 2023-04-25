using System;

namespace Controllers
{
    public class InvalidPhoneNumberException : Exception
    {
        public string InvalidPhoneNumber { get; set; }

        public InvalidPhoneNumberException() : base() { }

        public InvalidPhoneNumberException(string message) : base(message) { }

        public InvalidPhoneNumberException(string message, Exception innerException) : base(message, innerException) { } 
        
        public InvalidPhoneNumberException(string message, string invalidPhoneNumber) : base(message) {
            InvalidPhoneNumber = invalidPhoneNumber;
        } 
    }
}
