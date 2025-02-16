using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Shared.Models
{
    public class Workouts
    {
        [Key]
        public int WorkoutId { get; set; }

        [JsonPropertyName("workoutName")]
        public string WorkoutName { get; set; }

    }
}
