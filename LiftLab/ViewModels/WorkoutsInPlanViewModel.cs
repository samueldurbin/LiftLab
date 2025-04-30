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
        public ICommand SaveWorkoutCommand { get; }
        public ICommand DeletePlanCommand { get; }
        public ICommand DeleteWorkoutCommand { get; }

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
            SaveWorkoutCommand = new Command<WorkoutInPlanDisplay>(async (workout) => await SaveWorkout(workout));
            DeleteWorkoutCommand = new Command<WorkoutInPlanDisplay>(async (workout) => await DeleteWorkout(workout));
            DeletePlanCommand = new Command(async () => await DeleteWorkoutPlan());
        }

        public async void LoadWorkoutsForSelectedPlan()
        {
            try
            {
                if (SelectedPlan == null)
                {
                    return;

                }

                var workoutPlanData = await _workoutPlansService.GetWorkoutDetailsForPlan(SelectedPlan.WorkoutPlanId);

                var allWorkouts = await _workoutPlansService.GetAllWorkouts();

                WorkoutsInPlan.Clear();
                foreach (var item in workoutPlanData)
                {
                    var workout = allWorkouts.FirstOrDefault(w => w.WorkoutId == item.WorkoutId);
                    if (workout != null)
                    {
                        WorkoutsInPlan.Add(new WorkoutInPlanDisplay
                        {
                            WorkoutId = item.WorkoutId,
                            WorkoutName = workout.WorkoutName,
                            Reps = item.Reps,
                            Sets = item.Sets,
                            Kg = item.Kg,
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error!", $"Could not load workouts: {ex.Message}", "OK");
            }
        }

        private async Task SaveWorkout(WorkoutInPlanDisplay workout)
        {
            try
            {
                if (SelectedPlan == null || workout == null)
                {
                    return;

                }

                await _workoutPlansService.UpdateWorkoutInPlan(SelectedPlan.WorkoutPlanId, workout);

                await Application.Current.MainPage.DisplayAlert("Success", "Workout updated!", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to update workout: {ex.Message}", "OK");
            }
        }

        private async Task DeleteWorkout(WorkoutInPlanDisplay workout)
        {
            try
            {
                if (SelectedPlan == null || workout == null)
                    return;

                var response = await _workoutPlansService.DeleteWorkoutFromPlan(SelectedPlan.WorkoutPlanId, workout.WorkoutId);

                if (response)
                {
                    WorkoutsInPlan.Remove(workout);
                    await Application.Current.MainPage.DisplayAlert("Success", "Workout removed from plan!", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to remove workout from plan.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to remove workout: {ex.Message}", "OK");
            }
        }

        private async Task DeleteWorkoutPlan()
        {
            if (SelectedPlan == null)
                return;

            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Delete Plan?",
                $"Are you sure you want to delete the plan \"{SelectedPlan.WorkoutPlanName}\"?",
                "Yes", "Cancel");

            if (!confirm) return;

            try
            {
                var response = await _workoutPlansService.DeleteWorkoutPlan(SelectedPlan.WorkoutPlanId);

                if (response)
                {
                    await Application.Current.MainPage.DisplayAlert("Deleted", "Workout plan deleted successfully.", "OK");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Could not delete workout plan as it's attached to a public post. To delete you will need to delete the post first", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to delete plan: {ex.Message}", "OK");
            }
        }
    }


    public class WorkoutInPlanDisplay
    {
        public int WorkoutId { get; set; }
        public string WorkoutName { get; set; }
        public int? Reps { get; set; }
        public int? Sets { get; set; }
        public double? Kg { get; set; }
    }
}

