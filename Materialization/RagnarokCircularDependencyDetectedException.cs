using System;
using System.Runtime.Serialization;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    [Serializable]
    public class RagnarokCircularDependencyDetectedException : RagnarokException
    {
        public RagnarokCircularDependencyDetectedException(Type invalidType, string message)
            : base(invalidType, message)
        {

        }

        protected RagnarokCircularDependencyDetectedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(InvalidType), InvalidType, typeof(Type));
        }
    }
}
