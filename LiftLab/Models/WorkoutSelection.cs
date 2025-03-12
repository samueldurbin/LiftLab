using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftLab.Models
{
    public class WorkoutSelection
    {
        public Workouts Workout { get; set; }
        public bool IsSelected { get; set; }
        public string WorkoutName => Workout.WorkoutName;
    }
}
