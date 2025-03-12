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
        private readonly FitnessPostServiceUI _fitnessPostService;

        private string username;
        private string imageUrl;
        private string caption;
        private DateTime createdDate;
        private WorkoutPlans selectedWorkoutPlan; // stores the selected workout plan
        public ObservableCollection<WorkoutPlans> WorkoutPlans { get; set; } // collection of workoutplans, ui automatically updates through this dynamic list

        public CreatePostViewModel()
        {
            _fitnessPostService = new FitnessPostServiceUI(); // creates instance
            WorkoutPlans = new ObservableCollection<WorkoutPlans>();
            Task.Run(async () => await LoadWorkoutPlans()); // this allows the loadworkoutplans command to initiate on run
            CreatePostCommand = new Command(async () => await CreatePost()); // button on click
        }

        public string Username // gets ui data and sets them into the variables
        {
            get => username;
            set => SetProperty(ref username, value);
        }

        public string ImageUrl
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
        public ICommand CreatePostCommand { get; } // creates post onclick

        public async Task LoadWorkoutPlans()  // loads all of the workoutplans from the api
        {
            try
            {
                var plans = await _fitnessPostService.GetAllPlans(); // fetches all of the plans

                WorkoutPlans.Clear(); // this removes old data that may have been deletec or edited previously

                foreach (var plan in plans)
                {
                    WorkoutPlans.Add(plan); // adds all the plans to the ui list
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to load workout plans", "OK");
            }
        }

        private async Task CreatePost()
        {
            var newPost = await _fitnessPostService.CreatePost(Username, ImageUrl, Caption, SelectedWorkoutPlan.WorkoutPlanId); // creates post with the parameters // selectedworkoutplan is the selected from the ui

            if (newPost != null)
            {
                await Application.Current.MainPage.DisplayAlert("Nice!", "Your post has been created successfully!", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "FitnessPost creation has failed.", "OK");
            }
        }

    }
}

