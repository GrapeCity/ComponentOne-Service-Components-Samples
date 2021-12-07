using C1.TextParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ECommerceOrder
{
    public class OrderedArticle
    {
        [DataMember(Name = "article name")]
        public String ArticleName { get; set; }

        [DataMember(Name = "article seller")]
        public String ArticleSeller { get; set; }

        [DataMember(Name = "article price")]
        public String ArticlePrice { get; set; }
    }

    public class AmazonTemplateFixedPlaceHolders
    {
        [DataMember(Name = "delivery date")]
        public String DeliveryDate { get; set; }

        [DataMember(Name = "customer name")]
        public String CustomerName { get; set; }

        [DataMember(Name = "total order amount")]
        public String TotalOrderAmount { get; set; }
    }

    public class AmazonTemplateRepeatedBlocks
    {
        [DataMember(Name = "ordered articles")]
        public List<OrderedArticle> OrderedItems { get; set; }
    }

    public class Program
    {
        public static void Main()
        {
            /**************************************************Amazon template*********************************************/
            Stream amazonTemplateStream = File.Open(@"amazonEmail1.html", FileMode.Open);
            HtmlExtractor amazonTemplate = new HtmlExtractor(amazonTemplateStream);

            //Repeated block for each article in the order
            String articleNameXPath = @"//*[@id=""shipmentDetails""]/table/tbody/tr[1]/td[2]/p/a";
            amazonTemplate.AddPlaceHolder("ordered articles", "article name", articleNameXPath);
            String articlePriceXPath = @"//*[@id=""shipmentDetails""]/table/tbody/tr[1]/td[3]/strong";
            amazonTemplate.AddPlaceHolder("ordered articles", "article price", articlePriceXPath);
            String articleSellerXPath = @"//*[@id=""shipmentDetails""]/table/tbody/tr[1]/td[2]/p/span";
            amazonTemplate.AddPlaceHolder("ordered articles", "article seller", articleSellerXPath, 8, 18);

            //Fixed placeHolder for the expected delivery date
            String deliveryDateXPath = @"/html/body/div[2]/div/div/div/table/tbody/tr[3]/td/table/tbody/tr[1]/td[1]/p/strong";
            amazonTemplate.AddPlaceHolder("delivery date", deliveryDateXPath);

            //Fixed placeHolder for the total amount of the order
            String totalAmountXPath = @"//*[@id=""shipmentDetails""]/table/tbody/tr[8]/td[2]/strong";
            amazonTemplate.AddPlaceHolder("total order amount", totalAmountXPath);

            //Fixed placeHolder for the customer name
            String customerNameXPath = @"/html/body/div[2]/div/div/div/table/tbody/tr[2]/td/p[1]";
            amazonTemplate.AddPlaceHolder("customer name", customerNameXPath, 6, 15);
            /***************************************************************************************************************/

            Stream source = File.Open(@"amazonEmail2.html", FileMode.Open);
            IExtractionResult extractedResult = amazonTemplate.Extract(source);

            Console.WriteLine("------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("GrapeCity, inc, all rights reserved");
            Console.WriteLine("Demo of the C1TextParser library - Html extractor sample");
            Console.WriteLine("Test case: From amazon order emails extract relevant information about the order itself.");
            Console.WriteLine("           This sample pretends to demonstrate the repeated place holder extraction capabilities of");
            Console.WriteLine("           C1TextParser - Html extractor");
            Console.WriteLine("Detail: The sample consists on three fixed place holders and one repeated block. The fixed place holders are");
            Console.WriteLine("        the customer name, the order delivery date and also the total amount of the order. The repeated ");
            Console.WriteLine("        block is used to extract each article that appear in the ordered article list. It contains three");
            Console.WriteLine("        repeated place holders. These are: the name, the price and the seller of the article.");
            Console.WriteLine("        The amazon email used as the extraction source is \"amazonEmail2.html\" and can be consulted in the");
            Console.WriteLine("        current working directory. Also, \"ECommerceOrder.csv\" contains the parsing result");
            Console.WriteLine("------------------------------------------------------------------------------------------------------------");

            Console.WriteLine("------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("JSon String result:");
            Console.WriteLine("------------------------------------------------------------------------------------------------------------");
            Console.WriteLine(extractedResult.ToJsonString());
            Console.WriteLine("------------------------------------------------------------------------------------------------------------");

            AmazonTemplateFixedPlaceHolders amazonTemplateFixedPlaceHolders = extractedResult.Get<AmazonTemplateFixedPlaceHolders>();
            StringBuilder sb1 = CsvExportHelper.ExportList(new List<AmazonTemplateFixedPlaceHolders>() { amazonTemplateFixedPlaceHolders });
            var amazonTemplateOrderedItems = extractedResult.Get<AmazonTemplateRepeatedBlocks>().OrderedItems;
            StringBuilder sb2 = CsvExportHelper.ExportList(amazonTemplateOrderedItems);
            var sb3 = sb1 + "\n" + sb2;
            File.WriteAllText("ECommerceOrder.csv", sb3);

            Console.ReadLine();
        }
    }
}