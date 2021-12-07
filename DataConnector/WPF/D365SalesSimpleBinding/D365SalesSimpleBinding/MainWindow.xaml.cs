using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using C1.AdoNet.D365S;
using C1.DataCollection;
using C1.DataCollection.AdoNet;

namespace D365SalesSimpleBinding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string UrlDynamics = ""; //The url to D365Sales site, e.g., https://xxx.dynamics.com/api/data/v9.1/
        const string AccessToken = @""; //The access token
        const string ClientID = ""; //The cliend id, optional
        const string CllentSecret = ""; //The client secret, optional
        const string RefreshToken = ""; //The refresh token, optional, use to automatically acquire a new access token when current access token is expired
        const string TokenEnpoint = ""; //The token endpoint url, e.g., https://login.microsoftonline.com/common/oauth2/token
        const int MaxPageSize = 50;

        public MainWindow()
        {
            InitializeComponent();

            LoadItems();
        }

        public async void LoadItems()
        {
            //if (string.IsNullOrWhiteSpace(UrlDynamics) || string.IsNullOrWhiteSpace(AccessToken) || string.IsNullOrWhiteSpace(TokenEnpoint))
            //{
            //    throw new InvalidOperationException("Please update the configuration constants");
            //}

            string connstr = $@"Url={UrlDynamics};OAuth Access Token={AccessToken};Use Etag=true;OAuth Client Id={ClientID};OAuth Client Secret={CllentSecret};OAuth Refresh Token={RefreshToken};OAuth Token Endpoint={TokenEnpoint};Max Page Size = {MaxPageSize}";
            string[] fields = new string[] { "accountid", "name", "emailaddress1" };
            var d365SConnection = new C1D365SConnection(connstr);
            var dataCollection = new C1AdoNetCursorDataCollection(d365SConnection, "Accounts", fields, MaxPageSize);
            //Force collection to load once
            await dataCollection.RefreshAsync();
            C1.WPF.DataCollection.C1CollectionView cv = new C1.WPF.DataCollection.C1CollectionView(dataCollection);
            grid.ItemsSource = cv;
        }
    }
}
