using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using C1.TextParser;

namespace ExtractUsersInfo
{
    public class User
    {
        [DataMember(Name = "name")]
        public String Name { get; set; }

        [DataMember(Name = "age")]
        public int Age { get; set; }

        [DataMember(Name = "contacts")]
        public Contacts Contacts { get; set; }
    }

    public class Contacts
    {
        [DataMember(Name = "phone number")]
        public List<String> PhoneNumbers { get; set; }

        [DataMember(Name = "email")]
        public List<String> Emails { get; set; }
    }

    public class Users
    {
        [DataMember(Name = "user")]
        public List<User> User { get; set; }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            C1.TextParser.LicenseManager.Key = License.Key;

            FileStream fst = File.Open(@"template.xml", FileMode.Open);
            FileStream fss = File.Open(@"input.txt", FileMode.Open);

            TemplateBasedExtractor templateBasedExtractor = new TemplateBasedExtractor(fst);
            IExtractionResult extractedResult = templateBasedExtractor.Extract(fss);
            fss.Close();
            fst.Close();

            Console.WriteLine("------------------------------------------------------------------------------------------");
            Console.WriteLine("GrapeCity, inc, all rights reserved");
            Console.WriteLine("Demo of the C1TextParser library - TemplateBased extractor sample");
            Console.WriteLine("Test case: From a file containing information about the users of a specific service");
            Console.WriteLine("           extract all the fields related to each user, such as its name, age");
            Console.WriteLine("           residency address, work address and contacts.");
            Console.WriteLine("Detail: A custom format is used to specify the information about an user. The custom data");
            Console.WriteLine("        format described by the xml template is presented below.");
            Console.WriteLine("        Also, the extraction result was exported to \"ExtractUsersInfo.csv\" at the");
            Console.WriteLine("        current working directory");
            Console.WriteLine("------------------------------------------------------------------------------------------");

            Console.WriteLine("");

            Console.WriteLine("------------------------------------------------------------------------------------------");
            Console.WriteLine("Template:");
            Console.WriteLine("------------------------------------------------------------------------------------------");
            Console.WriteLine(new StreamReader(File.Open(@"template.xml", FileMode.Open)).ReadToEnd());
            Console.WriteLine("------------------------------------------------------------------------------------------");

            Console.WriteLine("");

            Console.WriteLine("------------------------------------------------------------------------------------------");
            Console.WriteLine("Input stream:");
            Console.WriteLine("------------------------------------------------------------------------------------------");
            Console.WriteLine(new StreamReader(File.Open(@"input.txt", FileMode.Open)).ReadToEnd());
            Console.WriteLine("------------------------------------------------------------------------------------------");

            Console.WriteLine("");

            Console.WriteLine("------------------------------------------------------------------------------------------");
            Console.WriteLine("JSon String result:");
            Console.WriteLine("------------------------------------------------------------------------------------------");
            Console.WriteLine(extractedResult.ToJsonString());
            Console.WriteLine("------------------------------------------------------------------------------------------");

            Users t = extractedResult.Get<Users>();
            StringBuilder sb = CsvExportHelper.ExportList(t.User);
            string str = sb.ToString();
            File.WriteAllText("ExtractUsersInfo.csv", sb.ToString());

            Console.ReadLine();
        }
    }
}