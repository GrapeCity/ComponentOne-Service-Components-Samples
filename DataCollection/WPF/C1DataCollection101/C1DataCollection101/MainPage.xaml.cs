using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace C1DataCollection101;

public partial class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();
    }
    
    private bool _updatingList;

    private void OnSampleSelected(object sender, SelectionChangedEventArgs e)
    {
        if (_updatingList) return;
        var sampleViewModel = e.AddedItems[0] as SampleViewModel;
        frame.NavigationService.Navigate(new Uri(sampleViewModel.Uri, UriKind.Relative));
    }

    private void OnPageNavigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
    {
        if (e.Content is Page page)
        {
            var mainViewModel = DataContext as MainViewModel;
            var sample = mainViewModel.Samples.FirstOrDefault(s => e.Uri.ToString().EndsWith(s.Uri));
            _updatingList = true;
            samplesList.SelectedIndex = mainViewModel.Samples.IndexOf(sample);
            _updatingList = false;
            Application.Current.MainWindow.Title = page.Title;
        }
    }
}
