using Serilog;
using System;
using System.Data;
using System.Windows;
using System.Data.Common;
using System.Windows.Input;
using C1.AdoNet.D365S;
using C1.AdoNet.OData;

namespace ConnectorsSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DbConnection _connectionBase;
        const string DEFAULT_ODATA_CONN_STRING = @"Url=https://services.odata.org/V4/Northwind/Northwind.svc/; Use Cache = True;";
        const string DEFAULT_ODATA_SQL_STRING = @"select * from Categories";

        const string DEFAULT_D365S_CONN_STRING = "Url=; OAuth Client Id=; OAuth Client Secret=; OAuth Token Endpoint=; Use Etag=true; Max Page Size = 100; Use Cache = True;";
        const string DEFAULT_D365S_SQL_STRING = @"select * from Products";

        public MainWindow()
        {
            InitializeComponent();

            cboDataSource.SelectedIndex = 0;

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        private async void btnExecute_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                if (_connectionBase == null || _connectionBase.State != ConnectionState.Open)
                {
                    throw new InvalidOperationException("Please connect to DataSource first");
                }

                var sql = txtSqlString.Text;

                if (string.IsNullOrWhiteSpace(sql))
                {
                    return;
                }

                switch (cboDataSource.SelectedIndex)
                {
                    case 0:
                        C1ODataConnection c1ODataConn = _connectionBase as C1ODataConnection;
                        using (C1ODataDataAdapter a = new C1ODataDataAdapter(c1ODataConn, sql))
                        {
                            DataTable t = new DataTable();
                            a.Fill(t);
                            dataGrid.ItemsSource = t.DefaultView;
                            break;
                        }
                    case 1:
                        C1D365SConnection c1D365Conn = _connectionBase as C1D365SConnection;
                        using (C1D365SDataAdapter a = new C1D365SDataAdapter(c1D365Conn, sql))
                        {
                            DataTable t = new DataTable();
                            a.Fill(t);
                            dataGrid.ItemsSource = t.DefaultView;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBoxResult result = MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            }
        }

        private async void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                if (_connectionBase != null && _connectionBase.State != ConnectionState.Closed)
                {
                    _connectionBase.Close();
                }
                var selectedIndex = cboDataSource.SelectedIndex;

                switch (selectedIndex)
                {
                    case 0:
                        C1ODataConnection oDataConn = new C1ODataConnection(txtConnString.Text);
                        await oDataConn.OpenAsync();
                        _connectionBase = oDataConn;
                        break;
                    case 1:
                        C1D365SConnection d365SalesConn = new C1D365SConnection(txtConnString.Text);
                        await d365SalesConn.OpenAsync();
                        _connectionBase = d365SalesConn; //Because C1D365SConnection wraps over C1ODataConnection
                        break;
                }

                if (selectedIndex != -1)
                {
                    //Populating TreeView
                    var schemas = _connectionBase.GetSchema().DefaultView;
                    treeView.ItemsSource = schemas;
                }
            }
            catch (Exception ex)
            {
                MessageBoxResult result = MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            }
        }

        private void cboDataSource_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            switch (cboDataSource.SelectedIndex)
            {
                case 0:
                    txtConnString.Text = DEFAULT_ODATA_CONN_STRING;
                    txtSqlString.Text = DEFAULT_ODATA_SQL_STRING;
                    break;
                case 1:
                    txtConnString.Text = DEFAULT_D365S_CONN_STRING;
                    txtSqlString.Text = DEFAULT_D365S_SQL_STRING;
                    break;
            }
        }
    }
}
