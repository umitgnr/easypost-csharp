﻿using System.Collections.Generic;
using System.Threading.Tasks;
using EasyPost.ApiCompatibility;
using EasyPost.Clients;
using EasyPost.Interfaces;
using EasyPost.Parameters.V2;
using Newtonsoft.Json;
using RestSharp;

namespace EasyPost.Models.V2
{
    public class Batch : EasyPostObject
    {
        [JsonProperty("error")]
        public string? Error { get; set; }
        [JsonProperty("label_url")]
        public string? LabelUrl { get; set; }
        [JsonProperty("message")]
        public string? Message { get; set; }
        [JsonProperty("num_shipments")]
        public int NumShipments { get; set; }
        [JsonProperty("reference")]
        public string? Reference { get; set; }
        [JsonProperty("scan_form")]
        public ScanForm? ScanForm { get; set; }
        [JsonProperty("shipments")]
        public List<BatchShipment>? Shipments { get; set; }
        [JsonProperty("state")]
        public string? State { get; set; }
        [JsonProperty("status")]
        public Dictionary<string, int>? Status { get; set; }

        /// <summary>
        ///     Add shipments to this batch.
        /// </summary>
        /// <param name="parameters">UpdateShipmentParameters</param>
        [ApiCompatibility(ApiVersion.Latest)]
        public async Task<Batch> AddShipments(Batches.UpdateShipments parameters) => await Update<Batch>(Method.Post, $"batches/{Id}/add_shipments", parameters);

        /// <summary>
        ///     Add shipments to this batch.
        /// </summary>
        /// <param name="shipmentIds">List of shipment ids to be added.</param>
        [ApiCompatibility(ApiVersion.Latest)]
        public async Task<Batch> AddShipments(List<string> shipmentIds)
        {
            var parameters = new Batches.UpdateShipments
            {
                ShipmentIds = shipmentIds
            };
            return await AddShipments(parameters);
        }

        /// <summary>
        ///     Add shipments to this batch.
        /// </summary>
        /// <param name="shipmentsToAdd">List of Shipment objects to be added.</param>
        [ApiCompatibility(ApiVersion.Latest)]
        public async Task<Batch> AddShipments(IEnumerable<Shipment> shipmentsToAdd)
        {
            List<string> shipmentIds = new List<string>();
            foreach (Shipment shipment in shipmentsToAdd)
            {
                if (shipment.Id == null)
                {
                    continue;
                }

                shipmentIds.Add(shipment.Id);
            }

            return await AddShipments(shipmentIds);
        }

        /// <summary>
        ///     Purchase all shipments within this batch. The Batch's state must be "created" before purchasing.
        /// </summary>
        [ApiCompatibility(ApiVersion.Latest)]
        public async Task<Batch> Buy()
        {
            return await Update<Batch>(Method.Post, $"batches/{Id}/buy");
        }

        /// <summary>
        ///     Asynchronously generate a label containing all of the Shipment labels belonging to this batch.
        /// </summary>
        /// <param name="fileFormat">Format to generate the label in. Valid formats: "pdf", "zpl" and "epl2".</param>
        [ApiCompatibility(ApiVersion.Latest)]
        public async Task<Batch> GenerateLabel(Batches.Label parameters)
        {
            return await Update<Batch>(Method.Post, $"batches/{Id}/label", parameters);
        }

        /// <summary>
        ///     Asynchronously generate a scan from for this batch.
        /// </summary>
        [ApiCompatibility(ApiVersion.Latest)]
        public async Task<Batch> GenerateScanForm()
        {
            return await Update<Batch>(Method.Post, $"batches/{Id}/scan_form");
        }

        /// <summary>
        ///     Remove shipments to this batch.
        /// </summary>
        /// <param name="parameters">UpdateShipmentParameters</param>
        [ApiCompatibility(ApiVersion.Latest)]
        public async Task<Batch> RemoveShipments(Batches.UpdateShipments parameters) => await Update<Batch>(Method.Post, $"batches/{Id}/remove_shipments", parameters);

        /// <summary>
        ///     Remove shipments to this batch.
        /// </summary>
        /// <param name="shipmentIds">List of shipment ids to be removed.</param>
        [ApiCompatibility(ApiVersion.Latest)]
        public async Task<Batch> RemoveShipments(List<string> shipmentIds)
        {
            var parameters = new Batches.UpdateShipments
            {
                ShipmentIds = shipmentIds
            };
            return await RemoveShipments(parameters);
        }

        /// <summary>
        ///     Remove shipments to this batch.
        /// </summary>
        /// <param name="shipmentsToAdd">List of Shipment objects to be removed.</param>
        [ApiCompatibility(ApiVersion.Latest)]
        public async Task<Batch> RemoveShipments(IEnumerable<Shipment> shipmentsToAdd)
        {
            List<string> shipmentIds = new List<string>();
            foreach (Shipment shipment in shipmentsToAdd)
            {
                if (shipment.Id == null)
                {
                    continue;
                }

                shipmentIds.Add(shipment.Id);
            }

            return await RemoveShipments(shipmentIds);
        }
    }
}
