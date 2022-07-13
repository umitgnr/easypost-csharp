using System.Collections.Generic;
using EasyPost.Interfaces;

namespace EasyPost.Parameters.V2
{
    public static class Billing
    {
        public class Fund : EasyPostParameters
        {
            [Parameter(Necessity.Required, "amount")]
            public string? Amount { internal get; set; }

            public Fund(Dictionary<string, object?>? overrideParameters = null) : base(overrideParameters)
            {
            }

            internal override Dictionary<string, object?>? ToDictionary()
            {
                return ToDictionary(this);
            }
        }
    }
}
