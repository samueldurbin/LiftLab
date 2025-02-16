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

        [ForeignKey("WorkoutPlanId")]
        public WorkoutPlans WorkoutPlan { get; set; }

        [ForeignKey("WorkoutId")]
        public Workouts Workout { get; set; }
    }
}
