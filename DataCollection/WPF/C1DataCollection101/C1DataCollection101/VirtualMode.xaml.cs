using System.Windows.Controls;

namespace C1DataCollection101;

public partial class VirtualMode : Page
{
    private VirtualModeDataCollection _dataCollection;

    public VirtualMode()
    {
        InitializeComponent();

        _dataCollection = new VirtualModeDataCollection() { Mode = C1.DataCollection.VirtualDataCollectionMode.Manual };
        grid.ItemsSource = new C1.WPF.DataCollection.C1CollectionView(_dataCollection);
    }

    private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        _ = _dataCollection.LoadAsync((int)e.VerticalOffset, (int)(e.VerticalOffset + e.ViewportHeight));
    }
}
