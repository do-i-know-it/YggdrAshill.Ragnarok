using System;
using System.Runtime.Serialization;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    [Serializable]
    public class RagnarokAlreadyAnnotatedException : RagnarokException
    {
        public RagnarokAlreadyAnnotatedException(Type invalidType, string message)
            : base(invalidType, message)
        {

        }

        protected RagnarokAlreadyAnnotatedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
