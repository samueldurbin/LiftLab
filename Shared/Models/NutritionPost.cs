using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class NutritionPost
    {
        [Key]
        public int NutritionPostId { get; set; } 
        public string Username { get; set; }
        public string ImageUrl { get; set; }
        public string Caption { get; set; }
    }
}
