using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using C1.DataEngine;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            _ = LoadAsync();
        }

        private async Task LoadAsync()
        {
            Workspace workspace = await Northwind.Invoice.GetWorkspace();
            IDataList results = workspace.GetQueryData("SalesByEmployeeCountry");
            dataGridView1.DataSource = results;
        }
    }
}
