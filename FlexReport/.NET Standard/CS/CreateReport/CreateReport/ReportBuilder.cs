using System.Data;
using System.Drawing;
using C1.Report;

namespace CreateReport;

/// <summary>
/// Builds a FlexReport Employee Directory report from a <see cref="DataTable"/>.
/// Each report section (header, detail, footer, etc.) is constructed by a dedicated method.
/// </summary>
internal class ReportBuilder
{
    private readonly DataTable _data;
    private readonly string _xmlPath;

    // Column layout: (header label, data expression, left position, width, alignment)
    // Positions and widths are in twips (1 inch = 1440 twips).
    private readonly (string Label, string Expr, int Left, int Width, FieldAlignEnum Align)[] _columns =
    [
        ("Name",       "=Name",                   180,  3000, FieldAlignEnum.LeftMiddle),
        ("Department", "=Department",             3380, 2500, FieldAlignEnum.LeftMiddle),
        ("Country",    "=Country",                6080, 1300, FieldAlignEnum.LeftMiddle),
        ("Salary",     "=Format(Salary, \"C0\")", 7580, 1600, FieldAlignEnum.RightMiddle),
    ];

    /// <summary>
    /// Initializes a new instance with the data and XML path needed to configure the report data source.
    /// </summary>
    /// <param name="data">Employee data table returned by <see cref="EmployeeData.Create"/>.</param>
    /// <param name="xmlPath">Path to the XML file FlexReport will read at runtime.</param>
    public ReportBuilder(DataTable data, string xmlPath)
    {
        _data = data;
        _xmlPath = xmlPath;
    }

    /// <summary>
    /// Builds and returns the complete <see cref="FlexReport"/> with all sections configured.
    /// </summary>
    /// <returns>A fully configured <see cref="FlexReport"/> ready to be saved.</returns>
    public FlexReport Build()
    {
        var report = new FlexReport();
        report.ReportName = "Employee Report";
        report.DataSource.DataProvider = DataProvider.XmlFile;
        report.DataSource.ConnectionString = _xmlPath;
        report.DataSource.RecordSource = _data.TableName;

        AddReportHeader(report);
        AddPageHeader(report);
        AddDetail(report);
        AddPageFooter(report);
        AddReportFooter(report);

        return report;
    }

    /// <summary>
    /// Adds the report header section — displays the report title once at the very top.
    /// </summary>
    void AddReportHeader(FlexReport report)
    {
        Console.WriteLine("      - Report header");
        var section = report.Sections[SectionTypeEnum.Header];
        section.Visible = true;
        section.Height = 900;

        var title = new TextField
        {
            Name = "lblTitle",
            Text = "Employee Directory",
            Left = 0,
            Top = 60,
            Width = 9360,
            Height = 720,
            Align = FieldAlignEnum.CenterMiddle
        };
        title.Font.Size = 22;
        title.Font.Bold = true;
        section.Fields.Add(title);
    }

    /// <summary>
    /// Adds the page header section — displays column labels on every page.
    /// </summary>
    void AddPageHeader(FlexReport report)
    {
        Console.WriteLine("      - Page header");
        var section = report.Sections[SectionTypeEnum.PageHeader];
        section.Visible = true;
        section.Height = 450;
        section.BackColor = Color.FromArgb(224, 224, 224);

        foreach (var (label, _, left, width, align) in _columns)
        {
            var hdr = new TextField
            {
                Name = "hdr" + label,
                Text = label,
                Left = left,
                Top = 60,
                Width = width,
                Height = 330,
                Align = align
            };
            hdr.Font.Size = 10;
            hdr.Font.Bold = true;
            section.Fields.Add(hdr);
        }
    }

    /// <summary>
    /// Adds the detail section — renders one row per employee record from the data source.
    /// </summary>
    void AddDetail(FlexReport report)
    {
        Console.WriteLine("      - Detail rows");
        var section = report.Sections[SectionTypeEnum.Detail];
        section.Visible = true;
        section.Height = 360;

        foreach (var (_, expr, left, width, align) in _columns)
        {
            var fld = new TextField
            {
                Name = "fld" + expr.TrimStart('='),
                Text = expr,  // e.g. "=Name" binds to the Name column
                Left = left,
                Top = 40,
                Width = width,
                Height = 280,
                Align = align
            };
            section.Fields.Add(fld);
        }
    }

    /// <summary>
    /// Adds the page footer section — displays the current page number on every page.
    /// </summary>
    void AddPageFooter(FlexReport report)
    {
        Console.WriteLine("      - Page footer");
        var section = report.Sections[SectionTypeEnum.PageFooter];
        section.Visible = true;
        section.Height = 360;

        var page = new TextField
        {
            Name = "lblPage",
            Text = "=\"Page \" & [Page] & \" of \" & [Pages]",  // [Page]/[Pages] are built-in FlexReport variables
            Left = 5760,
            Top = 30,
            Width = 3420,
            Height = 300,
            Align = FieldAlignEnum.RightMiddle
        };
        page.Font.Size = 8;
        section.Fields.Add(page);
    }

    /// <summary>
    /// Adds the report footer section — displays summary statistics once at the end of the report.
    /// Uses FlexReport aggregate functions: <c>Count()</c> and <c>Avg()</c>.
    /// </summary>
    void AddReportFooter(FlexReport report)
    {
        Console.WriteLine("      - Report footer");
        var section = report.Sections[SectionTypeEnum.Footer];
        section.Visible = true;
        section.Height = 480;

        var total = new TextField
        {
            Name = "lblTotal",
            Text = "=\"Total employees: \" & Count(Name)",
            Left = 180,
            Top = 90,
            Width = 4000,
            Height = 300,
            Align = FieldAlignEnum.LeftMiddle
        };
        total.Font.Size = 10;
        total.Font.Bold = true;
        section.Fields.Add(total);

        var avgSalary = new TextField
        {
            Name = "lblAvgSalary",
            Text = "=\"Avg salary: \" & Format(Avg(Salary), \"C0\")",
            Left = 4500,
            Top = 90,
            Width = 4680,
            Height = 300,
            Align = FieldAlignEnum.RightMiddle
        };
        avgSalary.Font.Size = 10;
        avgSalary.Font.Bold = true;
        section.Fields.Add(avgSalary);
    }
}
