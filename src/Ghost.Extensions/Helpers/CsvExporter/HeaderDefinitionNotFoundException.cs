using System;
using System.Runtime.Serialization;

namespace Ghost.Extensions.Helpers.CsvExporter
{
    [Serializable]
    internal class HeaderDefinitionNotFoundException : Exception
    {
        public HeaderDefinitionNotFoundException()
        {
        }

        public HeaderDefinitionNotFoundException(string message) : base(message)
        {
        }

        public HeaderDefinitionNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected HeaderDefinitionNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}