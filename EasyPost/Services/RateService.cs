using System.Threading.Tasks;
using EasyPost.Interfaces;
using EasyPost.Models;

namespace EasyPost.Services
{
    public class RateService : Service
    {
        internal RateService(Client client) : base(client)
        {
        }

        /// <summary>
        ///     Retrieve a Rate from its id.
        /// </summary>
        /// <param name="id">String representing a rate. Starts with `rate_`.</param>
        /// <returns>EasyPost.Rate instance.</returns>
        public async Task<Rate> Retrieve(string id)
        {
            return await Get<Rate>($"rates/{id}");
        }
    }
}
