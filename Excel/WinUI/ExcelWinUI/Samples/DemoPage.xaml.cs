using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;

using Windows.Foundation;
using Windows.Foundation.Collections;

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using C1.Excel;

using _Color = System.Drawing.Color;
using Windows.Storage;

namespace ExcelWinUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DemoPage : Page
    {
        C1XLBook _book;
        CollectionViewSource _cvs = new CollectionViewSource();

        public DemoPage()
        {
            this.InitializeComponent();

            // create a PDF file to work with
            _book = new C1XLBook();
        }

        // refresh view when collection is modified
        void RefreshView()
        {
        }

        async void _btnSave_Click(object sender, RoutedEventArgs e)
        {
            Debug.Assert(_book != null);

            var picker = new Windows.Storage.Pickers.FileSavePicker();
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            picker.FileTypeChoices.Add(Strings.Typexlsx, new List<string>() { ".xlsx" });
            picker.FileTypeChoices.Add(Strings.Typexlsm, new List<string>() { ".xlsm" });
            picker.FileTypeChoices.Add(Strings.Typexls, new List<string>() { ".xls" });
            picker.FileTypeChoices.Add(Strings.Typecsv, new List<string>() { ".csv" });
            picker.SuggestedFileName = Strings.DefaultFileName;

            var file = await picker.PickSaveFileAsync();
            if (file != null)
            {
                try
                {
                    // step 1: save file
                    var fileFormat = GetFormatByName(file.Path);
                    using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                    using (var s = stream.AsStream())
                    {
                        _book.Save(s, fileFormat);
                        //await stream.FlushAsync();
                    }

                    // step 2: user feedback
                    _tbContent.Text = string.Format(Strings.SaveLocationTip, file.Path);

                    RefreshView();
                }
                catch (Exception x)
                {
                    _tbContent.Text = string.Format(Strings.SaveAndOpenException, x.Message);
                }
            }
        }

        private void _btnHello_Click(object sender, RoutedEventArgs e)
        {
            // step 1: create a new workbook
            _book = new C1XLBook();

            // step 2: get the sheet that was created by default, give it a name
            XLSheet sheet = _book.Sheets[0];
            sheet.Name = Strings.SheetName;

            // step 3: create styles for odd and even values
            XLStyle styleOdd = new XLStyle(_book);
            styleOdd.Font = new XLFont("Tahoma", 9, false, true);
            styleOdd.ForeColor = _Color.FromArgb(255, 0, 0, 255);
            XLStyle styleEven = new XLStyle(_book);
            styleEven.Font = new XLFont("Tahoma", 9, true, false);
            styleEven.ForeColor = _Color.FromArgb(255, 255, 0, 0);

            // step 4: write content and format into some cells
            for (int i = 0; i < 100; i++)
            {
                XLCell cell = sheet[i, 0];
                cell.Value = i + 1;
                cell.Style = ((i + 1) % 2 == 0) ? styleEven : styleOdd;
            }

            // step 5: allow user to save the file
            _tbContent.Text = Strings.DataCreatedTip;
            RefreshView();
        }

        // open an existing zip file
        async void _btnOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var picker = new Windows.Storage.Pickers.FileOpenPicker();
                picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
                picker.FileTypeFilter.Add(".xlsx");
                picker.FileTypeFilter.Add(".xlsm");
                picker.FileTypeFilter.Add(".xls");
                picker.FileTypeFilter.Add(".csv");

                var file = await picker.PickSingleFileAsync();
                if (file != null)
                {
                    // step 1: create a new workbook
                    _book = new C1XLBook();

                    // step 2: load existing file
                    var fileFormat = GetFormatByName(file.Path);
                    using (var stream = await file.OpenAsync(FileAccessMode.Read))
                    using (var s = stream.AsStream())
                    {
                        _book.Load(s, fileFormat);
                    }

                    // step 3: allow user to save the file
                    _tbContent.Text = string.Format(Strings.OpenTip, _book.Sheets[0].Name);
                    RefreshView();
                }
            }
            catch (Exception x)
            {
                _tbContent.Text = string.Format(Strings.SaveAndOpenException, x.Message);
            }
        }

        static FileFormat GetFormatByName(string path)
        {
            switch (Path.GetExtension(path).ToLower())
            {
                case ".csv":
                    return FileFormat.Csv;
                case ".xls":
                    return FileFormat.Biff8;
                case ".xlsm":
                    return FileFormat.OpenXmlMacro;
                case ".xlsx":
                default:
                    return FileFormat.OpenXml;
            }
        }

    }
}

