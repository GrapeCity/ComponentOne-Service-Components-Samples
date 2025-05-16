using C1.DataCollection;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace C1DataCollection101;

public partial class Sorting : Page
{
    private C1DataCollection<YouTubeVideo> _dataCollection;

    public Sorting()
    {
        InitializeComponent();

        _ = LoadVideos();
    }

    private async Task LoadVideos()
    {
        var videos = await YouTubeDataCollection.LoadVideosAsync("WPF", "relevance", null, 50);
        _dataCollection = new C1DataCollection<YouTubeVideo>(videos.Item2);
        var cv = new C1.WPF.DataCollection.C1CollectionView(_dataCollection);
        grid.ItemsSource = cv;
    }


    private void OnAddingNewItem(object sender, System.Windows.Controls.AddingNewItemEventArgs e)
    {

    }

    private void OnSortClicked(object sender, System.Windows.RoutedEventArgs e)
    {
        var sortDirection = _dataCollection.GetSortDescriptions().Where(sd => sd.SortPath == nameof(YouTubeVideo.Title)).Select(sd => sd.Direction).FirstOrDefault();
        sortDirection = sortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
        _ = _dataCollection.SortAsync(nameof(YouTubeVideo.Title), sortDirection);
    }
}
