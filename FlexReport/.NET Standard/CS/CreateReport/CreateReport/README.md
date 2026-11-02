# CreateReport

Demonstrates how to build a complete FlexReport definition entirely in C# code — no designer, no template file — and save it as a `.flxr` file.

## What this sample shows

- Creating a `FlexReport` and configuring all five section types:
  `Header` · `PageHeader` · `Detail` · `PageFooter` · `Footer`
- Using `TextField` instances to display static text, data-bound values, and aggregate values
- Binding the report to an **XML data source** (written alongside the `.flxr` at runtime)
- Using data-bound field expressions (`=Name`, `=Salary`) and aggregate expressions (`Count(Name)`, `Avg(Salary)`)

## Prerequisites

| Requirement | Version |
|---|---|
| .NET SDK | 10.0 or later |
| C1.FlexReport (NuGet) | 10.0.20262.176 or later |

## How to run

1. Open `CreateReport.sln` in Visual Studio 2022 or later
2. Press **F5** (or `dotnet run` from the project folder)
3. The output is written to the system temp folder: `%TEMP%\EmployeeReport\`
4. Open `EmployeeReport.flxr` in **FlexReport Designer** or **FlexViewer** to view the report

## Expected console output

```
[1/3] Building employee data...
[2/3] Building report sections...
      - Report header
      - Page header
      - Detail rows
      - Page footer
      - Report footer
[3/3] Saving report...
Report saved to: <temp>\EmployeeReport\EmployeeReport.flxr
Open it with FlexViewer or FlexReport Designer to view the report.
```

## Output files

Two files are written to the temp folder:

| File | Description |
|---|---|
| `EmployeeReport.flxr` | Report definition (open in FlexReport Designer or FlexViewer) |
| `EmployeeReport.xml` | Employee data consumed by the report at runtime |

## Project structure

```
CreateReport/
├── Program.cs          Entry point — sets up paths, runs the pipeline, prints progress
├── EmployeeData.cs     Builds the DataTable and writes it as XML
└── ReportBuilder.cs    Constructs all five report sections using TextField directly
```
