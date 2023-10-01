using System;
using System.Runtime.Serialization;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    [Serializable]
    public class RagnarokCircularDependencyException : RagnarokException
    {
        public RagnarokCircularDependencyException(Type invalidType) :
            base(invalidType, $"Circular dependency detected in {invalidType}.")
        {

        }

        public RagnarokCircularDependencyException(Type invalidType, string message)
            : base(invalidType, message)
        {

        }

        protected RagnarokCircularDependencyException(SerializationInfo info, StreamingContext context)
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
