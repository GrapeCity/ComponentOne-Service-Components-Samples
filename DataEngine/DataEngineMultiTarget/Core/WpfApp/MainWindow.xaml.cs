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
using C1.DataEngine;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _ = LoadAsync();
        }

        private async Task LoadAsync()
        {
            Workspace workspace = await Northwind.Invoice.GetWorkspace();
            var results = workspace.GetQueryData("SalesByEmployeeCountry");
            dataGrid1.ItemsSource = ClassFactory.CreateFromDataList(results);
        }
    }
}
