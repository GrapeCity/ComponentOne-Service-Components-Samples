using System;
using System.Windows.Forms;
using C1.DataCollection.BindingList;
using C1DataCollection101.Resources;

namespace C1DataCollection101.View
{
    public partial class VirtualMode : UserControl
    {
        Menu _owner;

        public VirtualMode()
        {
            InitializeComponent();
            lblTitle.Text = AppResources.VirtualModeTitle;
        }

        internal async void ShowPage(Menu owner)
        {
            _owner = owner;
            UpdateVideos();
        }

        private void UpdateVideos()
        {
            try
            {
                lblMessage.Visible = false;
                grid.Visible = false;
                var collection = new VirtualModeDataCollection();
                grid.DataSource = new C1DataCollectionBindingList(collection);
                grid.Visible = true;
            }
            catch
            {
                lblMessage.Text = AppResources.InternetConnectionError;
                lblMessage.Visible = true;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            _owner.SelectedSampleViewType = -1;
        }
    }
}
