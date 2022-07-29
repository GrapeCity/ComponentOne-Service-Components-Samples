using System;
using System.Windows.Forms;
using C1.DataCollection.BindingList;
using C1DataCollection101.Resources;

namespace C1DataCollection101.View
{
    public partial class Editing : UserControl
    {
        Menu _owner;

        public Editing()
        {
            InitializeComponent();

            lblTitle.Text = AppResources.EditingTitle;
        }

        internal async void ShowPage(Menu owner)
        {
            _owner = owner;
            var collection = Customer.GetCustomerList(10);
            var bindingList = new C1DataCollectionBindingList(collection);
            dataGridView1.DataSource = bindingList;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            _owner.SelectedSampleViewType = -1;

        }
    }
}
