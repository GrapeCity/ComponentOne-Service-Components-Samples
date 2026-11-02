# LoadReport

Demonstrates how to **load an existing FlexReport definition** (`.flxr` file) and **inspect its structure** programmatically — listing the data sources, parameters, groups, and all fields grouped by section.

A single `.flxr` file is a container that can hold many report definitions, so the sample loads every report it finds and prints each one.

## What this sample shows

- Locating a shared sample report under the `Documents\ComponentOne Samples` folder
- Listing all reports in a `.flxr` file using `FlexReport.GetReportList()`
- Loading each report using `FlexReport.Load()`
- Inspecting data sources (`DataProvider`, `ConnectionString`, `RecordSource`)
- Inspecting report parameters and their data types
- Inspecting active groups (`GroupBy`, sort expression)
- Iterating over all fields and printing them grouped by section

## Prerequisites

| Requirement | Version |
|---|---|
| .NET SDK | 10.0 or later |
| C1.FlexReport (NuGet) | 10.0.20262.176 or later |
| ComponentOne Samples | Installed under `Documents\ComponentOne Samples` |

## Sample data

This sample uses a report shipped with the ComponentOne WinForms samples:

| File | Location |
|---|---|
| `FlexCommonTasks.flxr` | `Documents\ComponentOne Samples\WinForms\v10.0\FlexReport\CS\ExportFromFlexReportAndPdf\` |

If the folder or the file is missing, the app fails at startup with a clear message rather than an unhandled exception.

## How to run

1. Open `LoadReport.slnx` in Visual Studio 2022 or later
2. Press **F5** (or `dotnet run` from the project folder)
3. The structure of every report in the file is printed to the console

## Expected console output

```
[1/2] Loading reports...
[2/2] Reading 18 report(s)...

┌─ Report: Simple List
│
Data Sources (1)
  [OLEDB]  Main
    Connection : Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|C1NWind.mdb;Persist Security Info=False
    Query      : Employees
│
Parameters (0)
│
Groups (0)
│
Fields (10)
  [Detail]
    Image              Field4
    Text               Field6
    ...
  [Header]
    Text               Field11
    Image              Field12
    ...
  [PageFooter]
    Text               Field25
└──────────────────────────────
┌─ Report: Alternating Background
...
```

## Project structure

```
LoadReport/
├── LoadReport.slnx
└── LoadReport/
    ├── LoadReport.csproj
    ├── .sa3licx                Pre-compiled ComponentOne licence, embedded at build time
    ├── Program.cs              Entry point — resolves the path, loads and reads the reports
    ├── PathResolver.cs         Resolves and validates the sample report path (fails fast in the constructor)
    ├── ReportLoader.cs         Loads all reports from the .flxr file
    └── ReportReader.cs         Reads and prints data sources, parameters, groups, and fields
```

