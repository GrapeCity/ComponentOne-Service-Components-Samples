using C1.Report;

namespace LoadReport
{
    /// <summary>
    /// Reads <see cref="FlexReport"/> and prints its full structure to the console —
    /// data sources, parameters, groups, and fields grouped by section.
    /// </summary>
    internal class ReportReader
    {
        /// <summary>
        /// Reads the given report and prints all found elements to the console.
        /// </summary>
        public void Read(FlexReport report)
        {
            Print($"┌─ Report: {report.ReportName}");

            ReadDataSources(report);
            ReadParameters(report);
            ReadGroups(report);
            ReadFields(report);

            Print("└──────────────────────────────");
        }

        /// <summary>
        /// Prints all data sources configured on the report.
        /// </summary>
        private void ReadDataSources(FlexReport report)
        {
            Print("│");
            Print($"Data Sources ({report.DataSources.Count})");

            foreach (DataSource ds in report.DataSources)
            {
                Print($"  [{ds.DataProvider}]  {ds.Name}");

                if (!string.IsNullOrWhiteSpace(ds.ConnectionString))
                    Print($"    Connection : {ds.ConnectionString}");

                if (!string.IsNullOrWhiteSpace(ds.RecordSource))
                    Print($"    Query      : {ds.RecordSource}");
            }
        }

        /// <summary>
        /// Prints all report parameters and their data types.
        /// </summary>
        private void ReadParameters(FlexReport report)
        {
            Print("│");
            Print($"Parameters ({report.Parameters.Count})");

            foreach (ReportParameter p in report.Parameters)
                Print($"  {p.Name,-30} [{p.DataType}]");
        }

        /// <summary>
        /// Prints all active groups — those with a non-empty GroupBy expression.
        /// </summary>
        private void ReadGroups(FlexReport report)
        {
            var activeGroups = report.Groups
                .Cast<Group>()
                .Where(g => !string.IsNullOrWhiteSpace(g.GroupBy))
                .ToList();

            Print("│");
            Print($"Groups ({activeGroups.Count})");

            for (int i = 0; i < activeGroups.Count; i++)
            {
                var group = activeGroups[i];
                var sort = string.IsNullOrWhiteSpace(group.SortExpression) ? group.Sort.ToString() : group.SortExpression;
                Print($"  Group {i + 1}: GroupBy = {group.GroupBy}  Sort = {sort}");
            }
        }

        /// <summary>
        /// Prints all fields in the report, grouped by their section type.
        /// </summary>
        private void ReadFields(FlexReport report)
        {
            // FieldCollection is non-generic so we enumerate via foreach
            var fields = new List<FieldBase>();
            foreach (FieldBase field in report.Fields)
                fields.Add(field);

            // Fields not assigned to a section are skipped, so count what is actually printed.
            var assignedFields = fields.Where(f => f.Section != SectionTypeEnum.Undefined).ToList();

            var bySection = assignedFields
                .GroupBy(f => f.Section)
                .OrderBy(g => (int)g.Key);

            Print("│");
            Print($"Fields ({assignedFields.Count})");

            foreach (var group in bySection)
            {
                Print($"  [{group.Key}]");
                foreach (var field in group)
                {
                    var typeName = field.GetType().Name.Replace("Field", "");
                    Print($"    {typeName,-18} {field.Name}");
                }
            }
        }

        /// <summary>
        /// Prints a message.
        /// </summary>
        private void Print(string message) => Console.WriteLine(message);
    }

}
