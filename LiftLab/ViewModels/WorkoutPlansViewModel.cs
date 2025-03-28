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

        public ObservableCollection<WorkoutPlans> UsersWorkoutPlans { get; set; } = new ObservableCollection<WorkoutPlans>(); // this will hold all of the incoming workoutplans created by the user
        public ObservableCollection<WorkoutSelection> WorkoutList { get; set; } // this will hold the list of workouts within each workoutplan

        // onclick buttons for function calls
        public ICommand LoadUserWorkoutPlansCommand { get; } // onclick to load workoutplans created by the user on the page
        public ICommand ViewWorkoutsCommand { get; } // onclick to view a selected workout plan's workouts
        public ICommand CreatePlanCommand { get; } // onclick to create a workoutplan
        public ICommand GetWorkoutsCommand { get; } // onlick to get workouts
        public ICommand NavigateToWorkoutSelectionCommand { get; }

        // buttons for navigation
        public ICommand CreateWorkoutPlanButtonCommand { get; } // onclick to navigate user to the create workoutplan page, better for redirection
        public ICommand ViewWorkoutPlansButtonCommand{ get; } // onclick to navigate user to View All Workout Plans

        public string WorkoutPlanName // this helps bind the user input in the UI to the function
        {
            get => workoutPlanName;
            set => SetProperty(ref workoutPlanName, value); // this notifies the ui of changes
        }

        public WorkoutPlansViewModel()
        {
            _workoutPlansService = new WorkoutPlansServiceUI(); // initialises the WorkoutPlansServiceUI

            WorkoutList = new ObservableCollection<WorkoutSelection>(); // initialises the workoutlist

            CreatePlanCommand = new Command(async () => await CreatePlan()); // creates a workout plan when clicked

            GetWorkoutsCommand = new Command(async () => await GetWorkouts()); // get all workouts to populate the list

            CreateWorkoutPlanButtonCommand = new Command(async () => // add button in the ui navigates to create a plan
            {
                await Shell.Current.GoToAsync(nameof(CreateWorkoutPlanPage)); // shell navigation to move to create a workout plan page
            });

            ViewWorkoutPlansButtonCommand = new Command(async () => // add button in the ui navigates to create a post
            {
                await Shell.Current.GoToAsync(nameof(ViewAllWorkoutPlans)); // shell navigation to view all workout plans page
            });

            ViewWorkoutsCommand = new Command<WorkoutPlans>(async (selectedPlan) => // redirects user to view a page of the workouts in a selected plan
            {
                if (selectedPlan == null) return; // if no plan is selected, nothing happens

                await Shell.Current.GoToAsync($"{nameof(ViewAllWorkoutsPage)}",
                    new Dictionary<string, object>
                    {
                { "WorkoutPlanData", selectedPlan } // passing plan as a query param
                    });
            });

            LoadUserWorkoutPlansCommand = new Command(async () => await LoadUserWorkoutPlans()); // this will be used to load the workout plans on load

            NavigateToWorkoutSelectionCommand = new Command(async () =>
            {
                await GetWorkouts(); //

                await Shell.Current.GoToAsync(nameof(WorkoutSelectionPage));
            });

        }

        private async Task CreatePlan() // method to create a workout plan
        {
            try
            {
                var selectedWorkouts = WorkoutList // selected workout ids and into a list
                    .Where(w => w.IsSelected)
                    .Select(w => w.Workout.WorkoutId)
                    .ToList();

                int userId = Preferences.Get("UserId", 0); // this method gets the user id preferences from the login

                var newPlan = await _workoutPlansService.CreateWorkoutPlan(new CreateWorkoutPlan // creates new workout plan and puts in the list of selected workouts
                {
                    WorkoutPlanName = WorkoutPlanName, // the workoutplan name that has been created by the user input
                    UserId = userId, // userid from preferences
                    WorkoutIds = selectedWorkouts // all of the workouts that have been selected by the user
                });

                if (newPlan != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Workout plan created with workouts successfully!", "Nice"); // success message if created successfully

                    foreach (var workout in WorkoutList)
                    {
                        workout.IsSelected = false; // resets selected workouts from the list
                    }
                }
            }
            catch (Exception ex) // catch error message
            {
                await Application.Current.MainPage.DisplayAlert("Error!", $"Failed to create the new workout plan: {ex.Message}", "OK");
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
            catch (Exception ex) // catch error message
            {
                await Application.Current.MainPage.DisplayAlert("Error ", $"Failed to load the list of workouts: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false; // resets isbusy
            }
        }

        private async Task LoadUserWorkoutPlans() // this method loads the workout plans for a logged in user
        {
            if (IsBusy) // this prevents multiple api calls
                return;

            IsBusy = true;

            try
            {
                int userId = Preferences.Get("UserId", 0); // uses te set preferences from the login viewmodel
                var plans = await _workoutPlansService.GetPlansByUserId(userId); // uses the HTTP Get request in the WorkoutPlansServiceUI

                UsersWorkoutPlans.Clear(); // clears data before displaying
                foreach (var plan in plans) // shows all the workoutplans created by the userid
                {
                    UsersWorkoutPlans.Add(plan);
                }
            }
            catch (Exception ex) // catch error message
            {
                await Application.Current.MainPage.DisplayAlert("Error!", $"Failed to load workout plans for the logged in user: {ex.Message}", "OK"); // error message
            }
            finally
            {
                IsBusy = false; // resets isbsusy
            }
        }

    }

}
