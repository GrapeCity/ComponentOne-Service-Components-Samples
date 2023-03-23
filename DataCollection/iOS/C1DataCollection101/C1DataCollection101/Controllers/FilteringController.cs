using CoreGraphics;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using UIKit;
using C1.DataCollection;

namespace C1DataCollection101
{
    public partial class FilteringController : UITableViewController, IUISearchResultsUpdating
    {
        private YouTubeTableViewSource _source;

        public FilteringController(IntPtr handle) : base(handle)
        {
        }

        public UISearchController SearchController { get; private set; }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SearchController = new UISearchController(searchResultsController: null) { SearchResultsUpdater = this };
            SearchController.SearchBar.Placeholder = Foundation.NSBundle.MainBundle.GetLocalizedString("FilterPlaceholderText", "");
            SearchController.DimsBackgroundDuringPresentation = false;
            TableView.TableHeaderView = SearchController.SearchBar;
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
                var videos = (await YouTubeDataCollection.LoadVideosAsync("DotNet iOS", "relevance", null, 50)).Item2;
                _source = new YouTubeTableViewSource(TableView) { ItemsSource = videos };
                TableView.Source = _source;
            }
            catch
            {

                var alert = new UIAlertView("", Foundation.NSBundle.MainBundle.GetLocalizedString("InternetConnectionError", ""), null, "OK");
                alert.Show();
            }
            finally
            {
                indicator.StopAnimating();
            }
        }


        public async void UpdateSearchResultsForSearchController(UISearchController searchController)
        {
            await _source.DataCollection.FilterAsync(searchController.SearchBar.Text);
        }
    }
}