using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using C1.DataEngine;

namespace RazorPetLicenses.Pages
{
    public class IndexModel : PageModel
    {
        public List<object> BySpecies { get; set; }
        public List<object> ByYear { get; set; }
        public List<object> DogNames { get; set; }
        public List<object> OtherAnimals { get; set; }
        public List<object> KingCounty { get; set; }

        public void OnGet()
        {
            IDataList species = Program.Workspace.GetQueryData("BySpecies");
            BySpecies = ClassFactory.CreateFromDataList(species, "BySpecies");

            IDataList years = Program.Workspace.GetQueryData("ByYear");
            ByYear = ClassFactory.CreateFromDataList(years, "ByYear");

            IDataList names = Program.Workspace.GetQueryData("DogNames", 10); // Limit results to 10 rows
            DataList.Sort(names, "Count", false); // Sort by Count in descending order
            DogNames = ClassFactory.CreateFromDataList(names);

            IDataList others = Program.Workspace.GetQueryData("OtherAnimals");
            OtherAnimals = ClassFactory.CreateFromDataList(others, "OtherAnimals");

            IDataList king = Program.Workspace.GetQueryData("KingCounty");
            KingCounty = ClassFactory.CreateFromDataList(king, "KingCounty");
        }
    }
}
