using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using C1.DataCollection;
using C1.DataCollection.BindingList;
using C1.DataEngine;

namespace DataEngineCollectionSample
{
    public partial class Form1 : Form
    {
        private Workspace _workspace;
        private IDataCollection<object> _dataCollection;

        public Form1()
        {
            InitializeComponent();

            _ = LoadAsync();
        }

        private async Task LoadAsync()
        {
            toolStripStatusLabel1.Text = "Generating data...";
            await Task.Yield();
            _workspace = new Workspace();
            _workspace.Init("workspace");
            if (!DataService.TablesExist(_workspace))
            {
                await DataService.GenerateData(_workspace);
            }
            _dataCollection = await DataService.LoadDataCollection(_workspace);
            dataGridView1.DataSource = new C1DataCollectionBindingList(_dataCollection);
            _dataCollection.CollectionChanged += _dataCollection_CollectionChanged;
            this.FormClosed += (s, e) => ((IDisposable)_dataCollection).Dispose();
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            toolStripStatusLabel1.Text = string.Format("Count: {0}", _dataCollection.Count);
        }

        private void _dataCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateStatus();
        }

        private void searchBox_TextChanged(object sender, System.EventArgs e)
        {
            _dataCollection.FilterAsync(FilterExpression.FromString(searchBox.Text, new string[] { "FirstName" }));
        }
    }
}