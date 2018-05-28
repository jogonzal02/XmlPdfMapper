using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using XmlPdfMapper.Models;

namespace XmlPdfMapper.Models
{
    public class MapperContext: DbContext 
    {
        public MapperContext() : base("DefaultConnection") {
        }

        public DbSet<Mapper> Mappers { get; set; }
    }
}