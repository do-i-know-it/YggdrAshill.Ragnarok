using YggdrAshill.Ragnarok.Construction;
using System;
using System.Runtime.Serialization;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    [Serializable]
    public class RagnarokMultipleAnnotationFoundException : RagnarokException
    {
        public RagnarokMultipleAnnotationFoundException(Type invalidType, string message)
            : base(invalidType, message)
        {

        }

        protected RagnarokMultipleAnnotationFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
