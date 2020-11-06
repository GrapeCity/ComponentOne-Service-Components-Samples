using C1.TextParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ExtractQuotedText
{
    public class MyExtractionResultClassAux
    {
        [DataMember(Name = StartsAfterContinuesUntil.ExtractedText)]
        public string Text { get; set; }

        [DataMember(Name = StartsAfterContinuesUntil.StartIndex)]
        public int Index { get; set; }
    }

    public class MyExtractionResultClass
    {
        [DataMember(Name = StartsAfterContinuesUntil.ExtractedValues)]
        public List<MyExtractionResultClassAux> Result { get; set; }
    }

    public class Program
    {
        public static void Main()
        {
            C1.TextParser.LicenseManager.Key = License.Key;

            StartsAfterContinuesUntil startsAfterContinuesUntil = new StartsAfterContinuesUntil(@"""", @"""");
            Stream inputStream = File.Open(@"input.txt", FileMode.Open);
            IExtractionResult extractedResult = startsAfterContinuesUntil.Extract(inputStream);
            inputStream.Close();

            Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("GrapeCity, inc, all rights reserved");
            Console.WriteLine("Demo of the C1TextParser library - StartsAfterContinuesUntil extractor sample");
            Console.WriteLine("Test case: Extract all the quoted text from an excerpt of \"The Picture of Dorian Gray\" by Oscar Wilde");
            Console.WriteLine("           (public domain book)");
            Console.WriteLine("Extractor specification: Starts After \" Continues Until \"");
            Console.WriteLine("Detail: The input stream content, as well as the extracted result (in Json format) are displayed down below");
            Console.WriteLine("        Also, the extracted result was exported to \"ExtractQuotedText.csv\" at the current working directory");
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------");

            Console.WriteLine("");

            Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("Input stream:");
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
            Console.WriteLine(new StreamReader(File.Open(@"input.txt", FileMode.Open)).ReadToEnd());
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------");

            Console.WriteLine("");

            Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("JSon String result:");
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
            Console.WriteLine(extractedResult.ToJsonString());
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------");

            MyExtractionResultClass t = extractedResult.Get<MyExtractionResultClass>();
            StringBuilder sb = CsvExportHelper.ExportList(t.Result);
            string str = sb.ToString();
            File.WriteAllText("ExtractQuotedText.csv", sb.ToString());

            Console.ReadLine();
        }
    }
}