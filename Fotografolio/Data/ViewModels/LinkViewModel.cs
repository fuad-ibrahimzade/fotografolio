using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fotografolio.Data.ViewModels
{
    public class LinkViewModel
    {
        [Required]
        public string facebook { get; set; }
        [Required]
        public string instagram { get; set; }
        [Required]
        public string twitter { get; set; }
        [Required]
        public string youtube { get; set; }
    }
}
