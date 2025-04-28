using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class Meals
    {
        [Key]
        public int MealId { get; set; }

        public string MealName { get; set; } // name of the meal

        public string Type { get; set; } // type of meal, so users will have the option to select breakfast, lunch or dinner etc
        
        public int? Calories { get; set; } // optional to put how many calories

        public string Recipe { get; set; } // essentially the description box

        public int? UserId { get; set; }

    }
}
