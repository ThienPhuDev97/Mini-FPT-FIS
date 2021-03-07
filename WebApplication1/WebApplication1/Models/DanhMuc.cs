using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class DanhMuc
    {
        [Required]
        public int ID_DM { get; set; }
        [Required]
        public string NameDM { get; set; }
    }
}