using System;
using System.Threading.Tasks;

namespace EasyPost.Tests
{
    /// <summary>
    ///     Base class for all unit tests.
    ///     Sets up all available client versions for VCR and non-VCR requests.
    /// </summary>
    public class UnitTest : IDisposable
    {
        private readonly TestUtils.VCR? _vcr;

        protected Client Client;

        private string? _cleanupId;

        /// <summary>
        ///     An asynchronous function executed after every test.
        ///     Function should have a string (ID) passed in, and should return a boolean.
        /// </summary>
        protected Func<string, Task<bool>>? CleanupFunction { get; set; }

        protected UnitTest(string groupName, TestUtils.ApiKey apiKey = TestUtils.ApiKey.Test) => _vcr = new TestUtils.VCR(groupName, apiKey);

        public void Dispose()
        {
            if (CleanupFunction == null)
            {
                return;
            }

            if (_cleanupId == null)
            {
                return;
            }

            try
            {
                CleanupFunction.Invoke(_cleanupId).GetAwaiter().GetResult();
                _cleanupId = null;
            }
            catch
            {
                throw new Exception("Could not execute clean-up function.");
            }
        }

        /// <summary>
        ///     Set the ID that will be passed into the CleanupFunction after each unit test
        /// </summary>
        /// <param name="id"></param>
        protected void CleanUpAfterTest(string id) => _cleanupId = id;

        protected bool IsRecording() => _vcr != null && _vcr.IsRecording();

        /// <summary>
        ///     Skip running cleanup after the unit test
        ///     Helpful to avoid double deletion confusion for VCR
        /// </summary>
        protected void SkipCleanUpAfterTest() => _cleanupId = null;

        /// <summary>
        ///     Set up all clients to make live calls.
        /// </summary>
        /// <param name="apiKey"></param>
        protected void UseLive(string apiKey)
        {
            Client = TestUtils.GetClient(apiKey);
        }

        /// <summary>
        ///     Set up all clients to use the VCR.
        /// </summary>
        /// <param name="cassetteName"></param>
        /// <param name="overrideApiKey"></param>
        protected void UseVCR(string cassetteName, string? overrideApiKey = null)
        {
            Client = _vcr?.SetUpTest(cassetteName, overrideApiKey)!;
        }
    }
}
