using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using AndroidX.SwipeRefreshLayout.Widget;
using C1.Android.DataCollection;
using C1.DataCollection;

namespace C1DataCollection101
{
    [Activity(Label = "@string/SimpleOnDemandTitle", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class SimpleOnDemandActivity : Activity
    {
        private SimpleOnDemandDataCollection _dataCollection;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.SimpleOnDemand);

            ActionBar.Title = GetString(Resource.String.SimpleOnDemandTitle);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            SwipeRefresh = FindViewById<SwipeRefreshLayout>(Resource.Id.SwipeRefresh);
            RecyclerView = FindViewById<RecyclerView>(Resource.Id.RecyclerView);


            _dataCollection = new SimpleOnDemandDataCollection();
            RecyclerView.SetLayoutManager(new LinearLayoutManager(this));
            RecyclerView.SetAdapter(new SimpleOnDemandAdapter(_dataCollection));

            SwipeRefresh.Refresh += OnRefresh;
        }

        public SwipeRefreshLayout SwipeRefresh { get; set; }
        public RecyclerView RecyclerView { get; set; }

        private async void OnRefresh(object sender, System.EventArgs e)
        {
            try
            {
                SwipeRefresh.Refreshing = true;
                await _dataCollection.RefreshAsync();
            }
            finally
            {
                SwipeRefresh.Refreshing = false;
            }
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == global::Android.Resource.Id.Home)
            {
                Finish();
                return true;
            }
            else
            {
                return base.OnOptionsItemSelected(item);
            }
        }
    }

    internal class SimpleOnDemandAdapter : C1RecyclerViewAdapter<MyDataItem>
    {

        public SimpleOnDemandAdapter(IDataCollection<MyDataItem> dataCollection)
            : base(dataCollection)
        {
        }

        protected override RecyclerView.ViewHolder OnCreateItemViewHolder(ViewGroup parent)
        {
            var view = LayoutInflater.From(parent.Context)
                               .Inflate(Resource.Layout.ListItem, null, false);
            return new SimpleOnDemandViewHolder(view);
        }

        protected override void OnBindItemViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var h = holder as SimpleOnDemandViewHolder;
            var item = DataCollection[position];
            h.SetTitle(item?.ItemName ?? "");
            h.SetSubtitle(item?.ItemDateTime.ToLongTimeString() ?? "");
        }
    }

    internal class SimpleOnDemandViewHolder : RecyclerView.ViewHolder
    {
        private TextView _title;
        private TextView _subTitle;

        public SimpleOnDemandViewHolder(View itemView)
            : base(itemView)
        {
            _title = itemView.FindViewById<TextView>(Resource.Id.Title);
            _subTitle = itemView.FindViewById<TextView>(Resource.Id.Subtitle);
            var icon = itemView.FindViewById<ImageView>(Resource.Id.Icon);
            icon.Visibility = ViewStates.Gone;
        }

        internal void SetTitle(string title)
        {
            _title.Text = title;
        }

        internal void SetSubtitle(string subTitle)
        {
            _subTitle.Text = subTitle;
        }
    }


    public class SimpleOnDemandDataCollection : C1CursorDataCollection<MyDataItem>
    {
        public SimpleOnDemandDataCollection()
        {
            PageSize = 20;
        }

        public int PageSize { get; set; }

        protected override async Task<Tuple<string, IReadOnlyList<MyDataItem>>> GetPageAsync(int startingIndex, string pageToken, int? count = null, IReadOnlyList<SortDescription> sortDescriptions = null, FilterExpression filterExpresssion = null, CancellationToken cancellationToken = default(CancellationToken))
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
            return new Tuple<string, IReadOnlyList<MyDataItem>>("token not used", newItems);
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