using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Question
    {
        
        public string ID_Cauhoi { get; set; }
        public int? ID_DM { get; set; }
        public string Name_DM { get; set; }
        public int? ID_DV { get; set; }
        public string Name_DV { get; set; }
        public int? ID_MT { get; set; }
        public string Name_MT { get; set; }
        public int? ID_Type { get; set; }
        public string Name_Type { get; set; }
        public string Content_QS { get; set; }
        public string UrlFile { get; set; }
        public string Level_Question { get; set; }
        public int? Id_Level { get; set; }
        public string NameDM { get; set; }
        public DateTime? EndDate { get; set; }
        public string CountAnswer { get; set; }
        public string Status { get; set; }
        public int TypeQuestion { get; set; }
    }
}