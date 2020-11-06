using C1.WPF.TextParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // use C1.TextParser services directly
            var sacu = new C1.TextParser.StartsAfterContinuesUntil("\"", "\"");
            Stream inputStream = File.Open(@"SacuInput.txt", FileMode.Open);
            var res = sacu.Extract(inputStream);
            Console.WriteLine(res.ToJsonString());

            var tbe = new C1.TextParser.TemplateBasedExtractor(File.Open(@"template.xml", FileMode.Open));
            var res1 = tbe.Extract(File.Open(@"input.txt", FileMode.Open));
            Console.WriteLine(res1.ToJsonString());
            Console.WriteLine(res1.Extractor);

            var he = C1TextParserWrapper.GetHtmlExtractor(File.Open(@"amazonEmail1.html", FileMode.Open));
            //Fixed placeHolder for the expected delivery date
            he.AddPlaceHolder("delivery date", @"/html/body/div[2]/div/div/div/table/tbody/tr[3]/td/table/tbody/tr[1]/td[1]/p/strong");
            var res2 = he.Extract(File.Open(@"amazonEmail2.html", FileMode.Open));
            txt.Text = res2.ToJsonString();
        }
    }
}
 