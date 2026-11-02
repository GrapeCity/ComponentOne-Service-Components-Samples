using System.Data;

namespace CreateReport;

/// <summary>
/// Builds the employee <see cref="DataTable"/> and persists it as XML
/// so FlexReport can use it as a data source at runtime.
/// </summary>
internal class EmployeeData
{
    private readonly string _xmlPath;

    /// <summary>
    /// Initializes a new instance with the path where the XML data file will be written.
    /// </summary>
    /// <param name="xmlPath">Full path to the output XML file.</param>
    public EmployeeData(string xmlPath)
    {
        _xmlPath = xmlPath;
    }

    /// <summary>
    /// Creates a <see cref="DataTable"/> populated with sample employee records
    /// and writes it to an XML file that FlexReport will read at runtime.
    /// </summary>
    /// <returns>The populated <see cref="DataTable"/>.</returns>
    public DataTable Create()
    {
        var table = new DataTable("Employees");
        table.Columns.Add("Name", typeof(string));
        table.Columns.Add("Department", typeof(string));
        table.Columns.Add("Country", typeof(string));
        table.Columns.Add("Salary", typeof(decimal));

        table.Rows.Add("Alice Johnson", "Engineering", "USA", 95_000m);
        table.Rows.Add("Bob Smith", "Marketing", "UK", 72_000m);
        table.Rows.Add("Clara Lee", "Engineering", "Canada", 88_000m);
        table.Rows.Add("David Kim", "HR", "USA", 65_000m);
        table.Rows.Add("Eva Müller", "Engineering", "Germany", 91_000m);
        table.Rows.Add("Frank Tanaka", "Marketing", "Japan", 78_000m);
        table.Rows.Add("Grace Okafor", "HR", "Nigeria", 61_000m);
        table.Rows.Add("Henry Torres", "Engineering", "Brazil", 85_000m);
        table.WriteXml(_xmlPath, XmlWriteMode.WriteSchema);
        return table;
    }
}
