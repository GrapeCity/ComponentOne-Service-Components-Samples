﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using C1.DataEngine;
using C1.PivotEngine;

namespace WpfPivotApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            _ = LoadAsync();
        }

        private async Task LoadAsync()
        {
            Workspace workspace = await Northwind.Invoice.GetWorkspace();
            Northwind.Invoice.GetPivotEngine(workspace, (pivot) =>
            {
                dataGrid1.ItemsSource = pivot.PivotDefaultView;
            });
        }
    }
}
