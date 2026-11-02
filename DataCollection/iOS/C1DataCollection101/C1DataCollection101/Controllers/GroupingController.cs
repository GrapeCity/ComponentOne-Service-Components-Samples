using C1.DataCollection;
using CoreGraphics;
using System;
using System.Collections.ObjectModel;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using UIKit;

namespace C1DataCollection101
{
    public partial class GroupingController : UITableViewController
    {
        public GroupingController(IntPtr handle) : base(handle)
        {
        }

        [ObsoletedOSPlatform("ios9.0")]
        [ObsoletedOSPlatform("maccatalyst9.0")]
        [SupportedOSPlatform("ios")]
        [SupportedOSPlatform("maccatalyst")]
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var task = UpdateVideos();
        }

        [ObsoletedOSPlatform("ios9.0")]
        [ObsoletedOSPlatform("maccatalyst9.0")]
        [SupportedOSPlatform("ios")]
        [SupportedOSPlatform("maccatalyst")]
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
#pragma warning disable IL2026
                var source = new YouTubeTableViewSource(TableView) { ItemsSource = videos };
#pragma warning restore IL2026
                await source.DataCollection.GroupAsync("ChannelTitle");
                TableView.Source = source;
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

    }
}