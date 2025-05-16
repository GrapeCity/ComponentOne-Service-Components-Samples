using System;
using System.Windows.Controls;

namespace C1DataCollection101;

public partial class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void OnSampleSelected(object sender, SelectionChangedEventArgs e)
    {
        var sampleViewModel = e.AddedItems[0] as SampleViewModel;
        frame.NavigationService.Navigate(new Uri(sampleViewModel.Uri, UriKind.Relative));
    }
}
