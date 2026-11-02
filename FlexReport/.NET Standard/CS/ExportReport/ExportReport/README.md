# ExportReport

Demonstrates how to **export a FlexReport to multiple file formats** — PDF, Excel, HTML, RTF, and Word (.docx).
The user is prompted to choose a format at runtime; the report is then loaded, exported, and opened with its default application.

## What this sample shows

- Locating shared sample data (`.flxr` report and `.mdb` database) in the
  `Documents\ComponentOne Samples\Common` folder
- Loading a report from a `.flxr` file using `FlexReport.Load()`
- Prompting the user to choose an export format at runtime
- Exporting a report using `FlexReport.RenderToFilter()` with:
  - `PdfFilter` → PDF
  - `XlsFilter` → Excel (.xlsx)
  - `HtmlFilter` → HTML
  - `RtfFilter` → RTF
  - `GsWordFilter` → Word (.docx)
- Saving the exported file to the system temp folder and opening it automatically

## Prerequisites

| Requirement | Version |
|---|---|
| .NET SDK | 8.0 or later |
| C1.FlexReport (NuGet) | 10.0.20262.176 or later |
| ComponentOne Samples | Installed under `Documents\ComponentOne Samples\Common` |
| Microsoft Access Database Engine Redistributable (64-bit) | Required for the ACE OleDb provider |

## Sample data

This sample uses the shared ComponentOne sample files:

| File | Location |
|---|---|
| `AlternateBackground.flxr` | `Documents\ComponentOne Samples\Common\Reports\Formatting\` |
| `C1NWind.mdb` | `Documents\ComponentOne Samples\Common\` |

If either file is missing, the app fails at startup with a clear message explaining what to install.

## How to run

1. Open `ExportReport.slnx` in Visual Studio 2022 or later
2. Press **F5** (or `dotnet run` from the project folder)
3. Select a format when prompted — the exported file opens automatically

## Expected console output

```
Select an export format and press enter:
  [1] PDF
  [2] Excel (.xlsx)
  [3] HTML
  [4] RTF
  [5] Docx

Enter choice (1–5): 1

[1/2] Loading report...
[2/2] Exporting as Pdf...

Report saved as Pdf at: C:\Users\<you>\AppData\Local\Temp\ExportReport\AlternateBackground.pdf
```

## Project structure

```
ExportReport/
├── ExportReport.slnx
└── ExportReport/
    ├── ExportReport.csproj
    ├── Program.cs                  Entry point — orchestrates format selection, export, and error handling
    ├── PathResolver.cs             Resolves and validates all input/output paths (fails fast in the constructor)
    ├── ReportLoader.cs             Loads the .flxr and swaps the data source provider to ACE
    ├── ReportExporter.cs           Exports the loaded report using RenderToFilter()
    ├── ExportFormatMenu.cs         Prompts the user and returns the chosen ExportFormat
    └── ExportFormat.cs             Enum of supported formats + file-extension mapping
```
