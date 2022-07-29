using System.Text.Json;
using C1.DataCollection.Serialization;
using Microsoft.AspNetCore.ResponseCompression;
using ProxyDataCollection.Server;
using ProxyDataCollection.Server.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR().AddJsonProtocol(options =>
{
    options.PayloadSerializerOptions = new JsonSerializerOptions
    {
        Converters =
        {
            new FilterExpressionJsonConverter(),
            new SortDescriptionJsonConverter(),
            new NotifyCollectionChangedEventArgsJsonConverter()
        }
    };
});
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});
builder.Services.AddSingleton<StockCollection, StockCollection>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapHub<FinancialHub>("/financialHub");
app.MapFallbackToFile("index.html");


app.Run();
