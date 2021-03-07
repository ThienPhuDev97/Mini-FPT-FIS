using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class TypeAnswer
    {
        [Required]
        public int ID_Type { get; set; }
        [Required]
        public string Name_Type { get; set; }
    }
}