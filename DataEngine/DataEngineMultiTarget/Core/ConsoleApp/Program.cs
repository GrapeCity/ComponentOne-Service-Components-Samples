using System;
using C1.DataEngine;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Workspace workspace = Northwind.Invoice.GetWorkspace();
            IDataList results = workspace.GetQueryData("SalesByEmployeeCountry");
            DataList.Write(results, Console.Out);
        }
    }
}
