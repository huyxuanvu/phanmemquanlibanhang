using System;

namespace Controllers
{
    public class InvalidItemNameException : Exception
    {
        public string InvalidItemName { get; set; }

        public InvalidItemNameException() : base() { }
        public InvalidItemNameException(string message) : base(message) { }
        public InvalidItemNameException(string message, Exception innerException) : base(message, innerException) { }
        public InvalidItemNameException(string message, string invalidName) : base(message) { 
            InvalidItemName = invalidName;
        }
    }
}
