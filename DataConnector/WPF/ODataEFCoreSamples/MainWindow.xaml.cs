using EFCoreSamples.ViewModel;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EFCoreSamples
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            Log.Logger = new LoggerConfiguration()
                                    .WriteTo.File("EFCoreOData.log",
                                        outputTemplate: "{Timestamp:HH:mm:ss} [{Level}] [{SourceContext}] {Message}{NewLine}{Exception}")
                                    .MinimumLevel.Debug()
                                    .CreateLogger();

            this.DataContext = _viewModel = new MainWindowViewModel();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.SetSelectedCountry(cboCountry.SelectedItem.ToString());
            dataGrid1.ItemsSource = _viewModel.InvoiceInfos;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SetSearchText(txtSearch.Text, cboCountry.SelectedItem.ToString());
            dataGrid1.ItemsSource = _viewModel.InvoiceInfos;
        }
    }
}

