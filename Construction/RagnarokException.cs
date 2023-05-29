using System;
using System.Runtime.Serialization;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    [Serializable]
    public class RagnarokException : Exception
    {
        public Type InvalidType { get; }

        public RagnarokException(Type invalidType)
        {
            InvalidType = invalidType;
        }

        public RagnarokException(Type invalidType, string message)
            : base(message)
        {
            InvalidType = invalidType;
        }

        public RagnarokException(Type invalidType, string message, Exception innerException)
            : base(message, innerException)
        {
            InvalidType = invalidType;
        }

        protected RagnarokException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            InvalidType = (Type)info.GetValue(nameof(InvalidType), typeof(Type));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(InvalidType), InvalidType, typeof(Type));
        }
    }
}
