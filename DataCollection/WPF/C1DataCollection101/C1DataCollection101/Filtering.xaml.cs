using C1.DataCollection;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace C1DataCollection101;

public partial class Filtering : Page
{
    private C1DataCollection<YouTubeVideo> _dataCollection;

    public Filtering()
    {
        InitializeComponent();
        _ = LoadVideos();
    }

    private async Task LoadVideos()
    {
        var videos = await YouTubeDataCollection.LoadVideosAsync("WPF", "relevance", null, 50);
        _dataCollection = new C1DataCollection<YouTubeVideo>(videos.Item2);
        grid.ItemsSource = new C1.WPF.DataCollection.C1CollectionView(_dataCollection);
    }

    private void OnFilterTextChanged(object sender, TextChangedEventArgs e)
    {
        _ = _dataCollection.FilterAsync((sender as TextBox).Text);
    }
}
