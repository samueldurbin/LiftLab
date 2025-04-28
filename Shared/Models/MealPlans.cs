using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class MealPlans
    {
        [Key]
        public int MealPlanId { get; set; }

        [Required]
        public int UserId { get; set; } // foreign key to users table

        [Required]
        public string MealPlanName { get; set; } // name of the meal plan

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // gets the date it was created

        public ICollection<MealPlanMeals> MealPlanMeals { get; set; } = new List<MealPlanMeals>();
    }
}
