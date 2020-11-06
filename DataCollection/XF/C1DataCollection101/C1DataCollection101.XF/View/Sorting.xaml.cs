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
    public partial class Sorting : ContentPage
    {
        C1DataCollection<YouTubeVideo> _dataCollection;

        public Sorting()
        {
            InitializeComponent();
            Title = AppResources.SortingTitle;
            this.btnSort.Text = AppResources.Sort;
            var task = UpdateVideos();

        }

        private async Task UpdateVideos()
        {
            try
            {
                message.IsVisible = false;
                list.IsVisible = false;
                activityIndicator.IsRunning = true;
                var videos = new ObservableCollection<YouTubeVideo>((await YouTubeDataCollection.LoadVideosAsync("Xamarin Forms", "relevance", null, 50)).Item2);
                _dataCollection = new C1DataCollection<YouTubeVideo>(videos);
                list.ItemsSource = _dataCollection;
                _dataCollection.SortChanged += OnSortChanged;
                UpdateSortButton();
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

        void OnSortChanged(object sender, EventArgs e)
        {
            UpdateSortButton();
        }

        async void OnSortClicked(object sender, EventArgs e)
        {
            if (_dataCollection != null)
            {
                var direction = GetCurrentSortDirection();
                await _dataCollection.SortAsync(x => x.Title, direction == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending);
            }
        }

        private void UpdateSortButton()
        {
            var direction = GetCurrentSortDirection();
            if (direction == SortDirection.Ascending)
            {
                btnSort.IconImageSource = Device.RuntimePlatform == Device.Android ? new FileImageSource() { File = "ic_sort_descending.png" } : Device.RuntimePlatform == Device.UWP ? new FileImageSource() { File = "Assets/AppBar/appbar.sort.alphabetical.descending.png" } : null;
            }
            else
            {
                btnSort.IconImageSource = Device.RuntimePlatform == Device.Android ? new FileImageSource() { File = "ic_sort_ascending.png" } : Device.RuntimePlatform == Device.UWP ? new FileImageSource() { File = "Assets/AppBar/appbar.sort.alphabetical.ascending.png" } : null;
            }
        }

        private SortDirection GetCurrentSortDirection()
        {
            var sort = _dataCollection.SortDescriptions.FirstOrDefault(sd => sd.SortPath == "Title");
            return sort != null ? sort.Direction : SortDirection.Descending;
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
