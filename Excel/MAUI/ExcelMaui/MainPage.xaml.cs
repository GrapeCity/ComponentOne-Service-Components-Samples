using System.Windows.Input;

using C1.Excel;

namespace ExcelMaui;

/// <summary></summary>
public partial class MainPage : ContentPage
{
    //int count = 0;
    string _url = "https://vk.com/s/v1/doc/KBZICgndhMEGollxzYUxGCAVIIxxVmMRSVnpijziYziBal3pWzg";

    /// <summary></summary>
    public ICommand TapCommand => new Command<string>(async (url) => await Launcher.OpenAsync(string.IsNullOrEmpty(url) ? _url : url));


    /// <summary></summary>
	public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private void OnComplexCsvClicked(object sender, EventArgs e)
    {
        SetPath(ComplexTest(FileFormat.Csv));

        ComplexCsvBtn.Text = "Done CSV";
        ComplexBiffBtn.Text = "To XLS";
        ComplexOpenXmlBtn.Text = "To XLSX";

        SemanticScreenReader.Announce(ComplexCsvBtn.Text);
        SemanticScreenReader.Announce(ComplexBiffBtn.Text);
        SemanticScreenReader.Announce(ComplexOpenXmlBtn.Text);
    }

    private void OnComplexBiffClicked(object sender, EventArgs e)
    {
        SetPath(ComplexTest(FileFormat.Biff8));

        ComplexCsvBtn.Text = "To CSV";
        ComplexBiffBtn.Text = "Done XLS";
        ComplexOpenXmlBtn.Text = "To XLSX";

        SemanticScreenReader.Announce(ComplexCsvBtn.Text);
        SemanticScreenReader.Announce(ComplexBiffBtn.Text);
        SemanticScreenReader.Announce(ComplexOpenXmlBtn.Text);
    }

    private void OnComplexOpenXmlClicked(object sender, EventArgs e)
    {
        SetPath(ComplexTest(FileFormat.OpenXml));

        ComplexCsvBtn.Text = "To CSV";
        ComplexBiffBtn.Text = "To XLS";
        ComplexOpenXmlBtn.Text = "Done XLSX";

        SemanticScreenReader.Announce(ComplexCsvBtn.Text);
        SemanticScreenReader.Announce(ComplexBiffBtn.Text);
        SemanticScreenReader.Announce(ComplexOpenXmlBtn.Text);
    }

    string ComplexTest(FileFormat format)
    {
        using (var book = new C1XLBook())
        {
            // create
            var fn = new Functional(book);

            // create test sheets
            fn.DataTypes();
            fn.Formats();
            fn.Styles();
            fn.Formulas();
            fn.NamedRanges();

            // save file
            var mainDir = FileSystem.Current.AppDataDirectory;
            var path = Path.Combine(mainDir, "test" + GetExtByFormat(format));
            book.Save(path);

            // done
            return path;
        }
    }

    void SetPath(string path)
    {
        var url = new Uri(path, UriKind.Absolute);
        //_url = "file://" + path.Replace('\\', '/');
        _url = url.ToString();

        //FileUrl.Text = "Click here to open in browser";
        FileUrl.Text = path;

        //await DisplayAlert("Saved", "File has been saved to: " + path, "OK");
    }

    static string GetExtByFormat(FileFormat ff)
    {
        switch (ff)
        {
            case FileFormat.OpenXml:
                return ".xlsx";
            case FileFormat.Biff8:
            case FileFormat.OpaqueBiff8:
                return ".xls";
            case FileFormat.Csv:
                return ".csv";
            case FileFormat.OpenXmlMacro:
                return ".xlsm";
        }
        return ".xlsx";  // by default
    }
}

