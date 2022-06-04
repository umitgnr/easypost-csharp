﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyPost.Calculation;
using EasyPost.Interfaces;
using Newtonsoft.Json;
using RestSharp;

namespace EasyPost.Models.V2
{
    public class Shipment : Resource
    {
        [JsonProperty("batch_id")]
        public string batch_id { get; set; }
        [JsonProperty("batch_message")]
        public string batch_message { get; set; }
        [JsonProperty("batch_status")]
        public string batch_status { get; set; }
        [JsonProperty("buyer_address")]
        public Address buyer_address { get; set; }
        [JsonProperty("carrier_accounts")]
        public List<CarrierAccount> carrier_accounts { get; set; }
        [JsonProperty("created_at")]
        public DateTime? created_at { get; set; }
        [JsonProperty("customs_info")]
        public CustomsInfo customs_info { get; set; }
        [JsonProperty("fees")]
        public List<Fee> fees { get; set; }
        [JsonProperty("forms")]
        public List<Form> forms { get; set; }
        [JsonProperty("from_address")]
        public Address from_address { get; set; }
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("insurance")]
        public string insurance { get; set; }
        [JsonProperty("is_return")]
        public bool? is_return { get; set; }
        [JsonProperty("messages")]
        public List<Message> messages { get; set; }
        [JsonProperty("mode")]
        public string mode { get; set; }
        [JsonProperty("options")]
        public Options options { get; set; }
        [JsonProperty("order_id")]
        public string order_id { get; set; }
        [JsonProperty("parcel")]
        public Parcel parcel { get; set; }
        [JsonProperty("postage_label")]
        public PostageLabel postage_label { get; set; }
        [JsonProperty("rates")]
        public List<Rate> rates { get; set; }
        [JsonProperty("reference")]
        public string reference { get; set; }
        [JsonProperty("refund_status")]
        public string refund_status { get; set; }
        [JsonProperty("return_address")]
        public Address return_address { get; set; }
        [JsonProperty("scan_form")]
        public ScanForm scan_form { get; set; }
        [JsonProperty("selected_rate")]
        public Rate selected_rate { get; set; }
        [JsonProperty("service")]
        public string service { get; set; }
        [JsonProperty("status")]
        public string status { get; set; }
        [JsonProperty("tax_identifiers")]
        public List<TaxIdentifier> tax_identifiers { get; set; }
        [JsonProperty("to_address")]
        public Address to_address { get; set; }
        [JsonProperty("tracker")]
        public Tracker tracker { get; set; }
        [JsonProperty("tracking_code")]
        public string tracking_code { get; set; }
        [JsonProperty("updated_at")]
        public DateTime? updated_at { get; set; }
        [JsonProperty("usps_zone")]
        public string usps_zone { get; set; }

        /// <summary>
        ///     Purchase a label for this shipment with the given rate.
        /// </summary>
        /// <param name="rateId">The id of the rate to purchase the shipment with.</param>
        /// <param name="insuranceValue">The value to insure the shipment for.</param>
        public async Task Buy(string rateId, string? insuranceValue = null)
        {
            if (id == null)
            {
                throw new PropertyMissing("id");
            }

            Dictionary<string, object> parameters =
                new Dictionary<string, object>
                {
                    {
                        "rate", new Dictionary<string, object>
                        {
                            {
                                "id", rateId
                            }
                        }
                    }
                };

            if (insuranceValue != null)
            {
                parameters.Add("insurance", insuranceValue);
            }

            Shipment shipment = await Request<Shipment>(Method.Post, $"shipments/{id}/buy", parameters);

            insurance = shipment.insurance;
            postage_label = shipment.postage_label;
            tracking_code = shipment.tracking_code;
            tracker = shipment.tracker;
            selected_rate = shipment.selected_rate;
            forms = shipment.forms;
            messages = shipment.messages;
            fees = shipment.fees;
        }

        /// <summary>
        ///     Purchase a label for this shipment with the given rate.
        /// </summary>
        /// <param name="rate">EasyPost.Rate object instance to purchase the shipment with.</param>
        /// <param name="insuranceValue">The value to insure the shipment for.</param>
        public async Task Buy(Rate rate, string? insuranceValue = null) => await Buy(rate.id, insuranceValue);

        /// <summary>
        ///     Generate a postage label for this shipment.
        /// </summary>
        /// <param name="fileFormat">Format to generate the label in. Valid formats: "pdf", "zpl" and "epl2".</param>
        public async Task GenerateLabel(string fileFormat)
        {
            if (id == null)
            {
                throw new PropertyMissing("id");
            }

            await Update<Shipment>(Method.Get, $"shipments/{id}/label", new Dictionary<string, object>
            {
                {
                    "file_format", fileFormat
                }
            });
        }

        /// <summary>
        ///     Get the Smartrates for this shipment.
        /// </summary>
        /// <returns>A list of EasyPost.Smartrate instances.</returns>
        public async Task<List<Smartrate>> GetSmartrates()
        {
            if (id == null)
            {
                throw new PropertyMissing("id");
            }

            return await Request<List<Smartrate>>(Method.Get, $"shipments/{id}/smartrate", null, "result");
        }

        /// <summary>
        ///     Insure shipment for the given amount.
        /// </summary>
        /// <param name="amount">The amount to insure the shipment for. Currency is provided when creating a shipment.</param>
        public async Task Insure(double amount)
        {
            if (id == null)
            {
                throw new PropertyMissing("id");
            }

            await Update<Shipment>(Method.Post, $"shipments/{id}/insure", new Dictionary<string, object>
            {
                {
                    "amount", amount
                }
            });
        }

        /// <summary>
        ///     Send a refund request to the carrier the shipment was purchased from.
        /// </summary>
        public async Task Refund()
        {
            if (id == null)
            {
                throw new PropertyMissing("id");
            }

            await Update<Shipment>(Method.Get, $"shipments/{id}/refund");
        }

        /// <summary>
        ///     Refresh the rates for this Shipment.
        /// </summary>
        /// <param name="parameters">Optional dictionary of parameters for the API request.</param>
        public async Task RegenerateRates(Dictionary<string, object>? parameters = null)
        {
            if (id == null)
            {
                throw new PropertyMissing("id");
            }

            Shipment shipment = await Request<Shipment>(Method.Post, $"shipments/{id}/rerate", parameters);
            rates = shipment.rates;
        }

        /// <summary>
        ///     Get the lowest rate for this Shipment.
        /// </summary>
        /// <param name="includeCarriers">Carriers to include in the filter.</param>
        /// <param name="includeServices">Services to include in the filter.</param>
        /// <param name="excludeCarriers">Carriers to exclude in the filter.</param>
        /// <param name="excludeServices">Services to exclude in the filter.</param>
        /// <returns>Lowest EasyPost.Rate object instance.</returns>
        public Rate LowestRate(List<string>? includeCarriers = null, List<string>? includeServices = null, List<string>? excludeCarriers = null, List<string>? excludeServices = null)
        {
            return Rates.GetLowestObjectRate(rates, includeCarriers, includeServices, excludeCarriers, excludeServices);
        }

        /// <summary>
        ///     Get the lowest smartrate for this Shipment.
        /// </summary>
        /// <param name="deliveryDays">Delivery days restriction to use when filtering.</param>
        /// <param name="deliveryAccuracy">Delivery days accuracy restriction to use when filtering.</param>
        /// <returns>Lowest EasyPost.Smartrate object instance.</returns>
        public async Task<Smartrate> LowestSmartrate(int deliveryDays, SmartrateAccuracy deliveryAccuracy)
        {
            List<Smartrate> smartrates = await GetSmartrates();
            return Rates.GetLowestShipmentSmartrate(smartrates, deliveryDays, deliveryAccuracy);
        }

        private static void FilterRates(ref List<Rate> rates, Func<Rate, bool> filter) => rates = rates.Where(filter).ToList();
    }
}
