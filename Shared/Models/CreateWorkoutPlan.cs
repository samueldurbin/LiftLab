using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class CreateWorkoutPlan
    {
        public string WorkoutPlanName { get; set; }
        public int UserId { get; set; }
        public List<WorkoutInPlanDTO> Workouts { get; set; } // the list of workouts with reps & sets included in the workout plan

    }
}
