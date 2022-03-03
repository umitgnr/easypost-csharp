# EasyPost .Net Client Library

[![CI](https://github.com/EasyPost/easypost-csharp/workflows/CI/badge.svg)](https://github.com/EasyPost/easypost-csharp/actions?query=workflow%3ACI)
[![NuGet](https://img.shields.io/nuget/v/EasyPost-Official)](https://www.nuget.org/packages/EasyPost-Official)

EasyPost is a simple shipping API. You can sign up for an account at https://easypost.com

## Documentation

Up-to-date documentation at: https://www.easypost.com/docs/api/csharp

## Installation

The easiest way to add EasyPost to your project is with the NuGet package manager.

```
Install-Package EasyPost-Official
```

See NuGet docs for instructions on installing via the [dialog](http://docs.nuget.org/docs/start-here/managing-nuget-packages-using-the-dialog) or the [console](http://docs.nuget.org/docs/start-here/using-the-package-manager-console).

## Usage

The EasyPost API consists of many object types. There are several attributes that are consistent across all objects:

* `id` -- Guaranteed unique identifier of the object.
* `created_at`/`updated_at`  -- Timestamps of creation and last update time.

### Configuration

If you are operating with a single EasyPost API key, during the initialization of your application add the following to configure EasyPost.

```cs
using EasyPost;

ClientManager.SetCurrent("ApiKey");
```

If you are operating with multiple EasyPost API keys, or wish to delegate the construction of the client requests, configure the `ClientManager` with a delegate at application initialization.

```cs
using EasyPost;

ClientManager.SetCurrent(() => new Client(new ClientConfiguration("yourApiKeyHere")));
```

### [Address Verification](https://www.easypost.com/docs/api/csharp#create-and-verify-addresses)

An `Address` can be verified using one or many verifications [methods](https://www.easypost.com/docs/api/csharp#verifications-object). If `Address` is created without strict verifications the object will still be created, otherwise an `HttpException` will be raised.

```cs
using EasyPost;

Address address = new Address() {
    company = "Simpler Postage Inc",
    street1 = "164 Townsend Street",
    street2 = "Unit 1",
    city = "San Francisco",
    state = "CA",
    country = "US",
    zip = "94107",
    verify = new List<string>() { "delivery" }
};

address.Create();

if (address.verifications.delivery.success) {
    // successful verification
} else {
    // unsuccessful verification
}
```

```cs
using EasyPost;

Address address = new Address() {
    company = "Simpler Postage Inc",
    street1 = "164 Townsend Street",
    street2 = "Unit 1",
    city = "San Francisco",
    state = "CA",
    country = "US",
    zip = "94107",
    verify_strict = new List<string>() { "delivery" }
};

try {
    address.Create();
} except (HttpException) {
    // unsuccessful verification
}

// successful verification
```

### [Rating](https://www.easypost.com/docs/api/csharp#rates)

Rating is available through the `Shipment` object. Since we do not charge for rating there are rate limits for this action if you do not eventually purchase the `Shipment`. Please contact us at support@easypost.com if you have any questions.

```cs
Address fromAddress = new Address() { zip = "14534" };
Address toAddress = new Address() { zip = "94107" };

Parcel parcel = new Parcel() {
    length = 8,
    width = 6,
    height = 5,
    weight = 10
};

Shipment shipment = new Shipment() {
    from_address = fromAddress,
    to_address = toAddress,
    parcel = parcel
};

shipment.Create();

foreach (Rate rate in shipment.rates) {
    // process rates
}
```

### [Postage Label](https://www.easypost.com/docs/api/csharp#buy-a-shipment) Generation

Purchasing a shipment will generate a `PostageLabel` and any customs `Form`s that are needed for shipping.

```cs
Address fromAddress = new Address() { id = "adr_..." };
Address toAddress = new Address() {
    company = "EasyPost",
    street1 = "164 Townsend Street",
    street2 = "Unit 1",
    city = "San Francisco",
    state = "CA",
    country = "US",
    zip = "94107"
};

Parcel parcel = new Parcel() {
    length = 8,
    width = 6,
    height = 5,
    weight = 10
};

CustomsItem item = new CustomsItem() { description = "description" };
CustomsInfo info = new CustomsInfo() {
    customs_certify = "TRUE",
    eel_pfc = "NOEEI 30.37(a)",
    customs_items = new List<CustomsItem>() { item }
};

Options options = new Options() { label_format = "PDF" };

Shipment shipment = new Shipment() {
    from_address = fromAddress,
    to_address = toAddress,
    parcel = parcel,
    customs_info = info,
    options = options
};

shipment.Buy(shipment.LowestRate(
    includeServices: new List<string>() { "Priority" },
    excludeCarriers: new List<string>() { "USPS" }
));

shipment.postage_label.url; // https://easypost-files.s3-us-west-2.amazonaws.com/files/postage_label/20160826/8e77c397d47b4d088f1c684b7acd802a.png

foreach (Form form in shipment.forms) {
    // process forms
}
```

### Testing

All .NET/.NET Core versions of the library utilize a VCR tool (`Scotch`) to record and replay HTTP requests and responses via cassettes. These cassettes are used by our .NET/.NET Core unit tests; no live API calls are made once a cassette is recorded and the test is set to "replay" mode.

The .NET Framework version of our library is incompatible with `Scotch`. As a result, live API calls are made during .NET Framework unit testing.

Most unit tests can use a `Test` API key. Unit tests for `CarrierAccount` and `User` classes require `Production` API keys.

#### Test Coverage

Unit test coverage reports can be generated by running the `generate_test_reports.sh` Bash script from the root of this repository.

A report will be generated for each version of the library (.NET Core 3.1, .NET 5.0, .NET 6.0, and .NET Framework 3.5). The script may fail if one or more of the versions is unable to be tested on your machine (i.e. attempting to run a .NET Framework build on a non-Windows machine).

Final reports will be stored in the `coveragereport` folder in the root of the repository following generation.

The script requires the following tools installed in your PATH:
- `dotnet`
- [`reportgenerator`](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-code-coverage?tabs=linux#generate-reports)

### Warning about Threads

NOTE: The EasyPost .NET client library (in particular, the `ClientManager` global object) is not threadsafe; do not attempt to perform requests from multiple threads in parallel. This can be particularly problematic if using multiple API keys; make sure to always use a Mutex, Monitor, or other synchronization method to ensure that concurrent requests do not enter the EasyPost library from different threads.

### Reporting Issues

If you have an issue with the client feel free to open an issue on [GitHub](https://github.com/EasyPost/easypost-csharp/issues). If you have a general shipping question or a questions about EasyPost's service please contact support@easypost.com for additional assitance.
