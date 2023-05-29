using System;
using System.Runtime.Serialization;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    [Serializable]
    public class RagnarokAnnotationNotFoundException : RagnarokException
    {
        public RagnarokAnnotationNotFoundException(Type invalidType, string message)
            : base(invalidType, message)
        {

        }

        protected RagnarokAnnotationNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
