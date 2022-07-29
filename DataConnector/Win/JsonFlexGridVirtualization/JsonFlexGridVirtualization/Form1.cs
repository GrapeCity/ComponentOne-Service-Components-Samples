using C1.AdoNet.Json;
using C1.DataCollection.AdoNet;
using C1.DataCollection.BindingList;

namespace ParserTest
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
                if(i >= 0)
                    c1FlexGrid1[i, 0] = i.ToString();
            }

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string documentConnectionString = $@"Data Model=Document;Uri='output10k.json';Json Path='$.items';Max Page Size=1000";
            var con = new C1JsonConnection(documentConnectionString);
            dataCollection = new C1AdoNetCursorDataCollection<Data>(con, "items");
            await dataCollection.LoadMoreItemsAsync();
            c1FlexGrid1.DataSource = new C1DataCollectionBindingList(dataCollection);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            string documentConnectionString = $@"Data Model=Document;Uri='output100k.json';Json Path='$.items';Max Page Size=1000";
            var con = new C1JsonConnection(documentConnectionString);
            dataCollection = new C1AdoNetCursorDataCollection<Data>(con, "items");
            await dataCollection.LoadMoreItemsAsync();
            c1FlexGrid1.DataSource = new C1DataCollectionBindingList(dataCollection);
        }

        private void c1FlexGrid1_OwnerDrawCell(object sender, C1.Win.FlexGrid.OwnerDrawCellEventArgs e)
        {
            if(e.Row != 0)
                c1FlexGrid1[e.Row, 0] = e.Row.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.button1_Click(sender, e);
        }
    }
}