using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmailParserDemo.Models
{
  public class EmailParserRequestModel
  {
      [Required]
      public string FileName { get; set; }

      [Required]
      public IFormFile File { get; set; }

      public IFormFile Template { get; set; }
    
  }
}
