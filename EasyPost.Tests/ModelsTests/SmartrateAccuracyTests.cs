﻿using System.Collections.Generic;
using EasyPost.Models.API;
using EasyPost.Tests._Utilities;
using EasyPost.Tests._Utilities.Annotations;
using Xunit;

namespace EasyPost.Tests.ModelsTests
{
    public class SmartRateAccuracyTests : UnitTest
    {
        public SmartRateAccuracyTests() : base("smart_rate_accuracy")
        {
        }

        [Fact]
        [Testing.Function]
        public void TestAll()
        {
            IEnumerable<SmartrateAccuracy> enums = SmartrateAccuracy.All();
            foreach (SmartrateAccuracy? @enum in enums)
            {
                Assert.IsType<SmartrateAccuracy>(@enum);
                Assert.IsType<string>(@enum.Value);
            }
        }
    }
}
