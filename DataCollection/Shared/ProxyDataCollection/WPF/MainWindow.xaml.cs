using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using C1.DataCollection;
using C1.DataCollection.SignalR.Client;
using C1.WPF.Grid;
using C1.WPF.Sparkline;
using ProxyDataCollection.Shared;

namespace ProxyDataCollection.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private C1ProxyDataCollection<Stock> _stocks;

        public MainWindow()
        {
            InitializeComponent();

            _ = Load();
        }

        private async Task Load()
        {
            try
            {
                loadingIndicator.Visibility = Visibility.Visible;
                var url = new Uri("https://localhost:7278/financialHub");
                //var url = new Uri("http://10.41.0.137/ProxyDataCollection/financialHub");
                _stocks = new C1ProxyDataCollection<Stock>(url) { PageSize = 50 };
                _stocks.PropertyChanged += OnCollectionPropertyChanged;
                await _stocks.ConnectAsync();
                flexGrid.ItemsSource = _stocks;
            }
            catch (Exception ex)
            {
                disconnectedLayer.Visibility = Visibility.Visible;
            }
            finally
            {
                loadingIndicator.Visibility = Visibility.Collapsed;
            }
        }

        private void OnCollectionPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(C1ProxyDataCollection<Stock>.IsLoading))
            {
                loadingIndicator.Visibility = _stocks.IsLoading ? Visibility.Visible : Visibility.Collapsed;
            }
            if (e.PropertyName == nameof(ISupportConnection.ConnectionState))
            {
                switch (_stocks.ConnectionState)
                {
                    case ConnectionState.Disconnected:
                        disconnectedLayer.Visibility = Visibility.Visible;
                        retryButton.Visibility = Visibility.Visible;
                        retryButton.Content = "Connect";
                        retryButton.IsEnabled = true;
                        break;
                    case ConnectionState.Disconnecting:
                        disconnectedLayer.Visibility = Visibility.Visible;
                        retryButton.Visibility = Visibility.Visible;
                        retryButton.Content = "Disconnecting...";
                        retryButton.IsEnabled = false;
                        break;
                    case ConnectionState.Connected:
                        disconnectedLayer.Visibility = Visibility.Collapsed;
                        break;
                    case ConnectionState.Connecting:
                        disconnectedLayer.Visibility = Visibility.Visible;
                        retryButton.Visibility = Visibility.Visible;
                        retryButton.Content = "Connecting...";
                        retryButton.IsEnabled = false;
                        break;
                    case ConnectionState.Reconnecting:
                        disconnectedLayer.Visibility = Visibility.Visible;
                        retryButton.Visibility = Visibility.Visible;
                        retryButton.Content = "Reconnecting...";
                        retryButton.IsEnabled = false;
                        break;
                    default:
                        break;
                }
            }
        }

        private async void OnRetryClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                await _stocks.ConnectAsync();
            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
        }
    }

    internal class FinancialCellFactory : GridCellFactory
    {
        public override void BindCellContent(GridCellType cellType, GridCellRange range, FrameworkElement cellContent)
        {
            base.BindCellContent(cellType, range, cellContent);
            if (cellType == GridCellType.Cell && cellContent is TextBlock label)
            {
                var changeColumn = Grid.Columns[nameof(Stock.Change)];
                if (changeColumn.Index == range.Column)
                {
                    label.FontWeight = FontWeights.Bold;
                    var value = Grid.GetCellValue(range) as double?;
                    if (value == null)
                        return;
                    if (value > 0)
                        label.Foreground = new SolidColorBrush(Color.FromRgb(0x13, 0x73, 0x33));
                    else if (value < 0)
                        label.Foreground = new SolidColorBrush(Color.FromRgb(0xa5, 0x0e, 0x0e));
                }
            }
        }

        public override void UnbindCellContent(GridCellType cellType, GridCellRange range, FrameworkElement cellContent)
        {
            base.UnbindCellContent(cellType, range, cellContent);
            if (cellContent is TextBlock label)
            {
                //Revert changes in labels because they are reused.
                label.ClearValue(TextBlock.ForegroundProperty);
                label.ClearValue(TextBlock.FontWeightProperty);
            }
        }
    }

    public class GridSparklineColumn : GridColumn
    {
        protected override object GetCellContentType(GridCellType cellType, GridRow row)
        {
            if (cellType == GridCellType.Cell)
            {
                return typeof(C1Sparkline);
            }
            return base.GetCellContentType(cellType, row);
        }

        protected override FrameworkElement CreateCellContent(GridCellType cellType, object cellContentType, GridRow row)
        {
            if (cellContentType is Type type && type == typeof(C1Sparkline))
            {
                return new C1Sparkline() { Margin = new Thickness(5) };
            }
            return base.CreateCellContent(cellType, cellContentType, row);
        }

        protected override void BindCellContent(FrameworkElement cellContent, GridCellType cellType, GridRow row)
        {
            if (cellContent is C1Sparkline sparkline)
            {
                var data = (double[])GetCellValue(cellType, row);
                sparkline.Data = data;
            }
            base.BindCellContent(cellContent, cellType, row);
        }

        protected override void UnbindCellContent(FrameworkElement cellContent, GridCellType cellType, GridRow row)
        {
            base.UnbindCellContent(cellContent, cellType, row);
        }
    }
}
