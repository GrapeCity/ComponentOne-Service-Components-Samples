using Android.Content.PM;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using AndroidX.SwipeRefreshLayout.Widget;
using C1.DataCollection;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace C1DataCollection101
{
    [Activity(Label = "@string/OnDemandTitle", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class OnDemandActivity : Activity
    {
        private YouTubeDataCollection _dataCollection = null!;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.OnDemand);

            if (ActionBar != null)
            {
                ActionBar.Title = GetString(Resource.String.OnDemandTitle);
                ActionBar.SetDisplayHomeAsUpEnabled(true);
                ActionBar.SetHomeButtonEnabled(true);
            }

            SwipeRefresh = FindViewById<SwipeRefreshLayout>(Resource.Id.SwipeRefresh)!;
            RecyclerView = FindViewById<RecyclerView>(Resource.Id.RecyclerView)!;
            Search = FindViewById<EditText>(Resource.Id.Search)!;

            SwipeRefresh.Refresh += OnRefresh;
            Search.TextChanged += OnTextChanged;

            Load();
        }

        [DynamicDependency(DynamicallyAccessedMemberTypes.Interfaces, typeof(ObservableCollection<YouTubeVideo>))]
        [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "The dynamic dependency ensures the interfaces of the observable collection will not be trimmed.")]
        private async void Load()
        {
            _dataCollection = new YouTubeDataCollection();
            var grouping = new C1GroupDataCollection<YouTubeVideo>(_dataCollection, true);
            await grouping.GroupAsync("PublishedDay");
            RecyclerView.SetLayoutManager(new LinearLayoutManager(this));
            RecyclerView.SetAdapter(new YouTubeAdapter(grouping));
        }

        public SwipeRefreshLayout SwipeRefresh { get; set; } = null!;
        public RecyclerView RecyclerView { get; set; } = null!;
        public EditText Search { get; set; } = null!;

        private async void OnTextChanged(object? sender, Android.Text.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Search.Text))
                await _dataCollection.SearchAsync(Search.Text);
        }

        private async void OnRefresh(object? sender, System.EventArgs e)
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