﻿using System.Threading.Tasks;
using EasyPost.Clients;
using EasyPost.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyPost.Tests
{
    [TestClass]
    public class ParcelTest
    {
        private TestUtils.VCR _vcr;

        [TestInitialize]
        public void Initialize()
        {
            _vcr = new TestUtils.VCR("parcel");
        }

        private static async Task<Parcel> CreateBasicParcel(V2Client v2Client)
        {
            return await v2Client.Parcels.Create(Fixture.BasicParcel);
        }

        [TestMethod]
        public async Task TestCreate()
        {
            V2Client v2Client = _vcr.SetUpTest("create");

            Parcel parcel = await CreateBasicParcel(v2Client);

            Assert.IsInstanceOfType(parcel, typeof(Parcel));
            Assert.IsTrue(parcel.id.StartsWith("prcl_"));
            Assert.AreEqual(15.4, parcel.weight);
        }

        [TestMethod]
        public async Task TestRetrieve()
        {
            V2Client v2Client = _vcr.SetUpTest("retrieve");

            Parcel parcel = await CreateBasicParcel(v2Client);

            Parcel retrievedParcel = await v2Client.Parcels.Retrieve(parcel.id);

            Assert.IsInstanceOfType(retrievedParcel, typeof(Parcel));
            Assert.AreEqual(parcel, retrievedParcel);
        }
    }
}
