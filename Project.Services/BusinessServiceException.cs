using System;

namespace Project.Services
{
    [Serializable]
    public class BusinessServiceException : Exception
    {
        public BusinessServiceException()
        {
        }

        public BusinessServiceException(string message) : base(message)
        {
        }

        public BusinessServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
