using Fotografolio.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fotografolio.Data.Models
{
    public class About : IEntity
    {
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public string desc { get; set; }
        public string photo { get; set; }
    }
}
