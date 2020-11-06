using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C1.DataCollection;
using Xamarin.Forms;
using C1DataCollection101.Resources;
using System.Threading;

namespace C1DataCollection101
{
    public partial class SimpleOnDemand : ContentPage
    {
        public SimpleOnDemand()
        {
            InitializeComponent();

            Title = AppResources.SimpleOnDemandTitle;

            // instantiate our on demand collection view
            var myDataCollection = new SimpleOnDemandDataCollection();
            list.ItemsSource = myDataCollection;

            // start on demand loading
            list.LoadItemsOnDemand(myDataCollection);

        }
    }

    public class SimpleOnDemandDataCollection : C1CursorDataCollection<MyDataItem>
    {
        public SimpleOnDemandDataCollection()
        {
            PageSize = 30;
        }

        public int PageSize { get; set; }

        protected override async Task<Tuple<string, IReadOnlyList<MyDataItem>>> GetPageAsync(int startingIndex, string pageToken, int? count = null, IReadOnlyList<SortDescription> sortDescriptions = null, FilterExpression filterExpresssion = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Task.Run(() =>
            {
                // create new page of items
                var newItems = new List<MyDataItem>();
                for (int i = 0; i < this.PageSize; i++)
                {
                    newItems.Add(new MyDataItem(startingIndex + i));
                }

                return new Tuple<string, IReadOnlyList<MyDataItem>>("token not used", newItems);
            });
        }
    }
    public class MyDataItem
    {
        public MyDataItem(int index)
        {
            this.ItemName = "My Data Item #" + index.ToString();
            this.ItemDateTime = DateTime.Now;
        }
        public string ItemName { get; set; }
        public DateTime ItemDateTime { get; set; }

    }
}
