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
using C1.FlexPivot;

namespace WinFormsPivotApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            Workspace workspace = Northwind.Invoice.GetWorkspace();
            C1FlexPivotEngine pivot = Northwind.Invoice.GetPivotEngine(workspace);
            dataGridView1.ColumnAdded += (s, e) => dataGridView1.Columns[0].Frozen = true;
            dataGridView1.DataSource = pivot.FlexPivotDefaultView;
        }
    }
}
