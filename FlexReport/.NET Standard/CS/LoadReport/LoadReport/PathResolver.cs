namespace LoadReport
{
    /// <summary>
    /// Resolves and validates all file paths used by the sample.
    /// </summary>
    internal class PathResolver
    {
        private readonly string _samplesDir;

        public PathResolver()
        {
            _samplesDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), @"ComponentOne Samples");

            if (!Directory.Exists(_samplesDir)) throw new DirectoryNotFoundException($"The ComponentOne sample data folder was not found: {_samplesDir}");

            if (!File.Exists(FlxrPath)) throw new FileNotFoundException($"Report file not found: {FlxrPath}");
        }

        /// <summary>Gets the path to the <c>.flxr</c> report definition file.</summary>
        public string FlxrPath => Path.Combine(_samplesDir, @"WinForms\v10.0\FlexReport\CS\ExportFromFlexReportAndPdf\FlexCommonTasks.flxr");
    }
}
