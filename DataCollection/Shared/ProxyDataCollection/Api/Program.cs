using C1.DataCollection.Serialization;
using C1.DataCollection.SignalR.Server;
using ProxyDataCollection.Api;
using ProxyDataCollection.Api.Hubs;
using ProxyDataCollection.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR().AddDataCollection(StockSerializerContext.Default);
builder.Services.AddSingleton<StockCollection, StockCollection>();

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:7184")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseCors(myAllowSpecificOrigins);
app.MapHub<FinancialHub>("/financialHub");

app.Run();
