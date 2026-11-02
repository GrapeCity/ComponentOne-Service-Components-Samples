using Android.Content.PM;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using C1.DataCollection;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace C1DataCollection101
{
    [Activity(Label = "@string/GroupingTitle", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class GroupingActivity : Activity
    {
        private IDataCollection<object> _dataCollection = null!;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            if (ActionBar != null)
            {
                ActionBar.Title = GetString(Resource.String.GroupingTitle);
                ActionBar.SetDisplayHomeAsUpEnabled(true);
                ActionBar.SetHomeButtonEnabled(true);
            }

            RecyclerView = FindViewById<RecyclerView>(Resource.Id.RecyclerView)!;

            _ = UpdateVideos();
        }

        public RecyclerView RecyclerView { get; set; } = null!;

        [DynamicDependency(DynamicallyAccessedMemberTypes.Interfaces, typeof(ObservableCollection<YouTubeVideo>))]
        [UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "The dynamic dependency ensures the interfaces of the observable collection will not be trimmed.")]
        private async Task UpdateVideos()
        {
            var indicator = new ProgressBar(this);
            try
            {
                indicator.Activated = true;
                var videos = new ObservableCollection<YouTubeVideo>((await YouTubeDataCollection.LoadVideosAsync("Dotnet Android", "relevance", null, 50)).Item2);
                _dataCollection = new C1DataCollection<YouTubeVideo>(videos).AsPlain();
                await _dataCollection.GroupAsync("ChannelTitle");
                RecyclerView.SetLayoutManager(new LinearLayoutManager(this));
                RecyclerView.SetAdapter(new YouTubeAdapter(_dataCollection));
            }
            catch
            {
                var builder = new Android.App.AlertDialog.Builder(this);
                if (Resources != null)
                {
                    builder.SetMessage(Resources.GetString(Resource.String.InternetConnectionError));
                    var alert = builder.Create();
                    alert?.Show();
                }
            }
            finally
            {
                indicator.Activated = false;
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