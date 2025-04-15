using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiftLab.Views;
using System.Windows.Input;
using LiftLab.Services;
using Shared.Models;
using System.Collections.ObjectModel;

namespace LiftLab.ViewModels
{
    public class WorkoutsInPlanViewModel : BaseViewModel
    {
        private readonly WorkoutPlansServiceUI _workoutPlansService;

        public ObservableCollection<WorkoutInPlanDisplay> WorkoutsInPlan { get; set; } = new();

        private WorkoutPlans selectedPlan;

        public WorkoutPlans SelectedPlan
        {
            get => selectedPlan;
            set
            {
                selectedPlan = value;
                OnPropertyChanged();
                LoadWorkoutsForSelectedPlan();
            }
        }

        public WorkoutsInPlanViewModel()
        {
            _workoutPlansService = new WorkoutPlansServiceUI();
        }

        public async void LoadWorkoutsForSelectedPlan()
        {
            try
            {
                if (SelectedPlan == null)
                    return;

                // 1. Get reps, sets, workoutIds
                var workoutPlanData = await _workoutPlansService.GetWorkoutDetailsForPlan(SelectedPlan.WorkoutPlanId);

                // 2. Get all workouts (to resolve names)
                var allWorkouts = await _workoutPlansService.GetAllWorkouts();

                // 3. Join workoutId to get names
                WorkoutsInPlan.Clear();
                foreach (var item in workoutPlanData)
                {
                    var workout = allWorkouts.FirstOrDefault(w => w.WorkoutId == item.WorkoutId);
                    if (workout != null)
                    {
                        WorkoutsInPlan.Add(new WorkoutInPlanDisplay
                        {
                            WorkoutName = workout.WorkoutName,
                            Reps = item.Reps,
                            Sets = item.Sets
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error!", $"Could not load workouts: {ex.Message}", "OK");
            }
        }
    }

    public class WorkoutInPlanDisplay
    {
        public string WorkoutName { get; set; }
        public int? Reps { get; set; }
        public int? Sets { get; set; }
    }

}

