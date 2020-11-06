using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using C1.DataCollection;

namespace C1DataCollection101
{
    public static class ListViewEx
    {
        public static void LoadItemsOnDemand<T>(this ListView listview, C1CursorDataCollection<T> dataCollection) where T : class
        {
            listview.ItemAppearing += (s, e) =>
            {
                var index = dataCollection.IndexOf((T)e.Item);
                if (index == dataCollection.Count - 1)
                {
                    if (dataCollection.HasMoreItems)
                    {
                        _ = dataCollection.LoadMoreItemsAsync();
                    }
                }
            };
            listview.Refreshing += async (s, e) =>
            {
                listview.IsRefreshing = true;
                await dataCollection.RefreshAsync();
                listview.IsRefreshing = false;
            };
            listview.IsPullToRefreshEnabled = true;
            if (dataCollection.HasMoreItems)
            {
                _ = dataCollection.LoadMoreItemsAsync();
            }
        }
    }
}
