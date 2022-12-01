using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using C1.DataEngine;
using C1.PivotEngine;

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

        public static async Task<Workspace> GetWorkspace()
        {
            string path = "workspace"; // absolute or relative path to the workspace folder  
            Workspace workspace = new Workspace();
            workspace.Init(path);
            workspace.Clear();

            HttpClient hc = new HttpClient();
            string download = await hc.GetStringAsync("https://services.odata.org/v4/Northwind/Northwind.svc/Invoices");

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

        public static C1PivotEngine GetPivotEngine(Workspace workspace)
        {
            C1PivotEngine pe = new C1PivotEngine();
            pe.Workspace = workspace;
            pe.ConnectDataEngine("SalesByEmployeeCountry");
            pe.BeginUpdate();
            pe.ColumnFields.Add("Country");
            pe.RowFields.Add("Salesperson");
            pe.ValueFields.Add("Sales");
            pe.EndUpdate();
            return pe;
        }
        
        public static C1PivotEngine GetPivotEngine(Workspace workspace, Action<C1PivotEngine> action)
        {
            C1PivotEngine pe = new C1PivotEngine();
            pe.Workspace = workspace;
            pe.ConnectDataEngine("SalesByEmployeeCountry");
            pe.UpdateCompleted += (s, e) =>
            {
                action.Invoke(pe);
            };
            pe.BeginUpdate();
            pe.ColumnFields.Add("Country");
            pe.RowFields.Add("Salesperson");
            pe.ValueFields.Add("Sales");
            pe.EndUpdate();
            return pe;
        }
    }
}
