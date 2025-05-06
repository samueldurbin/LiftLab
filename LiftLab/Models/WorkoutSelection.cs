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
        public int? Reps { get; set; } // nullable as its not mandatory to add
        public int? Sets { get; set; }

        public bool IsSelected // checks whether a workout is selected
        {
            get => isSelected;
            set
            {
                if (SetProperty(ref isSelected, value))
                {
                    SelectionChanged?.Invoke(this, EventArgs.Empty); // if te selection changed, raise the event
                }
            }
        }

        public event EventHandler SelectionChanged; // used for UI

    }
}
