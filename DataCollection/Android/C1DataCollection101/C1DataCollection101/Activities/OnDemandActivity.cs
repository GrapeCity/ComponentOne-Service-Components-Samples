using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using C1.DataCollection;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace C1DataCollection101
{
    [Activity(Label = "@string/OnDemandTitle", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class OnDemandActivity : AppCompatActivity
    {
        private YouTubeDataCollection _dataCollection;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.OnDemand);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = GetString(Resource.String.OnDemandTitle);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            SwipeRefresh = FindViewById<SwipeRefreshLayout>(Resource.Id.SwipeRefresh);
            RecyclerView = FindViewById<RecyclerView>(Resource.Id.RecyclerView);
            Search = FindViewById<EditText>(Resource.Id.Search);
            SwipeRefresh.Refresh += OnRefresh;
            Search.TextChanged += OnTextChanged;

            Load();
        }

        private async void Load()
        {
            _dataCollection = new YouTubeDataCollection();
            var grouping = new C1GroupDataCollection<YouTubeVideo>(_dataCollection, true);
            await grouping.GroupAsync("PublishedDay");
            RecyclerView.SetLayoutManager(new LinearLayoutManager(this));
            RecyclerView.SetAdapter(new YouTubeAdapter(grouping));
        }

        public SwipeRefreshLayout SwipeRefresh { get; set; }
        public RecyclerView RecyclerView { get; set; }
        public EditText Search { get; set; }

        private async void OnTextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            await _dataCollection.SearchAsync(Search.Text);
        }

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
}