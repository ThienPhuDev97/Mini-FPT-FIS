using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class MonThi
    {
        [Required]
        public int ID_MT { get; set; }
        [Required]
        public string Name_MT { get; set; }
    }
}