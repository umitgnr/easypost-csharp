﻿using System.Collections.Generic;
using System.Threading.Tasks;
using EasyPost.Models.API;
using EasyPost.Utilities.Annotations;
using Xunit;

namespace EasyPost.Tests
{
    public class ReportTest : UnitTest
    {
        public ReportTest() : base("report")
        {
        }

        #region CRUD Operations

        [Fact]
        [CrudOperations.Create]
        public async Task TestCreateReport()
        {
            UseVCR("create_report");

            Report report = await CreateBasicReport(Fixture.ReportType);

            Assert.IsType<Report>(report);
            Assert.StartsWith(Fixture.ReportIdPrefix, report.id);
        }

        [Fact]
        [CrudOperations.Create]
        public async Task TestCreateReportWithAdditionalColumns()
        {
            UseVCR("create_report_with_additional_columns");

            List<string> additionalColumns = new List<string>()
            {
                "from_name",
                "from_company"
            };
            Report report = await CreateAdvancedReport("shipment", new Dictionary<string, object>
            {
                {
                    "additional_columns", additionalColumns
                }
            });

            // verify parameters by checking VCR cassette for correct URL
            // Some reports take a long time to generate, so we won't be able to consistently pull the report
            // There's unfortunately no way to check if the columns were included in the final report without parsing the CSV
            // so we assume, if we haven't gotten an error by this point, we've made the API calls correctly
            // any failure at this point is a server-side issue
            Assert.IsType<Report>(report);
        }

        [Fact]
        [CrudOperations.Create]
        public async Task TestCreateReportWithColumns()
        {
            UseVCR("create_report_with_columns");

            List<string> columns = new List<string>()
            {
                "usps_zone"
            };
            Report report = await CreateAdvancedReport("shipment", new Dictionary<string, object>
            {
                {
                    "columns", columns
                }
            });

            // verify parameters by checking VCR cassette for correct URL
            // Some reports take a long time to generate, so we won't be able to consistently pull the report
            // There's unfortunately no way to check if the columns were included in the final report without parsing the CSV
            // so we assume, if we haven't gotten an error by this point, we've made the API calls correctly
            // any failure at this point is a server-side issue
            Assert.IsType<Report>(report);
        }

        [Fact]
        [CrudOperations.Read]
        public async Task TestAll()
        {
            UseVCR("all");

            ReportCollection reportCollection = await Client.Report.All("shipment", new Dictionary<string, object>
            {
                {
                    "page_size", Fixture.PageSize
                }
            });

            List<Report> reports = reportCollection.reports;

            Assert.True(reports.Count <= Fixture.PageSize);
            foreach (Report report in reports)
            {
                Assert.IsType<Report>(report);
            }
        }

        [Fact]
        [CrudOperations.Read]
        public async Task TestRetrieveReport()
        {
            UseVCR("retrieve_report");

            Report report = await CreateBasicReport(Fixture.ReportType);

            Report retrievedReport = await Client.Report.Retrieve(report.id);

            Assert.IsType<Report>(retrievedReport);
            Assert.Equal(report.start_date, retrievedReport.start_date);
            Assert.Equal(report.end_date, retrievedReport.end_date);
        }

        #endregion

        private async Task<Report> CreateAdvancedReport(string reportType, Dictionary<string, object> parameters)
        {
            parameters["start_date"] = Fixture.ReportDate;
            parameters["end_date"] = Fixture.ReportDate;
            return await Client.Report.Create(reportType, parameters);
        }

        private async Task<Report> CreateBasicReport(string reportType) =>
            await Client.Report.Create(reportType, new Dictionary<string, object>
            {
                {
                    "start_date", Fixture.ReportDate
                },
                {
                    "end_date", Fixture.ReportDate
                }
            });
    }
}
