using C1.TextParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FlightETicket
{
    public class FlightTicket
    {
        [DataMember(Name = "passenger name")]
        public String PassengerName { get; set; }

        [DataMember(Name = "booking number")]
        public String BookingNumber { get; set; }

        [DataMember(Name = "booking status")]
        public String BookingStatus { get; set; }

        [DataMember(Name = "fare type")]
        public String FareType { get; set; }

        [DataMember(Name = "total amount")]
        public String TotalAmount { get; set; }

        [DataMember(Name = "city of departure")]
        public String CityOfDeparture { get; set; }

        [DataMember(Name = "year of booking")]
        public String YearOfBooking { get; set; }
    }

    public class Program
    {
        public static void Main()
        {
            C1.TextParser.LicenseManager.Key = License.Key;

            /***********************************************Vietjetair template********************************************/
            Stream vietjetairTemplateStream = File.Open(@"vietjetairEmail1.html", FileMode.Open);
            HtmlExtractor vietjetairTemplate = new HtmlExtractor(vietjetairTemplateStream);

            //Fixed placeHolder for the passenger name
            String passengerNameXPath = @"/html/body/div/div[4]/div[1]/div[2]/div[2]/table[2]/tbody/tr[3]/td";
            vietjetairTemplate.AddPlaceHolder("passenger name", passengerNameXPath);

            //Fixed placeHolder for the booking number
            String bookingNumberXPath = @"/html/body/div/div[4]/div[1]/div[2]/div[2]/table[1]/tbody/tr/td[2]/span";
            vietjetairTemplate.AddPlaceHolder("booking number", bookingNumberXPath);

            //Fixed placeHolder for the booking status
            String bookingStatusXPath = @"/html/body/div/div[4]/div[1]/div[2]/div[2]/table[2]/tbody/tr[1]/td[1]";
            vietjetairTemplate.AddPlaceHolder("booking status", bookingStatusXPath);

            //Fixed placeHolder for the fare type
            String fareTypeXPath = @"/html/body/div/div[4]/div[1]/div[2]/div[2]/table[4]/tbody/tr/td[3]";
            vietjetairTemplate.AddPlaceHolder("fare type", fareTypeXPath);

            //Fixed placeHolder for total amount
            String totalAmountXPath = @"/html/body/div/div[4]/div[1]/div[2]/div[2]/table[6]/tbody/tr[2]/td/table[2]/tbody/tr[2]/td[3]";
            vietjetairTemplate.AddPlaceHolder("total amount", totalAmountXPath);

            //Fixed placeHolder for city of departure
            String cityOfDepartureXPath = @"/html/body/div/div[4]/div[1]/div[2]/div[2]/table[4]/tbody/tr/td[4]/text()";
            vietjetairTemplate.AddPlaceHolder("city of departure", cityOfDepartureXPath, 8, 12);

            //Fixed placeHolder for year of booking date
            String yearOfBookingXPath = @"/html/body/div/div[4]/div[1]/div[2]/div[2]/table[2]/tbody/tr[2]/td[1]";
            vietjetairTemplate.AddPlaceHolder("year of booking", yearOfBookingXPath, 6, 4);
            /***************************************************************************************************************/

            Stream source = File.Open(@"vietjetairEmail2.html", FileMode.Open);
            IExtractionResult extractedResult = vietjetairTemplate.Extract(source);

            Console.WriteLine("------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("GrapeCity, inc, all rights reserved");
            Console.WriteLine("Demo of the C1TextParser library - Html extractor sample");
            Console.WriteLine("Test case: Test case: From a vietjetair e-ticket extract relevant information about the flight. Note that the");
            Console.WriteLine("           email used as extraction source was modified on purpose (added random text at different locations)");
            Console.WriteLine("           with the intent to show that html extractor is flexible enough to retrieve the intended text.");
            Console.WriteLine("Detail: This consists on seven fixed place holders. These are: the passenger name, the booking number, the");
            Console.WriteLine("        booking status, the fare type, the total amount, the city of departure and, finally, the year of booking");
            Console.WriteLine("        The vietjetair email used as the extraction source is \"vietjetairEmail2.html\" and can be consulted");
            Console.WriteLine("        in the current working directory. Also, \"FlightETicket.csv\" contains the parsing result");
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------");

            Console.WriteLine("------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("JSon String result:");
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine(extractedResult.ToJsonString());
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------");

            FlightTicket vietjetairResult = extractedResult.Get<FlightTicket>();
            StringBuilder sb = CsvExportHelper.ExportList(new List<FlightTicket>() { vietjetairResult });
            File.WriteAllText("FlightETicket.csv", sb.ToString());

            Console.ReadLine();
        }
    }
}