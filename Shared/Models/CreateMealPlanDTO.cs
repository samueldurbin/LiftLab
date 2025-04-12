using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class CreateMealPlanDTO
    {
        public int UserId { get; set; }
        public string MealPlanName { get; set; }
        public List<Meals> Meals { get; set; }
    }
}
