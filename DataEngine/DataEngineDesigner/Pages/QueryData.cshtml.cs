using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using C1.DataEngine;
using Newtonsoft.Json;

namespace DataEngineDesigner.Pages
{
    public class QueryDataModel : PageModel
    {
        public async Task<JsonResult> OnGetAsync(string name, string sort)
        {
            JsonResult results = null;
            
            await Task.Run(() => {
                if (Program.Workspace.QueryExists(name))
                {
                    string className = name + DateTime.Now.Ticks.ToString();
                    IDataList test = Program.Workspace.GetQueryData(name);
                    if (!string.IsNullOrEmpty(sort))
                    {
                        var sorts = sort.Split(',');
                        for (int i = 0; i < sorts.Length; i++) {
                            string field = sorts[i];
                            bool ascend = field[0] == '+';
                            string[] parts = field.Substring(1).Split('.');
                            string prop = parts[parts.Length - 1];
                            DataList.Sort(test, prop, ascend);
                        }
                    }
                    var list = ClassFactory.CreateFromDataList(test, className);
                    results = new JsonResult(list);
                }
            });

            return results ?? new JsonResult(null);
        }

        public async Task<IActionResult> OnPostAsync([FromBody]RuntimeQuery query)
        {
            try
            {
                await Task.Run(() => {
                    QueryFactory.Delete(Program.Workspace, query.name);
                    dynamic test = QueryFactory.CreateQueryFromRuntimeQuery(Program.Workspace, query);
                    test.Query.Execute();
                });
                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }

        public async Task<IActionResult> OnDeleteAsync([FromBody]RuntimeQuery query)
        {
            try
            {
                await Task.Run(() => {
                    QueryFactory.Delete(Program.Workspace, query.name);
                });
                return StatusCode(200);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }
    }
}
