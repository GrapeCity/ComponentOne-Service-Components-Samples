using System;
using System.Net;
using System.Threading.Tasks;
using C1.DataEngine;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            Workspace workspace = await Northwind.Invoice.GetWorkspace();
            IDataList results = workspace.GetQueryData("SalesByEmployeeCountry");
            DataList.Write(results, Console.Out);
            Console.ReadKey();
        }
    }
}
