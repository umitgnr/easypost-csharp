﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyPost.ApiCompatibility;
using EasyPost.Clients;
using EasyPost.Interfaces;
using Newtonsoft.Json;
using RestSharp;

namespace EasyPost.Models.V2
{
    public class CarrierAccount : EasyPostObject
    {
        [JsonProperty("billing_type")]
        public string? billing_type { get; set; }
        [JsonProperty("credentials")]
        public Dictionary<string, object>? credentials { get; set; }
        [JsonProperty("description")]
        public string? description { get; set; }
        [JsonProperty("readable")]
        public string? readable { get; set; }
        [JsonProperty("reference")]
        public string? reference { get; set; }
        [JsonProperty("test_credentials")]
        public Dictionary<string, object>? test_credentials { get; set; }
        [JsonProperty("type")]
        public string? type { get; set; }


        /// <summary>
        ///     Remove this CarrierAccount from your account.
        /// </summary>
        /// <returns>Whether the request was successful or not.</returns>
        [ApiCompatibility(ApiVersion.V2)]
        public async Task<bool> Delete()
        {
            return await Request(Method.Delete, $"carrier_accounts/{id}");
        }

        /// <summary>
        ///     Update this CarrierAccount.
        /// </summary>
        /// <param name="parameters">See CarrierAccount.Create for more details.</param>
        [ApiCompatibility(ApiVersion.V2)]
        public async Task<CarrierAccount> Update(Dictionary<string, object> parameters)
        {
            return await Update<CarrierAccount>(Method.Patch, $"carrier_accounts/{id}", parameters);
        }
    }
}
