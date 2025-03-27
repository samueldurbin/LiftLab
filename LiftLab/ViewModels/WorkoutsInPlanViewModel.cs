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
    public class WorkoutsInPlanViewModel : BaseViewModel // this viewmodel is for the workouts that are contained in a workout plan
    {
        private readonly WorkoutPlansServiceUI _workoutPlansService;

        public ObservableCollection<Workouts> WorkoutsInPlan { get; set; } = new(); // workouts associated with a selected workoutplan

        private WorkoutPlans selectedPlan;

        public WorkoutPlans SelectedPlan
        {
            get => selectedPlan;
            set
            {
                selectedPlan = value; // sets new selected workout plan
                OnPropertyChanged();
                LoadWorkoutsForSelectedPlan(); // loads the workouts for the selectedworkout plan
            }
        }

        public WorkoutsInPlanViewModel()
        {
            _workoutPlansService = new WorkoutPlansServiceUI(); // initialises the WorkoutPlansServiceUI

        }

        public async void LoadWorkoutsForSelectedPlan() // this method will be used to load the workouts associated for a selected workout plan
        {
            try
            {
                if (SelectedPlan == null) 
                {
                    return; // if no plan is selected
                } 

                var workoutIds = await _workoutPlansService.GetWorkoutsByPlanId(SelectedPlan.WorkoutPlanId); // calls the method in the service to get the associated workout ids to the selected plan
                var allWorkouts = await _workoutPlansService.GetAllWorkouts(); // get all workouts

                var filteredWorkouts = allWorkouts // gets the workouts that match the ones selected
                    .Where(w => workoutIds.Contains(w.WorkoutId))
                    .ToList();

                WorkoutsInPlan.Clear(); // clear
                foreach (var workout in filteredWorkouts) // this adds each workouts into a list to be shown on the view
                {
                    WorkoutsInPlan.Add(workout);
                }
            }
            catch (Exception ex) // catch exception message
            {
                await Application.Current.MainPage.DisplayAlert("Error!", $"Could not loadthe list of workouts: {ex.Message}", "OK"); // alert message if incoming data did not load
            }
        }

    }
}
