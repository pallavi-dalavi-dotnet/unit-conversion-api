# Unit Conversion API

## Overview

A RESTful API built with C# and ASP.NET Core Web API to convert values between different units of measurement.

The API supports length, temperature, and weight conversions, along with input validation and error handling.

## Technologies

- C#
- ASP.NET Core Web API
- .NET 10
- Swagger / OpenAPI
- xUnit

## Features

- Convert values between supported units.
- Support length, temperature, and weight.
- Retrieve the supported units through an API endpoint.
- Validate unsupported units and incompatible categories.
- Reject temperatures below absolute zero.
- Use dependency injection and a separate conversion service.
- Include automated unit tests.

## Project Structure

```text
UnitConversionApi/
├── src/
│   └── UnitConversion.Api/
│       ├── Controllers/
│       ├── Models/
│       ├── Services/
│       └── Program.cs
├── tests/
│   └── UnitConversion.Tests/
│       └── UnitTest1.cs
├── UnitConversionApi.slnx
└── README.md
```

## Prerequisites

- .NET 10 SDK
- Git (for version control)

Check your installed .NET version:

```bash
dotnet --version
```

## Run the Project

Run these commands from the project root directory.

Restore dependencies:

```bash
dotnet restore UnitConversionApi.slnx
```

Build the solution:

```bash
dotnet build UnitConversionApi.slnx
```

Run the tests:

```bash
dotnet test UnitConversionApi.slnx
```

Start the API:

```bash
dotnet run --project src/UnitConversion.Api/UnitConversion.Api.csproj
```

Open the Swagger URL printed in the terminal and add `/swagger` to explore the endpoints.

## API Endpoints

### Convert a value

**Method:** POST

**Endpoint:** `/api/Conversions`

Request example:

```json
{
  "value": 10,
  "fromUnit": "meter",
  "toUnit": "foot"
}
```

Expected response:

```json
{
  "originalValue": 10,
  "fromUnit": "meter",
  "toUnit": "foot",
  "convertedValue": 32.80839895,
  "category": "length"
}
```

### Get supported units

**Method:** GET

**Endpoint:** `/api/Conversions/units`

Returns supported units grouped by category.

## Supported Categories

- **Length:** meter, kilometer, centimeter, millimeter, foot, inch, mile
- **Temperature:** Celsius, Fahrenheit, Kelvin
- **Weight:** kilogram, gram, milligram, pound, ounce, tonne

## Error Handling

The API returns HTTP 400 for invalid conversion requests, including unsupported units, incompatible categories, and temperatures below absolute zero.

## Design Decisions

- Controllers handle HTTP requests, and the conversion service handles conversion logic.
- Dependency injection keeps the service separate from the controller.
- Length and weight conversions use a common base unit.
- Temperature conversions use formulas specific to each scale.
- Conversion results are rounded to eight decimal places.
- xUnit tests verify conversions and error scenarios.

## Future Improvements

- Add additional unit categories.
- Store unit definitions in configuration or a database.
- Add integration tests.
- Configure a CI/CD pipeline.
