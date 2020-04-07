using Fotografolio.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fotografolio.Data.Models
{
    public class Slide : IEntity
    {
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int id { get; set; }
        public string name { get; set; }
    }
}
