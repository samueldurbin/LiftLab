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
using LiftLab.Models;

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
        public ICommand CreateWorkoutPlanButtonCommand { get; } // instead of using navigation in the backend of the xaml file, its better practise to keep in viewmodel

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

            CreatePlanCommand = new Command(async () => await CreatePlan()); // links the methods to the buttons within the ui
            GetWorkoutsCommand = new Command(async () => await GetWorkouts());

            CreateWorkoutPlanButtonCommand = new Command(async () => // add button in the ui navigates to create a post
            {
                await Shell.Current.GoToAsync(nameof(CreatePost));
            });

        }

        private async Task CreatePlan()
        {
            try
            {
                var selectedWorkouts = WorkoutList // selected workout ids and into a list
                    .Where(w => w.IsSelected)
                    .Select(w => w.Workout.WorkoutId)
                    .ToList();

                var newPlan = await _workoutPlansService.CreatePlan(new CreateWorkoutPlan // creates new workout plan and puts in the list of selected workouts
                {
                    WorkoutPlanName = WorkoutPlanName,
                    UserId = UserId,
                    WorkoutIds = selectedWorkouts
                });

                if (newPlan != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Workout plan created with workouts successfully!", "Nice");

                    foreach (var workout in WorkoutList)
                    {
                        workout.IsSelected = false; // resets selected workouts from the list
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to create the new plan: {ex.Message}", "OK");
            }
        }

        private async Task GetWorkouts() // method to get workouts
        {
            if (IsBusy) // to prevent multiple api calls
                return;

            IsBusy = true; // to show loading after button is pressed

            try
            {
                var workouts = await _workoutPlansService.GetAllWorkouts(); // gets a list of all the workouts
                
                WorkoutList.Clear(); // clears old data

                foreach (var workout in workouts) // adds new workouts
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
                await Application.Current.MainPage.DisplayAlert("Error ", $"Failed to load the list of workouts: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false; // resets isbusy
            }
        }

    }

}
