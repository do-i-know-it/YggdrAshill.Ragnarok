using System;
using System.Runtime.Serialization;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    [Serializable]
    public class RagnarokNotAnnotatedException : RagnarokException
    {
        public RagnarokNotAnnotatedException(Type invalidType, string message)
            : base(invalidType, message)
        {

        }

        protected RagnarokNotAnnotatedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
