using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using C1.DataEngine;

namespace DataEngineDesigner.Pages
{
    public class DesignerModel : PageModel
    {
        public Dictionary<string, List<string>> BaseTables { get; set; }
        
        public void OnGet()
        {
            BaseTables = new Dictionary<string, List<string>>();
            
            foreach (string name in Program.Workspace.GetTableNames())
            {
                AddTable(name);
            }
        }

        private void AddTable(string name)
        {
            if (Program.Workspace.TableExists(name))
            {
                var table = Program.Workspace.table(name);
                var fields =
                    from pair in table
                    where !(pair.Value is Table)
                    select pair.Key;
                BaseTables.Add(name, fields.ToList());
            }
        }
    }
}
