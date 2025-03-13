
namespace Exceptions
{
    public class InvalidDeveloperIDException: Exception
    {
        public InvalidDeveloperIDException(): base() { }        

        public InvalidDeveloperIDException(string? message): base(message) { }

        public InvalidDeveloperIDException(string? message, Exception? innerException) : base(message, innerException) { }

    }
}
