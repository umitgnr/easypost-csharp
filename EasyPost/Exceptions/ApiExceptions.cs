using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using EasyPost.Models.Base;
using EasyPost.Utilities;
using RestSharp;

namespace EasyPost.Exceptions
{
    [Serializable]
    internal class ApiException : HttpException
    {
        public readonly string? ApiCode;

        public readonly List<Error>? ApiErrors;

        /// <summary>
        ///     Constructor for an ApiException.
        /// </summary>
        /// <param name="statusCode">HttpStatusCode object.</param>
        /// <param name="apiCode">Error code thrown by the EasyPost API.</param>
        /// <param name="apiErrors">A list of EasyPost.Error instances.</param>
        /// <param name="message">Custom exception message.</param>
        internal ApiException(HttpStatusCode statusCode, string? apiCode, List<Error>? apiErrors, string? message) : base(statusCode, message)
        {
            ApiCode = apiCode;
            ApiErrors = apiErrors;
        }

        /// <summary>
        ///     Constructor for an ApiException.
        /// </summary>
        /// <param name="statusCode">HttpStatusCode object.</param>
        /// <param name="apiCode">Error code thrown by the EasyPost API.</param>
        /// <param name="apiErrors">A list of EasyPost.Error instances.</param>
        /// <param name="message">Custom exception message.</param>
        /// <param name="innerException">Inner exception.</param>
        internal ApiException(Exception innerException, HttpStatusCode statusCode, string? apiCode, List<Error>? apiErrors, string? message) : base(innerException, statusCode, message)
        {
            ApiCode = apiCode;
            ApiErrors = apiErrors;
        }

        /// <summary>
        ///     Load the exception data into a SerializationInfo object instance.
        /// </summary>
        /// <param name="info">SerializationInfo object instance to load data into.</param>
        /// <param name="context">StreamingContext to use for base GetObjectData call.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("API Error Code", ApiCode);
            info.AddValue("API Error Messages", ApiErrors);
        }

        /// <summary>
        ///     Create an ApiException from a RestResponse.
        /// </summary>
        /// <param name="response">RestResponse to create ApiException from.</param>
        /// <param name="innerException">Inner exception.</param>
        /// <returns>An ApiException instance.</returns>
        internal static ApiException FromRestResponse(RestResponse response, Exception? innerException = null)
        {
            string? apiCode = null;
            string? apiErrorMessage;
            List<Error>? errors = null;
            try
            {
                Dictionary<string, Dictionary<string, object>> body = JsonSerialization.ConvertJsonToObject<Dictionary<string, Dictionary<string, object>>>(response.Content);

                apiCode = body["error"]["code"].ToString();
                apiErrorMessage = body["error"]["message"].ToString();
                errors = JsonSerialization.ConvertJsonToObject<List<Error>>(response.Content, null, new List<string>
                {
                    "error",
                    "errors"
                });
            }
            catch (DeserializationException)
            {
                apiErrorMessage = "The EasyPost API threw an error, but the error could not be parsed.";
            }

            return innerException != null ? new ApiException(innerException, response.StatusCode, apiCode, errors, apiErrorMessage) : new ApiException(response.StatusCode, apiCode, errors, apiErrorMessage);
        }
    }
}
