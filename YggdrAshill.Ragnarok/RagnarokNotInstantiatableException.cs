using YggdrAshill.Ragnarok.Construction;
using System;
using System.Runtime.Serialization;

namespace YggdrAshill.Ragnarok
{
    // TODO: add document comments.
    [Serializable]
    public class RagnarokNotInstantiatableException : RagnarokException
    {
        public RagnarokNotInstantiatableException(Type invalidType)
            : base(invalidType, $"{invalidType} is not instantiatable.")
        {

        }

        protected RagnarokNotInstantiatableException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
