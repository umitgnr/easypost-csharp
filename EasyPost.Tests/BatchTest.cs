﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EasyPost.Models.API;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace EasyPost.Tests
{
    public class BatchTest : UnitTest
    {
        public BatchTest() : base("batch")
        {
        }

        [Fact]
        public async Task TestAddRemoveShipment()
        {
            UseVCR("add_remove_shipment");

            Shipment shipment = await Client.Shipment.Create(Fixture.OneCallBuyShipment);

            Batch batch = await Client.Batch.Create();

            batch = await batch.AddShipments(new List<Shipment>
            {
                shipment
            });

            Assert.AreEqual(1, batch.num_shipments);

            batch = await batch.RemoveShipments(new List<Shipment>
            {
                shipment
            });

            Assert.AreEqual(0, batch.num_shipments);
        }

        [Fact]
        public async Task TestAll()
        {
            UseVCR("all");

            BatchCollection batchCollection = await Client.Batch.All(new Dictionary<string, object?>
            {
                {
                    "page_size", Fixture.PageSize
                }
            });

            List<Batch> batches = batchCollection.batches;

            Assert.IsTrue(batches.Count <= Fixture.PageSize);
            Assert.IsNotNull(batchCollection.HasMore);
            foreach (Batch item in batches)
            {
                Assert.IsInstanceOfType(item, typeof(Batch));
            }
        }

        [Fact]
        public async Task TestBuy()
        {
            UseVCR("buy");

            Batch batch = await CreateOneCallBuyBatch();

            batch = await batch.Buy();

            Assert.IsInstanceOfType(batch, typeof(Batch));
            Assert.AreEqual(1, batch.num_shipments);
        }

        [Fact]
        public async Task TestCreate()
        {
            UseVCR("create");

            Batch batch = await CreateBasicBatch();

            Assert.IsInstanceOfType(batch, typeof(Batch));
            Assert.IsTrue(batch.id.StartsWith("batch_"));
            Assert.IsNotNull(batch.shipments);
        }

        [Fact]
        public async Task TestCreateAndBuy()
        {
            UseVCR("create_and_buy");

            Batch batch = await Client.Batch.CreateAndBuy(new Dictionary<string, object?>
            {
                {
                    "shipments", new List<Dictionary<string, object?>>
                    {
                        Fixture.OneCallBuyShipment
                    }
                }
            });

            Assert.IsInstanceOfType(batch, typeof(Batch));
            Assert.IsTrue(batch.id.StartsWith("batch_"));
            Assert.AreEqual(1, batch.num_shipments);
        }

        [Fact]
        public async Task TestCreateScanForm()
        {
            UseVCR("create_scan_form");


            Batch batch = await CreateOneCallBuyBatch();

            batch = await batch.Buy();

            if (IsRecording())
            {
                Thread.Sleep(15000); // Wait enough time to process
            }

            batch = await batch.GenerateScanForm();

            // We can't assert anything meaningful here because the scanform gets queued for generation and may not be immediately available
            Assert.IsInstanceOfType(batch, typeof(Batch));
        }

        [Fact]
        public async Task TestLabel()
        {
            UseVCR("label");

            Batch batch = await CreateOneCallBuyBatch();

            batch = await batch.Buy();

            if (IsRecording())
            {
                Thread.Sleep(15000); // Wait enough time to process
            }

            await batch.GenerateLabel("ZPL");

            // We can't assert anything meaningful here because the label gets queued for generation and may not be immediately available
            Assert.IsInstanceOfType(batch, typeof(Batch));
        }

        [Fact]
        public async Task TestRetrieve()
        {
            UseVCR("retrieve");

            Batch batch = await CreateBasicBatch();

            Batch retrievedBatch = await Client.Batch.Retrieve(batch.id);

            Assert.IsInstanceOfType(retrievedBatch, typeof(Batch));
            // Must compare IDs since elements of batch (i.e. status) may be different
            Assert.AreEqual(batch.id, retrievedBatch.id);
        }

        private async Task<Batch> CreateBasicBatch() =>
            await Client.Batch.Create(new Dictionary<string, object?>
            {
                {
                    "shipments", new List<Dictionary<string, object?>>
                    {
                        Fixture.BasicShipment
                    }
                }
            });

        private async Task<Batch> CreateOneCallBuyBatch() =>
            await Client.Batch.Create(new Dictionary<string, object?>
            {
                {
                    "shipments", new List<Dictionary<string, object?>>
                    {
                        Fixture.OneCallBuyShipment
                    }
                }
            });
    }
}
