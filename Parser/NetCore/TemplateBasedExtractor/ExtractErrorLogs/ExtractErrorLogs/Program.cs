using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using C1.TextParser;

namespace ExtractErrorLogs
{
    public class TimeHMS
    {
        [DataMember(Name = "hour")]
        public int Hour { get; set; }

        [DataMember(Name = "minute")]
        public int Minute { get; set; }

        [DataMember(Name = "second")]
        public int Second { get; set; }
    }

    public class Time
    {
        [DataMember(Name = "timeHMS")]
        public TimeHMS TimeHMS { get; set; }

        [DataMember(Name = "millisecond")]
        public int MilliSecond { get; set; }
    }

    public class Log
    {
        [DataMember(Name = "description")]
        public String Description { get; set; }

        [DataMember(Name = "time")]
        public Time Time { get; set; }
    }

    public class Logs
    {
        [DataMember(Name = "errorLog")]
        public List<Log> ErrorLogs { get; set; } 
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            FileStream fst = File.Open(@"template.xml", FileMode.Open);
            FileStream fss = File.Open(@"input.txt", FileMode.Open);

            TemplateBasedExtractor templateBasedExtractor = new TemplateBasedExtractor(fst);
            IExtractionResult extractedResult = templateBasedExtractor.Extract(fss);
            fss.Close();
            fst.Close();

            Console.WriteLine("------------------------------------------------------------------------------------------");
            Console.WriteLine("MESCIUS inc. All Rights Reserved.");
            Console.WriteLine("Demo of the C1TextParser library - TemplateBased extractor sample");
            Console.WriteLine("Test case: From a server log file, extract all the ERROR logs");
            Console.WriteLine("Detail: Each log follows a predefined fixed structure, that consists in 4 major elements.");
            Console.WriteLine("        These are: The date, the time (up to ms), the log type and finally, ");
            Console.WriteLine("        the description of the log");
            Console.WriteLine("        The input stream content, the template and also the extracted result");
            Console.WriteLine("        (in Json format) are displayed down below. Also, the extracted result was");
            Console.WriteLine("        exported to \"ExtractErrorLogs.csv\" at the current working directory");
            Console.WriteLine("------------------------------------------------------------------------------------------");

            Console.WriteLine("");

            Console.WriteLine("------------------------------------------------------------------------------------------");
            Console.WriteLine("Input stream:");
            Console.WriteLine("------------------------------------------------------------------------------------------");
            Console.WriteLine(new StreamReader(File.Open(@"input.txt", FileMode.Open)).ReadToEnd());
            Console.WriteLine("------------------------------------------------------------------------------------------");

            Console.WriteLine("");

            Console.WriteLine("------------------------------------------------------------------------------------------");
            Console.WriteLine("Template:");
            Console.WriteLine("------------------------------------------------------------------------------------------");
            Console.WriteLine(new StreamReader(File.Open(@"template.xml", FileMode.Open)).ReadToEnd());
            Console.WriteLine("------------------------------------------------------------------------------------------");

            Console.WriteLine("");

            Console.WriteLine("------------------------------------------------------------------------------------------");
            Console.WriteLine("JSon String result:");
            Console.WriteLine("------------------------------------------------------------------------------------------");
            Console.WriteLine(extractedResult.ToJsonString());
            Console.WriteLine("------------------------------------------------------------------------------------------");

            Logs t = extractedResult.Get<Logs>();
            StringBuilder sb = CsvExportHelper.ExportList(t.ErrorLogs);
            string str = sb.ToString();
            File.WriteAllText("ExtractErrorLogs.csv", sb.ToString());

            Console.ReadLine();
        }
    }
}