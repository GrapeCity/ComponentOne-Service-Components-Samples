using System;
using System.IO;
using System.IO.Compression;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using C1.DataEngine;

namespace PetLicenses
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create and initialize a new workspace folder relative to the project root
            Workspace workspace = new Workspace();
            workspace.KeepFiles = KeepFileType.Results;
            workspace.Init("workspace");

            // Uncomment the following line to clear the workspace before each run
            workspace.Clear();

            // Extract JSON data from a zip file, if not already done
            if (!File.Exists("seattle-pet-licenses.json") || !File.Exists("washington-zip-codes.json"))
            {
                ZipFile.ExtractToDirectory("data.zip", ".");
            }

            // Import the main license table
            if (!workspace.TableExists("PetLicenses"))
            {
                List<PetLicense> collection = JsonConvert.DeserializeObject<List<PetLicense>>(File.ReadAllText("seattle-pet-licenses.json"));
                ObjectConnector<PetLicense> connector = new ObjectConnector<PetLicense>(workspace, collection);
                connector.GetData("PetLicenses");
                workspace.Save();
                Console.WriteLine("{0:d} licenses imported.", collection.Count);
            }

            // Import the secondary location table
            if (!workspace.TableExists("Locations"))
            {
                List<Location> collection = JsonConvert.DeserializeObject<List<Location>>(File.ReadAllText("washington-zip-codes.json"));
                ObjectConnector<Location> connector = new ObjectConnector<Location>(workspace, collection);
                connector.GetData("Locations");
                workspace.Save();
                Console.WriteLine("{0:d} locations imported.", collection.Count);
            }

            // Retrieve the main table for use in constructing queries
            dynamic licenses = workspace.table("PetLicenses");

            // Number of licenses, by species
            if (!workspace.QueryExists("BySpecies"))
            {
                dynamic query = workspace.query("BySpecies", new {
                    licenses.Species,
                    Count = Op.Count(licenses.Species)
                });

                query.Query.Execute();
            }

            // Number of licenses, by calendar year
            if (!workspace.QueryExists("ByYear"))
            {
                // Create a query with all base table columns and add a Year field, extracted from the DateTime value
                dynamic parent = workspace.query(new {
                    _base = "*",
                    Year = Op.DtPart(licenses.IssueDate, DateTimeParts.Year)
                });

                // Derive another query from the unnamed query above and perform the aggregation
                dynamic query = workspace.query("ByYear", new {
                    parent.Year,
                    Count = Op.Count(parent.Year)
                });

                query.Query.Execute();
            }

            // Most popular dog names (sort criteria and row limits are applied later)
            if (!workspace.QueryExists("DogNames"))
            {
                // Use the _range attribute to limit the results to dogs only
                dynamic query = workspace.query("DogNames", new {
					_range = licenses.Species.Eq("Dog"),
					Species = licenses.Species,
                    DogName = licenses.AnimalName,
                    Count = Op.Count(licenses.AnimalName)
                });

                query.Query.Execute();
            }

            // List names for all species except dogs and cats 
            if (!workspace.QueryExists("OtherAnimals"))
            {
                // Create a query with specified base table columns and use the _filter attribute to limit the results
                // (_filter is used instead of _range because the latter does not support the Ne operator)
                dynamic parent = workspace.query(new {
                    _filter = licenses.Species.Ne("Dog").And().Ne("Cat"),
                    licenses.LicenseNumber,
                    licenses.Species,
                    licenses.AnimalName
                });

                // Derive another query from the unnamed query above and use First as the aggregation operator
                // (otherwise the query will not yield any results)
                dynamic query = workspace.query("OtherAnimals", new {
                    parent.LicenseNumber,
                    parent.Species,
                    AnimalName = Op.First(parent.AnimalName)
                });

                query.Query.Execute();
            }

            // Retrieve the secondary table for use in a join query
            dynamic locations = workspace.table("Locations");

            // Number of licenses in King County, by city
            if (!workspace.QueryExists("KingCounty"))
            {
                // Create a join from the main table (licenses) to the secondary table (locations)
                // * The addition expression to the left of the vertical bar denotes which secondary fields to include
                // * The equality expression to the right of the vertical bar specifies the join condition
                // * The assignment statement specifies a property of the anonymous type (its name is not significant)
                dynamic join = workspace.join(licenses, new {
                    locale = locations.County + locations.City | licenses.ZipCode == locations.Zip
                });

                // Derive another query from the join query above, specifying both a _range and an aggregation
                dynamic query = workspace.query("KingCounty", new {
                    _range = join.County.Eq("King"),
                    join.County,
                    join.City,
                    Count = Op.Count(join.LicenseNumber)
                });

                query.Query.Execute();
            }

            // Output query results in CSV format to the console
            IDataList species = workspace.GetQueryData("BySpecies");
            Console.WriteLine();
            Console.WriteLine("// Licenses issued, by species");
            DataList.Write(species, Console.Out);

            IDataList years = workspace.GetQueryData("ByYear");
            Console.WriteLine();
            Console.WriteLine("// Licenses issued, by year");
            DataList.Write(years, Console.Out);

            IDataList names = workspace.GetQueryData("DogNames", 10); // Limit results to 10 rows
            Console.WriteLine();
            Console.WriteLine("// Most popular dog names");
            DataList.Sort(names, "Count", false); // Sort by Count in descending order
            DataList.Write(names, Console.Out);

            IDataList others = workspace.GetQueryData("OtherAnimals");
            Console.WriteLine();
            Console.WriteLine("// Excluding dogs and cats");
            DataList.Write(others, Console.Out);

            IDataList cities = workspace.GetQueryData("KingCounty");
            Console.WriteLine();
            Console.WriteLine("// Licenses issued in King County, by city");
            DataList.Write(cities, Console.Out);
        }
    }
}
