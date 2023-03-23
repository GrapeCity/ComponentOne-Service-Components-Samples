using C1.AdoNet.OData;
using C1.DataCollection.AdoNet;
using C1.WinUI.DataCollection;
using Microsoft.UI.Xaml;

namespace ODataViewer
{
    public sealed partial class MainWindow : Window
    {
        const string ODataServerUrl = @"https://services.odata.org/V4/Northwind/Northwind.svc";

        public MainWindow()
        {
            this.InitializeComponent();

            Title = "ODataViewer";

            var oDataConnection = new C1ODataConnection($@"Url={ODataServerUrl};Max Page Size=50");
            var dataCollection = new C1AdoNetCursorDataCollection<Invoice>(oDataConnection, "Invoices");
            listView.ItemsSource = new C1CollectionView(dataCollection);
        }
    }

    public class Invoice
    {
        public string ProductName { get; set; }
        public string CustomerName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
