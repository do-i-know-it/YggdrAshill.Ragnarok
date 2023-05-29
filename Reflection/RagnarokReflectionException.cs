using System;
using System.Runtime.Serialization;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    [Serializable]
    public class RagnarokReflectionException : RagnarokException
    {
        public RagnarokReflectionException(Type invalidType, string message)
            : base(invalidType, message)
        {

        }

        protected RagnarokReflectionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
