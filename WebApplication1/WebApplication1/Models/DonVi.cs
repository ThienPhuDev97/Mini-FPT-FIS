using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class DonVi
    {
        [Required]
        public int ID_DV { get; set; }
        [Required]
        public string NameDV { get; set; }
    }
}