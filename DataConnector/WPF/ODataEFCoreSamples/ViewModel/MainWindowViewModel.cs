using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreSamples.ViewModel
{
    class MainWindowViewModel
    {
        public ObservableCollection<InvoiceInfo> InvoiceInfos { get; private set; } = new ObservableCollection<InvoiceInfo>();
        public IList<string> Countries { get; private set; }

        public MainWindowViewModel()
        {
            using (var context = new NorthwindContext())
            {
                Countries = (from c in context.Customers select c.Country).Distinct().ToList();
            }
        }

        public void SetSelectedCountry(string country)
        {
            using (var context = new NorthwindContext())
            {
                var filteredQuery = (from p in context.Invoices
                                        from c in context.Customers
                                        where p.CustomerID == c.CustomerID && c.Country.Equals(country, StringComparison.OrdinalIgnoreCase)
                                        select new
                                        {
                                            p.OrderID,
                                            p.ShipName,
                                            p.ShipAddress,
                                            p.CustomerName,
                                            c.CompanyName,
                                            c.ContactName,
                                            p.ShippedDate
                                        }).ToList();

                var invoices = InvoiceInfos;
                invoices.Clear();
                foreach (var result in filteredQuery)
                {
                    invoices.Add(new InvoiceInfo()
                    {
                        OrderID = result.OrderID,
                        ShipName = result.ShipName,
                        ShipAddress = result.ShipAddress,
                        CustomerName = result.CustomerName,
                        CompanyName = result.CompanyName,
                        ContactName = result.ContactName,
                        ShippedDate = result.ShippedDate
                    });
                }
            }
        }

        public void SetSearchText(string searchText, string country)
        {
            using (var context = new NorthwindContext())
            {
                var filteredQuery = (from p in context.Invoices
                                     from c in context.Customers
                                     where p.CustomerID == c.CustomerID && c.Country.Equals(country, StringComparison.OrdinalIgnoreCase) && 
                                     (  c.CompanyName.Contains(searchText) || c.ContactName.Contains(searchText)
                                     || p.ShipName.Contains(searchText) || p.ShipAddress.Contains(searchText) || p.CustomerName.Contains(searchText))
                                     select new
                                     {
                                         p.OrderID,
                                         p.ShipName,
                                         p.ShipAddress,
                                         p.CustomerName,
                                         c.CompanyName,
                                         c.ContactName,
                                         p.ShippedDate
                                     }).ToList();

                var invoices = InvoiceInfos;
                invoices.Clear();
                foreach (var result in filteredQuery)
                {
                    invoices.Add(new InvoiceInfo()
                    {
                        OrderID = result.OrderID,
                        ShipName = result.ShipName,
                        ShipAddress = result.ShipAddress,
                        CustomerName = result.CustomerName,
                        CompanyName = result.CompanyName,
                        ContactName = result.ContactName,
                        ShippedDate = result.ShippedDate
                    });
                }
            }
        }
    }

    public class InvoiceInfo
    {
        public int OrderID { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string CustomerName { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public DateTimeOffset? ShippedDate { get; set; }

    }

}
