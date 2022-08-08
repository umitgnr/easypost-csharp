﻿using System.Threading.Tasks;
using EasyPost.Models.API;
using Xunit;

namespace EasyPost.Tests
{
    public class RateTest : UnitTest
    {
        public RateTest() : base("rate")
        {
        }

        [Fact]
        public async Task TestRetrieve()
        {
            UseVCR("retrieve");

            Shipment shipment = await Client.Shipment.Create(Fixture.BasicShipment);

            Rate rate = await Client.Rate.Retrieve(shipment.rates[0].id);

            Assert.IsType<Rate>(rate);
            Assert.StartsWith("rate_", rate.id);
            Assert.Equal(shipment.rates[0], rate);
        }
    }
}
