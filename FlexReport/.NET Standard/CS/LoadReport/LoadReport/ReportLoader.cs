using C1.Report;

namespace LoadReport
{
    /// <summary>
    /// Loads all reports found inside a <c>.flxr</c> file.
    /// </summary>
    internal class ReportLoader
    {
        /// <summary>
        /// Loads and returns all reports defined in the given <c>.flxr</c> file.
        /// </summary>
        public IReadOnlyList<FlexReport> Load(string flxrPath)
        {
            var reportNames = FlexReport.GetReportList(flxrPath);
            var reports = new List<FlexReport>();

            foreach (var name in reportNames)
            {
                var report = new FlexReport();
                report.Load(flxrPath, name);
                reports.Add(report);
            }

            return reports;
        }
    }
}
