using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using C1.DataEngine;

namespace DataEngineDesigner
{
    public class Program
    {
        private static Workspace workspace;

        public static Workspace Workspace
        {
            get { return workspace; }
        }

        public static void Main(string[] args)
        {
            Initialize();
            CreateHostBuilder(args).Build().Run();
        }

        public static void Initialize()
        {
            // Create and initialize a new workspace folder relative to the project root
            workspace = new Workspace();
            workspace.KeepFiles = KeepFileType.Results;
            workspace.Init("workspace");

            // Uncomment the following line to clear the workspace before each run
            // workspace.Clear();

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
            }

            // Import the secondary location table
            if (!workspace.TableExists("Locations"))
            {
                List<Location> collection = JsonConvert.DeserializeObject<List<Location>>(File.ReadAllText("washington-zip-codes.json"));
                ObjectConnector<Location> connector = new ObjectConnector<Location>(workspace, collection);
                connector.GetData("Locations");
                workspace.Save();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
