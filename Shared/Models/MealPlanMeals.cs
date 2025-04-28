using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class MealPlanMeals
    {
        [Key]
        public int MealPlanMealId { get; set; }
        public int MealPlanId { get; set; }
        public MealPlans? MealPlan { get; set; }
        public int MealId { get; set; }
        public Meals? Meal { get; set; }

    }
}
