using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class UpdateWorkoutInPlanDTO
    {
        public int WorkoutPlanId { get; set; }
        public int WorkoutId { get; set; }
        public int? Reps { get; set; }
        public int? Sets { get; set; }
        public double? Kg { get; set; }
    }
}
