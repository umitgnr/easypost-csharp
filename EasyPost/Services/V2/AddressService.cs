using System.Threading.Tasks;
using EasyPost.ApiCompatibility;
using EasyPost.Clients;
using EasyPost.Interfaces;
using EasyPost.Models.V2;
using EasyPost.Parameters;
using EasyPost.Parameters.V2;

namespace EasyPost.Services.V2
{
    public class AddressService : EasyPostService
    {
        internal AddressService(Client client) : base(client)
        {
        }

        /// <summary>
        ///     List all Address objects.
        /// </summary>
        /// <param name="parameters">
        ///     Optional dictionary containing parameters to filter the list with. Valid pairs:
        ///     * {"before_id", string} String representing an Address ID. Starts with "adr_". Only retrieve addresses created
        ///     before this id. Takes precedence over after_id.
        ///     * {"after_id", string} String representing an Address ID. Starts with "adr". Only retrieve addresses created after
        ///     this id.
        ///     * {"start_datetime", string} ISO 8601 datetime string. Only retrieve addresses created after this datetime.
        ///     * {"end_datetime", string} ISO 8601 datetime string. Only retrieve addresses created before this datetime.
        ///     * {"page_size", int} Max size of list. Default to 20.
        ///     All invalid keys will be ignored.
        /// </param>
        /// <returns>An EasyPost.AddressCollection instance.</returns>
        [ApiCompatibility(ApiVersion.Latest)]
        public async Task<AddressCollection> All(All? parameters = null)
        {
            return await List<AddressCollection>("addresses", parameters);
        }

        /// <summary>
        ///     Create an Address.
        /// </summary>
        /// <param name="parameters">
        ///     Optional dictionary containing parameters to create the address with. Valid pairs:
        ///     * {"name", string}
        ///     * {"company", string}
        ///     * {"street1", string}
        ///     * {"street2", string}
        ///     * {"city", string}
        ///     * {"state", string}
        ///     * {"zip", string}
        ///     * {"country", string}
        ///     * {"phone", string}
        ///     * {"email", string}
        ///     * {"verifications", List&lt;string&gt;} Possible items are "delivery" and "zip4".
        ///     * {"strict_verifications", List&lt;string&gt;} Possible items are "delivery" and "zip4".
        ///     All invalid keys will be ignored.
        /// </param>
        /// <returns>EasyPost.Address instance.</returns>
        [ApiCompatibility(ApiVersion.Latest)]
        public async Task<Address> Create(Addresses.Create parameters)
        {
            return await SendCreate("addresses", parameters);
        }

        /// <summary>
        ///     Create and verify an Address.
        /// </summary>
        /// <param name="parameters">
        ///     Optional dictionary containing parameters to create the address with. Valid pairs:
        ///     * {"name", string}
        ///     * {"company", string}
        ///     * {"street1", string}
        ///     * {"street2", string}
        ///     * {"city", string}
        ///     * {"state", string}
        ///     * {"zip", string}
        ///     * {"country", string}
        ///     * {"phone", string}
        ///     * {"email", string}
        ///     All invalid keys will be ignored.
        /// </param>
        [ApiCompatibility(ApiVersion.Latest)]
        public async Task<Address> CreateAndVerify(Addresses.Create parameters)
        {
            return await SendCreate("addresses/create_and_verify", parameters, "address");
        }

        /// <summary>
        ///     Retrieve an Address from its id.
        /// </summary>
        /// <param name="id">String representing an Address. Starts with "adr_".</param>
        /// <returns>EasyPost.Address instance.</returns>
        [ApiCompatibility(ApiVersion.Latest)]
        public async Task<Address> Retrieve(string id)
        {
            return await Get<Address>($"addresses/{id}");
        }

        private async Task<Address> SendCreate(string endpoint, Addresses.Create parameters, string? rootElement = null)
        {
            return await Create<Address>(endpoint, parameters, rootElement);
        }
    }
}
