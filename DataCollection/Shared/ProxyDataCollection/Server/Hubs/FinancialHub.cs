using C1.DataCollection.SignalR.Server;
using Microsoft.AspNetCore.SignalR;
using ProxyDataCollection.Shared;

namespace ProxyDataCollection.Server.Hubs
{
    public class FinancialHub : C1DataCollectionHub<Stock>
    {
        public FinancialHub(IHubContext<FinancialHub> context, StockCollection collection)
            : base(context.Clients, collection)
        {
        }
    }
}
