using Fotografolio.Data.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fotografolio.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        //public int id { get; set ; }
        public string api_token { get; set; }
    }
}
