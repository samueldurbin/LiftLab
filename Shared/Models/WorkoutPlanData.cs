using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class WorkoutPlanData
    {
        [Key]
        public int WorkoutPlanDataId { get; set; }
        public int WorkoutPlanId { get; set; }
        public int WorkoutId { get; set; }
        public int? Reps { get; set; }
        public int? Sets { get; set; }
        public double? Kg { get; set; }

        [ForeignKey("WorkoutPlanId")] // foreign key to workout plans table
        public WorkoutPlans WorkoutPlan { get; set; }

        [ForeignKey("WorkoutId")] // foreign key to workouts
        public Workouts Workout { get; set; }
    }
}
