using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fotografolio.Data.ViewModels
{
    public class AboutViewModel
    {
        [File(FileTypes = new string[] { "image/jpeg", "image/png", "image/bmp", "image/gif", "image/svg+xml" })]
        public IFormFile photo { get; set; }
        [Required]
        public string title { get; set; }
        [Required]
        public string desc { get; set; }
    }
}
