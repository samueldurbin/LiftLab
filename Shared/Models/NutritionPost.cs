using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class NutritionPost
    {
        public int NutritionPostId { get; set; } 
        public string Username { get; set; }
        public string ImageUrl { get; set; }
        public string Caption { get; set; }
        public int? MealPlanId { get; set; } // optional as not every post needs an added meal plan
    }
}
