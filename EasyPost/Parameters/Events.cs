using System.Collections.Generic;
using EasyPost._base;

namespace EasyPost.Parameters
{
    public static class Events
    {
        public sealed class Create : ApiParameters
        {
            public Create(Dictionary<string, object?>? overrideParameters = null) : base(overrideParameters)
            {
            }

            internal override Dictionary<string, object?>? ToDictionary()
            {
                return ToDictionary(this);
            }
        }
    }
}
