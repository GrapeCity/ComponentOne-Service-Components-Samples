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
    public partial class Filtering : ContentPage
    {
        C1DataCollection<YouTubeVideo> _dataCollection;

        public Filtering()
        {
            InitializeComponent();
            Title = AppResources.FilteringTitle;
            filter.Placeholder = AppResources.FilterPlaceholderText;
            var task = UpdateVideos();
        }

        private async Task UpdateVideos()
        {
            try
            {
                message.IsVisible = false;
                list.IsVisible = false;
                activityIndicator.IsRunning = true;
                var _videos = new ObservableCollection<YouTubeVideo>((await YouTubeDataCollection.LoadVideosAsync("Xamarin Forms", "relevance", null, 50)).Item2);
                _dataCollection = new C1DataCollection<YouTubeVideo>(_videos);
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
