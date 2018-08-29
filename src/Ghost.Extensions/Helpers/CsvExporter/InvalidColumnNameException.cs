using System;
using System.Runtime.Serialization;

namespace Ghost.Extensions.Helpers.CsvExporter
{
    [Serializable]
    internal class InvalidColumnNameException : Exception
    {
        public InvalidColumnNameException()
        {
        }

        public InvalidColumnNameException(string message) : base(message)
        {
        }

        public InvalidColumnNameException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidColumnNameException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}