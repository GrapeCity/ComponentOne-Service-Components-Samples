using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using C1.DataEngine;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            _ = LoadAsync();
        }

        private async Task LoadAsync()
        {
            DataGridView grid = new DataGridView();
            Controls.Add(grid);
            grid.Dock = DockStyle.Fill;

            Workspace workspace = await Northwind.Invoice.GetWorkspace();
            IDataList results = workspace.GetQueryData("SalesByEmployeeCountry");
            grid.DataSource = results;
        }
    }
}
