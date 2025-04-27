using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiftLab.ViewModels;

namespace LiftLab.Models
{
    public class WorkoutSelection : BaseViewModel
    {
        private bool isSelected;

        public Workouts Workout { get; set; }
        public int? Reps { get; set; }
        public int? Sets { get; set; }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (SetProperty(ref isSelected, value))
                {
                    // Trigger selection changed event
                    SelectionChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler SelectionChanged;
        //public Workouts Workout { get; set; }
        //public bool IsSelected { get; set; }
        //public int? Reps { get; set; }
        //public int? Sets { get; set; }

    }
}
