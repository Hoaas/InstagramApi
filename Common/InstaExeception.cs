using System;
using System.Runtime.Serialization;

namespace Common
{
    [Serializable]
    public class InstaException : Exception
    {
        public InstaException()
        {
        }

        public InstaException(string message) : base(message)
        {
        }

        public InstaException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InstaException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}