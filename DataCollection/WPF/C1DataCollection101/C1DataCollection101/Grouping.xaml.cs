using C1.DataCollection;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace C1DataCollection101;

public partial class Grouping : Page
{
    private C1DataCollection<YouTubeVideo> _dataCollection;

    public Grouping()
    {
        InitializeComponent();

        _ = LoadVideos();
    }

    private async Task LoadVideos()
    {
        var videos = await YouTubeDataCollection.LoadVideosAsync("WPF", "relevance", null, 50);
        _dataCollection = new C1DataCollection<YouTubeVideo>(videos.Item2);
        await _dataCollection.GroupAsync(nameof(YouTubeVideo.TitleIndex));
        grid.ItemsSource = new C1.WPF.DataCollection.C1CollectionView(_dataCollection);
    }
}
