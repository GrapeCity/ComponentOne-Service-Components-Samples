using System.Windows.Controls;

namespace C1DataCollection101;

public partial class OnDemand : Page
{
    private YouTubeDataCollection _dataCollection;

    public OnDemand()
    {
        InitializeComponent();


        _dataCollection = new YouTubeDataCollection() { PageSize = 50 };
        grid.ItemsSource = new C1.WPF.DataCollection.C1CollectionView(_dataCollection);
        _ = _dataCollection.SearchAsync("WPF");
    }

    private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        if (e.VerticalOffset + e.ViewportHeight == e.ExtentHeight && _dataCollection.HasMoreItems)
            _ = _dataCollection.LoadMoreItemsAsync();
    }
}
