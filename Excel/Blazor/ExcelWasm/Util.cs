using Microsoft.JSInterop;

namespace ExcelWasm;

/// <summary>The file utilizes.</summary>
public static class FileUtils
{
    /// <summary>Save as file.</summary>
    /// <param name="js"></param>
    /// <param name="filename"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static ValueTask<object> SaveAs(this IJSRuntime js, string filename, byte[] data)
        => js.InvokeAsync<object>(
            "saveAsFile",
            filename,
            Convert.ToBase64String(data));
}
