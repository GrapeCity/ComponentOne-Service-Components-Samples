using System.ComponentModel;
using C1.DataCollection;
using C1.DataCollection.BindingList;
using C1.DataCollection.SignalR.Client;
using ProxyDataCollection.Shared;

namespace ProxyDataCollection.Win
{
    public partial class Form1 : Form
    {
        private C1ProxyDataCollection<Stock> _stocks;

        public Form1()
        {
            InitializeComponent();

            Load();
        }

        private async Task Load()
        {
            try
            {
                //loadingIndicator.Visibility = Visibility.Visible;
                var url = new Uri("https://localhost:7278/financialHub");
                //var url = new Uri("http://10.41.0.137/ProxyDataCollection/financialHub");
                _stocks = new C1ProxyDataCollection<Stock>(url) { PageSize = 50 };
                _stocks.PropertyChanged += OnCollectionPropertyChanged;
                await _stocks.ConnectAsync();
                c1FlexGrid1.AutoGenerateColumns = false;
                c1FlexGrid1.DataSource = new C1DataCollectionBindingList(_stocks);

            }
            catch (Exception ex)
            {
                //disconnectedLayer.Visibility = Visibility.Visible;
            }
            finally
            {
                //loadingIndicator.Visibility = Visibility.Collapsed;
            }
        }

        private void OnCollectionPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            //if (e.PropertyName == nameof(C1ProxyDataCollection<Stock>.IsLoading))
            //{
            //    loadingIndicator.Visibility = _stocks.IsLoading ? Visibility.Visible : Visibility.Collapsed;
            //}
            //if (e.PropertyName == nameof(ISupportConnection.ConnectionState))
            //{
            //    switch (_stocks.ConnectionState)
            //    {
            //        case ConnectionState.Disconnected:
            //            disconnectedLayer.Visibility = Visibility.Visible;
            //            retryButton.Visibility = Visibility.Visible;
            //            retryButton.Content = "Connect";
            //            retryButton.IsEnabled = true;
            //            break;
            //        case ConnectionState.Disconnecting:
            //            disconnectedLayer.Visibility = Visibility.Visible;
            //            retryButton.Visibility = Visibility.Visible;
            //            retryButton.Content = "Disconnecting...";
            //            retryButton.IsEnabled = false;
            //            break;
            //        case ConnectionState.Connected:
            //            disconnectedLayer.Visibility = Visibility.Collapsed;
            //            break;
            //        case ConnectionState.Connecting:
            //            disconnectedLayer.Visibility = Visibility.Visible;
            //            retryButton.Visibility = Visibility.Visible;
            //            retryButton.Content = "Connecting...";
            //            retryButton.IsEnabled = false;
            //            break;
            //        case ConnectionState.Reconnecting:
            //            disconnectedLayer.Visibility = Visibility.Visible;
            //            retryButton.Visibility = Visibility.Visible;
            //            retryButton.Content = "Reconnecting...";
            //            retryButton.IsEnabled = false;
            //            break;
            //        default:
            //            break;
            //    }
            //}
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            _ = _stocks.FilterAsync(textBox1.Text, matchNumbers: false);
        }
    }
}