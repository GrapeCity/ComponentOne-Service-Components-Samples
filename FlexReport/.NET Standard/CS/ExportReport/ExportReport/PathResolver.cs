namespace ExportReport
{

    /// <summary>
    /// Resolves all file and directory paths used by the sample.
    /// </summary>
    internal class PathResolver
    {
        private readonly string _commonDir;

        public PathResolver()
        {
            _commonDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), @"ComponentOne Samples");

            if (!Directory.Exists(_commonDir)) throw new DirectoryNotFoundException($"The ComponentOne sample data folder was not found: {_commonDir}");

            if (!File.Exists(FlxrPath)) throw new FileNotFoundException($"Report file not found: {FlxrPath}");

            if (!File.Exists(MdbPath)) throw new FileNotFoundException($"Database file not found: {MdbPath}");
        }

        /// <summary>Gets the path to the <c>.flxr</c> report definition file.</summary>
        public string FlxrPath => Path.Combine(_commonDir, @"WinForms\v10.0\FlexReport\CS\FlexReportExplorer\Reports\Formatting\AlternateBackground.flxr");

        /// <summary>Gets the path to the Access database used by the report's data source.</summary>
        public string MdbPath => Path.Combine(_commonDir, @"WinForms\v10.0\FlexReport\CS\FlexReportExplorer\Data\C1NWind.mdb");

        /// <summary>Gets the directory where exported files will be saved.</summary>
        public string OutputDir => Path.Combine(Path.GetTempPath(), "ExportReport");

        /// <summary>
        /// Builds the full output file path from the report name and chosen format.
        /// </summary>
        public string BuildOutputPath(string reportName, ExportFormat format)
            => Path.Combine(OutputDir, reportName + format.ToFileExtension());
    }

}
