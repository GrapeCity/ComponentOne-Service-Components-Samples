using System.Data;
using C1.AdoNet.CSV;
using C1.DataCollection.AdoNet;
using C1.DataCollection.BindingList;

namespace CSVWinFormFlexGridVirtualization
{
    public partial class Form1 : Form
    {
        C1AdoNetCursorDataCollection<Data> dataCollection;

        public Form1()
        {
            InitializeComponent();
        }

        private void c1FlexGrid1_AfterScroll(object sender, C1.Win.FlexGrid.RangeEventArgs e)
        {
            if (e.NewRange.BottomRow == c1FlexGrid1.Rows.Count - 1)
                _ = dataCollection.LoadMoreItemsAsync();
            for (int i = e.NewRange.TopRow; i <= e.NewRange.BottomRow; i++)
            {
                if (i >= 0)
                    c1FlexGrid1[i, 0] = i.ToString();
            }
        }

        private async Task LoadItemsAsync()
        {
            try
            {
                string documentConnectionString = @"Uri='output100k.csv';Trim Values=true;Max Page Size=1000";
                var con = new C1CSVConnection(documentConnectionString);
                dataCollection = new C1AdoNetCursorDataCollection<Data>(con, "output100k"); 
                await dataCollection.LoadMoreItemsAsync();
                c1FlexGrid1.DataSource = new C1DataCollectionBindingList(dataCollection);

            }
            catch(Exception ex)
            {
                string error = ex.Message;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _ = LoadItemsAsync();
        }

        private void c1FlexGrid1_OwnerDrawCell(object sender, C1.Win.FlexGrid.OwnerDrawCellEventArgs e)
        {
            if (e.Row != 0)
                c1FlexGrid1[e.Row, 0] = e.Row.ToString();
        }
    }
}