using C1.DataCollection;
using Foundation;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UIKit;

namespace C1DataCollection101
{
    public partial class VirtualModeController : UITableViewController
    {
        public VirtualModeController (IntPtr handle) : base (handle)
        {
        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // instantiate our on demand collection view
            RefreshControl = new UIRefreshControl();
            var myDataCollection = new VirtualModedDataCollection();
            var myCollectionViewSource = new SimpleOnDemandCollectionViewSource(TableView);
            myCollectionViewSource.ItemsSource = myDataCollection;
            myCollectionViewSource.RefreshControl = RefreshControl;
            TableView.Source = myCollectionViewSource;
        }
    }

    public class VirtualModedDataCollection : C1VirtualDataCollection<MyDataItem>
    {

        protected override async Task<Tuple<int, IReadOnlyList<MyDataItem>>> GetPageAsync(int pageIndex, int startingIndex, int count, IReadOnlyList<SortDescription> sortDescriptions = null, FilterExpression filterExpression = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var newItems = new List<MyDataItem>();
            await Task.Run(() =>
            {
                // create new page of items
                for (int i = 0; i < this.PageSize; i++)
                {
                    newItems.Add(new MyDataItem(startingIndex + i));
                }
            });
            return new Tuple<int, IReadOnlyList<MyDataItem>>(2_000_000, newItems);
        }
    }
}