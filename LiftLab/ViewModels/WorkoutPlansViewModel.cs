using System;
using System.Collections.Generic;
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
    public class WorkoutPlansViewModel : BaseViewModel
    {
        private readonly WorkoutPlansServiceUI _workoutPlansService;
        private string workoutPlanName;
        private int userId;

        public ObservableCollection<WorkoutSelection> WorkoutList { get; set; }

        public ICommand CreatePlanCommand { get; }
        public ICommand GetWorkoutsCommand { get; }

        public int UserId
        {
            get => userId;
            set => SetProperty(ref userId, value);
        }

        public string WorkoutPlanName
        {
            get => workoutPlanName;
            set => SetProperty(ref workoutPlanName, value);
        }

        public WorkoutPlansViewModel()
        {
            _workoutPlansService = new WorkoutPlansServiceUI();
            WorkoutList = new ObservableCollection<WorkoutSelection>();

            CreatePlanCommand = new Command(async () => await CreatePlan());
            GetWorkoutsCommand = new Command(async () => await GetWorkouts());

        }

        private async Task CreatePlan()
        {
            try
            {
                var selectedWorkouts = WorkoutList
                    .Where(w => w.IsSelected)
                    .Select(w => w.Workout.WorkoutId)
                    .ToList();

                var newPlan = await _workoutPlansService.CreatePlan(new CreateWorkoutPlan
                {
                    WorkoutPlanName = WorkoutPlanName,
                    UserId = UserId,
                    WorkoutIds = selectedWorkouts
                });

                if (newPlan != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Workout plan created with workouts successfully!", "OK");

                    foreach (var workout in WorkoutList)
                    {
                        workout.IsSelected = false;
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to create plan: {ex.Message}", "OK");
            }
        }

        private async Task GetWorkouts()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var workouts = await _workoutPlansService.GetAllWorkouts();
                WorkoutList.Clear();

                foreach (var workout in workouts)
                {
                    WorkoutList.Add(new WorkoutSelection
                    {
                        Workout = workout,
                        IsSelected = false
                    });
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error Exception", $"Failed to load the workouts: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }

    public class WorkoutSelection
    {
        public Workouts Workout { get; set; }
        public bool IsSelected { get; set; }
        public string WorkoutName => Workout.WorkoutName;
    }
}
