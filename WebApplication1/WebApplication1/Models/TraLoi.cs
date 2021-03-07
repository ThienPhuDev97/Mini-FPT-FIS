using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class TraLoi
    {
        public int? ID_AS { get; set; }
        public string Content_AS { get; set; } = null;
        public string Explain { get; set; } = null;
        public int? ID_TypesAs { get; set; }
        public string ID_Cauhoi { get; set; } = null;
    }
}