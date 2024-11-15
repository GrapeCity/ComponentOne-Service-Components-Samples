using System.Windows.Input;

using C1.Excel;
using Microsoft.Maui.Controls.PlatformConfiguration;

namespace ExcelMaui;

/// <summary></summary>
public partial class MainPage : ContentPage
{
    //int count = 0;
    string _url = "about:blank";

    /// <summary></summary>
    //public ICommand TapCommand => new Command<string>(async (url) => await Launcher.OpenAsync(string.IsNullOrEmpty(url) ? _url : url));
    //public ICommand TapCommand => new Command<string>(async (url) => await Launch(url));

#if ANDROID
    public ICommand TapCommand => new Command<string>((url) =>
#else
    public ICommand TapCommand => new Command<string>(async (url) =>
#endif
    {
        url = string.IsNullOrEmpty(url) ? _url : url;
#if ANDROID
        var intent = new Android.Content.Intent(Android.Content.Intent.ActionView);
        intent.SetDataAndType(Android.Net.Uri.Parse(url), "*/*");
        var context = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity;
        context.StartActivity(intent);
#else
        await Launcher.OpenAsync(url);
#endif
    });

    /// <summary></summary>
	public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private async Task<bool> CheckPermissions()
    {
        PermissionStatus status = await Permissions.RequestAsync<Permissions.StorageRead>();
        if (status == PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.StorageWrite>();
            return status == PermissionStatus.Granted;
        }
        return false;
    }
    private async void OnComplexCsvClicked(object sender, EventArgs e)
    {
        if (!await CheckPermissions())
            return;

        SetPath(ComplexTest(FileFormat.Csv));

        ComplexCsvBtn.Text = "Done CSV";
        ComplexBiffBtn.Text = "To XLS";
        ComplexOpenXmlBtn.Text = "To XLSX";

        SemanticScreenReader.Announce(ComplexCsvBtn.Text);
        SemanticScreenReader.Announce(ComplexBiffBtn.Text);
        SemanticScreenReader.Announce(ComplexOpenXmlBtn.Text);
    }
    private async void OnComplexBiffClicked(object sender, EventArgs e)
    {
        if (!await CheckPermissions())
            return;

        SetPath(ComplexTest(FileFormat.Biff8));

        ComplexCsvBtn.Text = "To CSV";
        ComplexBiffBtn.Text = "Done XLS";
        ComplexOpenXmlBtn.Text = "To XLSX";

        SemanticScreenReader.Announce(ComplexCsvBtn.Text);
        SemanticScreenReader.Announce(ComplexBiffBtn.Text);
        SemanticScreenReader.Announce(ComplexOpenXmlBtn.Text);
    }
    private async void OnComplexOpenXmlClicked(object sender, EventArgs e)
    {
        if (!await CheckPermissions())
            return;

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
            //var mainDir = FileSystem.Current.CacheDirectory;
            var mainDir = FileSystem.Current.AppDataDirectory;
#if ANDROID
            mainDir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).Path;
#endif
            var path = Path.Combine(mainDir, "test" + GetExtByFormat(format));
            if (format == FileFormat.Csv)
                book.Sheets[0].SaveCsv(path);
            else
                book.Save(path);

            // done
            return path;
        }
    }

    void SetPath(string path)
    {
        var url = new Uri(path, UriKind.Absolute);
        _url = url.ToString();
#if ANDROID
        _url = _url.Replace("file://", "content://").Replace('\\', '/');
#endif
        FileUrl.Text = path;
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