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
using C1.AdoNet.CSV;
using C1.DataCollection.AdoNet;
using C1.DataCollection.BindingList;

namespace CSVFlexGridVirtualization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string localConnectionString = @"Uri='output100k.csv';Trim Values=true;Max Page Size=1000";

        public MainWindow()
        {
            InitializeComponent();
            LoadItems();
        }

        public void LoadItems()
        {

            var csvConnection = new C1CSVConnection(localConnectionString);
            var dataCollection = new C1AdoNetCursorDataCollection<Data>(csvConnection, "output100k");
            flexGrid.ItemsSource = dataCollection;
        }
    }
}
