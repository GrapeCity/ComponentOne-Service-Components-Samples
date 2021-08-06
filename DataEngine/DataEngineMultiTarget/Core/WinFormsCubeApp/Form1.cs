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
using C1.PivotEngine;

namespace WinFormsCubeApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
            DataGridView grid = new DataGridView();
            Controls.Add(grid);
            grid.Dock = DockStyle.Fill;
			grid.ReadOnly = true;
			grid.AllowUserToAddRows = false;
			grid.AllowUserToDeleteRows = false;
			grid.ColumnAdded += (s, e) => grid.Columns[0].Frozen = true;

			C1PivotEngine pivot = AdventureWorks.Cube.GetPivotEngine();
			grid.DataSource = pivot.PivotDefaultView;
        }
    }
}
