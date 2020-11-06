using CoreGraphics;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using UIKit;
using C1.DataCollection;

namespace C1DataCollection101
{
    public partial class SortingController : UITableViewController
    {
        private YouTubeTableViewSource _source;

        public SortingController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var task = UpdateVideos();
        }

        private async Task UpdateVideos()
        {
            var indicator = new UIActivityIndicatorView(new CGRect(0, 0, 40, 40));
            indicator.ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.Gray;
            indicator.Center = View.Center;
            View.AddSubview(indicator);
            try
            {
                indicator.StartAnimating();
                var videos = (await YouTubeDataCollection.LoadVideosAsync("Xamarin iOS", "relevance", null, 50)).Item2;
                _source = new YouTubeTableViewSource(TableView) { ItemsSource = videos };
                TableView.Source = _source;
            }
            catch
            {
                var alert = new UIAlertView("", Foundation.NSBundle.MainBundle.LocalizedString("InternetConnectionError", ""), null, "OK");
                alert.Show();
            }
            finally
            {
                indicator.StopAnimating();
            }
        }

        async partial void SortButton_Activated(UIBarButtonItem sender)
        {
            if (_source != null)
            {
                var direction = GetCurrentSortDirection();
                await _source.DataCollection.SortAsync("Title", direction == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending);
            }
        }

        private SortDirection GetCurrentSortDirection()
        {
            var sort = _source.DataCollection.GetSortDescriptions().FirstOrDefault(sd => sd.SortPath == "Title");
            return sort != null ? sort.Direction : SortDirection.Descending;
        }
    }
}