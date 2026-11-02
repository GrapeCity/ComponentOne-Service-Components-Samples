using C1.Report;

namespace ExportReport
{

    /// <summary>
    /// Loads a report from a <c>.flxr</c> file.
    /// </summary>
    internal class ReportLoader
    {
        /// <summary>
        /// Loads the first report definition from the given <c>.flxr</c> file and updates
        /// its data source to point to the provided Access database.
        /// </summary>
        /// <param name="flxrPath">Full path to the <c>.flxr</c> file.</param>
        /// <param name="mdbPath">Full path to the <c>.mdb</c> database file.</param>
        /// <returns>A loaded <see cref="FlexReport"/> ready to export.</returns>
        public FlexReport Load(string flxrPath, string mdbPath)
        {
            var reportNames = FlexReport.GetReportList(flxrPath);
            if (reportNames.Length == 0)
                throw new InvalidOperationException($"No reports found in: {flxrPath}");
            var report = new FlexReport();
            report.Load(flxrPath, reportNames[0]);
            report.DataSource.ConnectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={mdbPath}";

            return report;
        }
    }

}
