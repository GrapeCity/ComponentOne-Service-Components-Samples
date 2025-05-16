using C1.DataCollection;
using C1.WinUI.DataCollection;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml;

namespace DataCollectionExplorer
{
    public sealed partial class MainWindow : Window
    {
        private C1DataCollection<Customer> _dataCollection;

        public MainWindow()
        {
            this.InitializeComponent();

            var collection = Customer.GetCustomerList(100);
            _dataCollection = new C1DataCollection<Customer>(collection);
            grid.ItemsSource = new C1CollectionView(_dataCollection);
        }

        private void OnSorting(object sender, DataGridColumnEventArgs e)
        {
            if (e.Column is DataGridBoundColumn boundColumn)
            {
                SortDirection? sortDirection = !boundColumn.SortDirection.HasValue ? SortDirection.Ascending : boundColumn.SortDirection == DataGridSortDirection.Ascending ? SortDirection.Descending : null;
                if (sortDirection.HasValue)
                    _ = _dataCollection.SortAsync(boundColumn.Binding.Path.Path, sortDirection.Value);
                else
                    _ = _dataCollection.SortAsync();
                boundColumn.SortDirection = sortDirection == SortDirection.Ascending ? DataGridSortDirection.Ascending : sortDirection == SortDirection.Descending ? DataGridSortDirection.Descending : null;

                // Remove sorting indicators from other columns
                foreach (var dgColumn in grid.Columns)
                {
                    if (dgColumn != e.Column)
                    {
                        dgColumn.SortDirection = null;
                    }
                }
            }
        }
    }
}
