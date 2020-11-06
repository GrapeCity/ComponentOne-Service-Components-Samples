using C1.AdoNet.OData;
using C1.DataCollection.AdoNet;
using C1.iOS.DataCollection;
using Foundation;
using System;
using UIKit;

namespace ODataViewer
{
    public partial class MainViewController : UITableViewController
    {
        const string NorthwindSampleUrl = @"https://services.odata.org/V4/Northwind/Northwind.svc";

        public MainViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var oDataConnection = new C1ODataConnection($@"Url={NorthwindSampleUrl}");
            var tableViewSource = new InvoicesSource(TableView);
            var dataCollection = new C1AdoNetCursorDataCollection<Invoice>(oDataConnection, "Invoices", new string[] { "ProductName", "Address", "City", "Country" });
            TableView.Source = tableViewSource;
            RefreshControl = new UIRefreshControl();
            tableViewSource.RefreshControl = RefreshControl;
            tableViewSource.ItemsSource = dataCollection;
        }
    }

    public class Invoice
    {
        public string ProductName { get; set; }
        public string CustomerName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
    }


    public class InvoicesSource : C1TableViewSource<Invoice>
    {
        private string CellIdentifier = "Default";

        public InvoicesSource(UITableView tableView) : base(tableView)
        {
        }

        public override UITableViewCell GetItemCell(UITableView tableView, NSIndexPath indexPath, Invoice item)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
            if (cell == null)
                cell = new UITableViewCell(UITableViewCellStyle.Subtitle, CellIdentifier);

            if (item != null)
            {
                cell.TextLabel.Text = $"{indexPath.Row} - {item.ProductName}";
                cell.DetailTextLabel.Text = $"{item.Address} - {item.City} {item.Country}";
            }
            else
            {
                cell.TextLabel.Text = "Loading...";
                cell.DetailTextLabel.Text = "";
            }
            return cell;
        }
    }
}