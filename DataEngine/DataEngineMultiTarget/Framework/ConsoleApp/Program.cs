using System;
using System.Net;
using C1.DataEngine;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            Workspace workspace = Northwind.Invoice.GetWorkspace();
            IDataList results = workspace.GetQueryData("SalesByEmployeeCountry");
            DataList.Write(results, Console.Out);
            Console.ReadKey();
        }
    }
}
