using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFCoreSamples
{
    public class NorthwindContext : DbContext
    {
        public DbSet<Invoices> Invoices { get; set; }
        public DbSet<Customers> Customers { get; set; }

        public NorthwindContext() : base()
        {
            Database.AutoTransactionsEnabled = false;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string NorthwindSampleUrl = @"http://services.odata.org/V4/Northwind/Northwind.svc";
            optionsBuilder.UseOData($@"Url={NorthwindSampleUrl};Use Cache=True");
        }        
    }

    //[DataContract]
    public class Invoices
    {
        [Key]
        //[DataMember(Name = "OrderID")]
        public int OrderID { get; set; }
        //[DataMember(Name = "ShipName")]
        public string ShipName { get; set; }
        //[DataMember(Name = "ShipAddress")]
        public string ShipAddress { get; set; }
        //[DataMember(Name = "ShipCity")]
        public string ShipCity { get; set; }
        //[DataMember(Name = "ShipRegion")]
        public string ShipRegion { get; set; }
        //[DataMember(Name = "ShipPostalCode")]
        public string ShipPostalCode { get; set; }
        //[DataMember(Name = "ShipCountry")]
        public string ShipCountry { get; set; }
        //[DataMember(Name = "CustomerID")]
        public string CustomerID { get; set; }
        //[DataMember(Name = "CustomerName")]
        public string CustomerName { get; set; }
        //[DataMember(Name = "Address")]
        public string Address { get; set; }
        //[DataMember(Name = "ShippedDate")]
        public DateTimeOffset? ShippedDate { get; set; }
    }

    public class Customers
    {
        [Key]
        public string CustomerID { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }        
        public string City { get; set; }        
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
    }
}
