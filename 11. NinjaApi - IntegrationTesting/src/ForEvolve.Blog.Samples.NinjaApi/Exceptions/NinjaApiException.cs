using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ForEvolve.Blog.Samples.NinjaApi
{
    public class NinjaApiException : Exception
    {
        public NinjaApiException()
        {
        }

        public NinjaApiException(string message) : base(message)
        {
        }

        public NinjaApiException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NinjaApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
