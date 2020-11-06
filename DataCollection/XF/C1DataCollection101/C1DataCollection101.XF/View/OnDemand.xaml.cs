using C1DataCollection101.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using C1.DataCollection;
using Xamarin.Essentials;

namespace C1DataCollection101
{
    public partial class OnDemand: ContentPage
    {
        YouTubeDataCollection _dataCollection;

        public OnDemand()
        {
            InitializeComponent();
            Title = AppResources.OnDemandTitle;
            search.Placeholder = AppResources.SearchPlaceholderText;
            _dataCollection = new YouTubeDataCollection();
            list.ItemsSource = _dataCollection;

            // start on demand loading
            list.LoadItemsOnDemand(_dataCollection);
        }

        private async void OnTextChanged(object sender, EventArgs e)
        {
            try
            {

                message.IsVisible = false;
                list.IsVisible = false;
                activityIndicator.IsRunning = true;
                await _dataCollection.SearchAsync(search.Text);
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
            var video = e.Item as YouTubeVideo;
            _ = Launcher.OpenAsync(new Uri(video.Link));
        }
    }
}
