using System.Collections.Generic;
using System.Threading.Tasks;
using EasyPost.ApiCompatibility;
using EasyPost.Clients;
using EasyPost.Interfaces;
using EasyPost.Models.V2;

namespace EasyPost.Services.V2
{
    public class ApiKeyService : Service
    {
        internal ApiKeyService(Client client) : base(client)
        {
        }

        /// <summary>
        ///     Get a list of all API keys.
        /// </summary>
        /// <returns>A list of EasyPost.ApiKey instances.</returns>
        [ApiCompatibility(ApiVersion.V2)]
        public async Task<List<ApiKey>> All()
        {
            return await List<List<ApiKey>>("api_keys", null, "keys");
        }
    }
}
