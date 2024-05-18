using System;
using System.Runtime.Serialization;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    [Serializable]
    public class RagnarokAlreadySelectedException : RagnarokException
    {
        public RagnarokAlreadySelectedException(Type invalidType, string message)
            : base(invalidType, message)
        {

        }

        protected RagnarokAlreadySelectedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
