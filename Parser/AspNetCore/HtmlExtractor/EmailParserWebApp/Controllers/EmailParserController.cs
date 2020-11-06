using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using C1.TextParser;
using EmailParserDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

using System.Collections;
using System.Collections.Generic;


namespace EmailParserDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailParserController : ControllerBase
    {
        // GET: api/emailparser
        [HttpGet("default_files")]
        public IActionResult Get()
        {
            Console.WriteLine("Get Default files");
            ArrayList files = new ArrayList();


            foreach (string file in Directory.EnumerateFiles(
            "Resources",
            "*",
            SearchOption.AllDirectories)
            )
            {
                
                files.Add(file.Replace("Resources", "").Replace(@"\","/"));
                // do something
                Console.WriteLine(file);
            }


            //IDirectoryContents contents = fileProvider.GetDirectoryContents("wwwroot/assets");
            //var listContent = contents.ToList();
            //listContent.ForEach(item => {
            //    Console.WriteLine(item.Name);
            //});
            
            return Ok(files);
        }

        // POST: api/emailparser
        [HttpPost("extract_data")]
        public async Task<IActionResult> ExtractData([FromForm] EmailParserRequestModel model)
        {
            IExtractionResult result = null;
            string content = "";
            if (model.Template == null)
            {
                content = "No template found!";
            }
            try
            {
                if (model.Template != null)
                {
                    result = this._Extract(model.Template.OpenReadStream(), model.File.OpenReadStream());
                    if (result != null)
                    {
                        content = result.ToJsonString();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                content = "Can not extract data \n";
                content += e.ToString();
            }

            return Ok(new { status = result == null ? 0 : 1, content = content });
        }


        private IExtractionResult _Extract(Stream templateStream, Stream source)
        {
            IExtractionResult result = null;

            try
            {
                HtmlExtractor _Template =  HtmlExtractor.Load(templateStream);
                result = _Template.Extract(source);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            return result;
        }


    }
}
