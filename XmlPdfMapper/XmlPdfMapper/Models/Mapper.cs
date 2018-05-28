using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace XmlPdfMapper.Models
{
    public class Mapper
    {
        [Key]
        public int MapperId { get; set; }
        public string FileName { get; set; }
        public string PdfName { get; set; }
        public string XmlXpath { get; set; }
    }
}