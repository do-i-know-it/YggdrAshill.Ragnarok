using System;
using System.Runtime.Serialization;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    [Serializable]
    public class RagnarokAlreadyRegisteredException : RagnarokException
    {
        public RagnarokAlreadyRegisteredException(Type invalidType)
            : base(invalidType, $"Conflict implementation type : {invalidType}.")
        {

        }

        public RagnarokAlreadyRegisteredException(Type invalidType, string message)
            : base(invalidType, message)
        {

        }

        protected RagnarokAlreadyRegisteredException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
