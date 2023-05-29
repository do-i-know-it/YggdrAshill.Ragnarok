using System;
using System.Runtime.Serialization;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    [Serializable]
    public class RagnarokNotRegisteredException : RagnarokException
    {
        public RagnarokNotRegisteredException(Type invalidType)
            : base(invalidType, $"{invalidType} not found.")
        {

        }

        public RagnarokNotRegisteredException(Type invalidType, string message)
            : base(invalidType, message)
        {

        }

        protected RagnarokNotRegisteredException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
