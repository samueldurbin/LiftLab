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
        public List<int> WorkoutIds { get; set; }

    }
}
