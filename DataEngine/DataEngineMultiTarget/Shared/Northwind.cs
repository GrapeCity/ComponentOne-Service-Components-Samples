using System;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using C1.DataEngine;
using C1.FlexPivot;

namespace Northwind
{
    public class Invoice
    {
        public string Country { get; set; }
        public string Salesperson { get; set; }
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string ProductName { get; set; }
        public double UnitPrice { get; set; }
        public double Quantity { get; set; }
        public double ExtendedPrice { get; set; }

        public static Workspace GetWorkspace()
        {
            string path = "workspace"; // absolute or relative path to the workspace folder  
            Workspace workspace = new Workspace();
            workspace.Init(path);
            workspace.Clear();

            WebClient wc = new WebClient();
            string download = wc.DownloadString("https://services.odata.org/v4/Northwind/Northwind.svc/Invoices");

            JObject root = JObject.Parse(download);
            JArray values = (JArray)root["value"];
            List<Invoice> collection = JsonConvert.DeserializeObject<List<Invoice>>(values.ToString());

            var connector = new ObjectConnector<Invoice>(workspace, collection);
            connector.GetData("Invoices");
            workspace.Save();

            dynamic invoices = workspace.table("Invoices");

            dynamic query = workspace.query("SalesByEmployeeCountry", new
            {
                invoices.Salesperson,
                invoices.Country,
                Sales = Op.Sum(invoices.ExtendedPrice)
            });

            query.Query.Execute();
            return workspace;
        }

        public static C1FlexPivotEngine GetPivotEngine(Workspace workspace)
        {
            C1FlexPivotEngine fp = new C1FlexPivotEngine();
            fp.Workspace = workspace;
            fp.ConnectDataEngine("SalesByEmployeeCountry");
            fp.BeginUpdate();
            fp.ColumnFields.Add("Country");
            fp.RowFields.Add("Salesperson");
            fp.ValueFields.Add("Sales");
            fp.EndUpdate();
            return fp;
        }
        
        public static C1FlexPivotEngine GetPivotEngine(Workspace workspace, Action<C1FlexPivotEngine> action)
        {
            C1FlexPivotEngine fp = new C1FlexPivotEngine();
            fp.Workspace = workspace;
            fp.ConnectDataEngine("SalesByEmployeeCountry");
            fp.UpdateCompleted += (s, e) =>
            {
                action.Invoke(fp);
            };
            fp.BeginUpdate();
            fp.ColumnFields.Add("Country");
            fp.RowFields.Add("Salesperson");
            fp.ValueFields.Add("Sales");
            fp.EndUpdate();
            return fp;
        }
    }
}
