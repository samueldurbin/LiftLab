using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
    public class Workouts
    {
        [Key]
        public int WorkoutId { get; set; }
        public string WorkoutName { get; set; }
        public int MuscleGroupId { get; set; }

        [ForeignKey("MuscleGroupId")]
        public WMuscleGroups WMuscleGroups { get; set; }

    }
}
