using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LiftLab.Services;
using Shared.Models;

namespace LiftLab.ViewModels
{
    public class CreatePostViewModel : BaseViewModel
    {
        private readonly CommunityServiceUI _communityService;

        private string username;
        private string imageUrl;
        private string caption;
        private WorkoutPlans selectedWorkoutPlan; // stores the selected workout plan

        public ICommand CreatePostCommand { get; } // creates post onclick
        public ObservableCollection<WorkoutPlans> WorkoutPlans { get; set; } // collection of workoutplans, ui automatically updates through this dynamic list

        public string ImageUrl // this is how the user inpyt gets put into the methods below
        {
            get => imageUrl;
            set => SetProperty(ref imageUrl, value);
        }

        public string Caption
        {
            get => caption;
            set => SetProperty(ref caption, value);
        }

        public WorkoutPlans SelectedWorkoutPlan
        {
            get => selectedWorkoutPlan;
            set => SetProperty(ref selectedWorkoutPlan, value);
        }

        public CreatePostViewModel()
        {
            _communityService = new CommunityServiceUI(); // creates instance

            WorkoutPlans = new ObservableCollection<WorkoutPlans>(); // workout plans list

            Task.Run(async () => await LoadWorkoutPlans()); // this allows the loadworkoutplans command to initiate on run

            CreatePostCommand = new Command(async () => await CreatePost()); // button on click
        }

        public async Task LoadWorkoutPlans()  // loads all of the workoutplans from the api
        {
            try
            {
                var plans = await _communityService.GetAllPlans(); // fetches all of the plans

                WorkoutPlans.Clear(); // this removes old data that may have been deletec or edited previously

                foreach (var plan in plans)
                {
                    WorkoutPlans.Add(plan); // adds all the plans to the ui list
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error!", "Failed to load worout plans", "OK");
            }
        }

        private async Task CreatePost()
        {
            int userId = Preferences.Get("UserId", 0); // this method gets the user id from the login
            string username = Preferences.Get("Username", "Unknown");  // this gets the username from the login (soon to be replaced for userid)

            if (userId == 0) // if the userId is not existent
            {
                await Application.Current.MainPage.DisplayAlert("Not Account is Logged In", "Please Log In before making a post", "OK"); // alert message if no userid or username
                return;
            }

            int? planId = SelectedWorkoutPlan?.WorkoutPlanId; // this gets the workoutplan id that is selected by the user, but it can also be nullable

            var newPost = await _communityService.CreatePost(userId, username, ImageUrl, Caption, planId); // sends the input data

            if (newPost != null) // checks if the data has been successfully sent or not
            {
                await Application.Current.MainPage.DisplayAlert("Success!", "Your post has now been created!", "OK"); // success message
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to create your post.", "OK"); // error message
            }
        }
    }
}

