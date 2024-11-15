﻿using System;
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

            C1PivotEngine pivot = AdventureWorks.Cube.GetPivotEngine();
			dataGridView1.ColumnAdded += (s, e) => dataGridView1.Columns[0].Frozen = true;
            dataGridView1.DataSource = pivot.PivotDefaultView;
        }
    }
}
