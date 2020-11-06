EFCoreSamples for NetCore3.1
------------------------------------------
Shows how to query data from OData server using EFCore database first.

The sample shows how to config a custom DbContext class to interact with tables from OData server.
Developers who work with EFCore database first will find it very straight forward:
- Create custom class to map with OData table/entity
- Create custom DbContext class to work with EFCore
- In custom DbContext, update code to connect to specified OData server:
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string NorthwindSampleUrl = @"http://services.odata.org/V4/Northwind/Northwind.svc";
            optionsBuilder.UseOData($@"Url={NorthwindSampleUrl};Use Cache=True");
        }     
- Now write code in UI to query data using EFCore as usual.