using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class CreateMealPlanDTO
    {
        public string MealPlanName { get; set; }
        public int UserId { get; set; }
        public List<int> MealIds { get; set; } // list of mealids to be selected into a plan
    }
}
