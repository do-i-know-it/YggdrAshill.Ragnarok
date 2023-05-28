using System;
using System.Runtime.Serialization;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    [Serializable]
    public class RagnarokArgumentException : RagnarokException
    {
        public RagnarokArgumentException(Type invalidType, string message)
            : base(invalidType, message)
        {

        }

        protected RagnarokArgumentException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
