namespace ExportReport
{
    /// <summary>
    /// Prompts the user to choose an export format from a numbered menu.
    /// </summary>
    internal class ExportFormatMenu
    {
        /// <summary>
        /// Displays the menu and returns the chosen <see cref="ExportFormat"/>.
        /// Keeps prompting until a valid choice is entered.
        /// </summary>
        public ExportFormat SelectExportFormat()
        {
            Console.WriteLine("Select an export format and press enter:");
            Console.WriteLine("  [1] PDF");
            Console.WriteLine("  [2] Excel (.xlsx)");
            Console.WriteLine("  [3] HTML");
            Console.WriteLine("  [4] RTF");
            Console.WriteLine("  [5] Word (.docx)");
            Console.WriteLine();

            while (true)
            {
                Console.Write("Enter choice (1–5): ");
                var input = Console.ReadLine()?.Trim();

                switch (input)
                {
                    case "1": return ExportFormat.Pdf;
                    case "2": return ExportFormat.Excel;
                    case "3": return ExportFormat.Html;
                    case "4": return ExportFormat.Rtf;
                    case "5": return ExportFormat.Docx;
                    default:
                        Console.WriteLine("Invalid choice. Please enter 1, 2, 3, 4, or 5.");
                        break;
                }
            }
        }
    }

}
