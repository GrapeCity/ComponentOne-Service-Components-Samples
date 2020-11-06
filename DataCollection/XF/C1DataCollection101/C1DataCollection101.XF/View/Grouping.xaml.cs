using C1DataCollection101.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using C1.DataCollection;
using Xamarin.Essentials;

namespace C1DataCollection101
{
    public partial class Grouping : ContentPage
    {
        C1DataCollection<YouTubeVideo> _dataCollection;
        ObservableCollection<YouTubeVideo> _videos;

        public Grouping()
        {
            InitializeComponent();
            Title = AppResources.GroupingTitle;

            list.IsGroupingEnabled = true;
            list.GroupDisplayBinding = new Binding("Group");
            var task = UpdateVideos();
        }

        private async Task UpdateVideos()
        {
            try
            {
                message.IsVisible = false;
                list.IsVisible = false;
                activityIndicator.IsRunning = true;
                _videos = new ObservableCollection<YouTubeVideo>((await YouTubeDataCollection.LoadVideosAsync("Xamarin Forms", "relevance", null, 50)).Item2);
                _dataCollection = new C1DataCollection<YouTubeVideo>(_videos);
                await _dataCollection.GroupAsync(v => v.ChannelTitle);
                list.ItemsSource = _dataCollection;
                list.IsVisible = true;
            }
            catch
            {
                message.Text = AppResources.InternetConnectionError;
                message.IsVisible = true;
            }
            finally
            {
                activityIndicator.IsRunning = false;
            }
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is YouTubeVideo)
            {
                var video = e.Item as YouTubeVideo;
                _ = Launcher.OpenAsync(new Uri(video.Link));
            }
        }
    }
}
