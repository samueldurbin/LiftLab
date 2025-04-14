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
using LiftLab.Helpers;
using System.Net.Http;
using CommunityToolkit.Maui.Views;

namespace LiftLab.ViewModels
{
    public class CommunityPostViewModel : BaseViewModel
    {
        private readonly CommunityServiceUI _communityService;  // gets community posts from api
        private readonly WorkoutPlansServiceUI _workoutPlansService;

        #region ICommands - OnClickLisenters

        public ICommand ViewUserProfileCommand { get; }
        public ICommand GetCommunityPostsCommand { get; }  // gets and displays posts
        public ICommand NavigationCommand { get; }
        public ICommand CreateCommentCommand { get; }
        public ICommand LoadCommunityPostsCommand { get; }
        public ICommand LikePostCommand { get; }
        public ICommand AddPlansToUserAccountCommand { get; }
        public ICommand AddFriendsCommand { get; }
        public ICommand AddWorkoutPlanCommand { get; }
        public ICommand ShowPlanDetailsCommand { get; }

        #endregion

        public ObservableCollection<CommunityPost> CommunityPosts { get; set; } // collection of posts objects

        #region Comment Variables
        private string comment;
        public string Comment // this is where the input of the users comment will be taken
        {
            get => comment;
            set => SetProperty(ref comment, value);
        }

        #endregion

        public CommunityPostViewModel()
        {
            _communityService = new CommunityServiceUI();
            _workoutPlansService = new WorkoutPlansServiceUI();

            CommunityPosts = new ObservableCollection<CommunityPost>(); // initializes empty to store fitness posts

            GetCommunityPostsCommand = new Command(async () => await GetsPosts());

            CreateCommentCommand = new Command<CommunityPost>(async (post) => await AddComment(post)); // assigns the function to a on vlick button in the ui

            NavigationCommand = new Command(async () => // add button in the ui navigates to create a post
            {
                await Shell.Current.GoToAsync(nameof(CreatePost));
            });

            AddFriendsCommand = new Command(async () => // add button in the ui navigates to create a post
            {
                await Shell.Current.GoToAsync(nameof(FriendsPage));
            });

            LoadCommunityPostsCommand = new Command(async () => await GetsPosts()); // this will be used to load the workout plans on load

            LikePostCommand = new Command<CommunityPost>(async (post) => await LikePost(post));

            AddWorkoutPlanCommand = new Command<CommunityPost>(async (post) => await AddExternalWorkoutPlan(post));

            ViewUserProfileCommand = new Command<CommunityPost>(async (post) => await ViewUserProfile(post));

            AddPlansToUserAccountCommand = new Command<CommunityPost>(async (post) => await AddPlansToUserAccount(post));

            ShowPlanDetailsCommand = new Command<CommunityPost>(async (post) => await ShowPlanDetails(post));

        }

        private async Task ShowPlanDetails(CommunityPost post) // this method is what will be called by the view icon to view a plan or meal
        {
            try // try catch method
            {
                if (post.WorkoutPlanId != null) // initially checks if the post contains a WorkoutPlanId (contains a workout plan)
                {
                    var userId = post.UserId;

                    var userPlans = await _communityService.GetWorkoutPlansByUserId(userId); // gets the workout plans made by the user who made the post

                    var thisPlan = userPlans.FirstOrDefault(p => p.WorkoutPlanId == post.WorkoutPlanId); // finds the plan attatched to the post

                    if (thisPlan != null) // checks if the plan exists and contains data
                    {
                        var workoutIds = await _workoutPlansService.GetWorkoutsByPlanId(thisPlan.WorkoutPlanId); // gets all associated workouts with the plan
                        var allWorkouts = await _workoutPlansService.GetAllWorkouts(); // then gets all the workouts that exists to match the ids with whats in the plan

                        var workoutNames = allWorkouts // filters through and gets the matching ids with a workoutname, and puts them into a list to be displayed in the popup view
                            .Where(w => workoutIds.Contains(w.WorkoutId))
                            .Select(w => w.WorkoutName)
                            .ToList();

                        var popup = new ViewAddedPlan(thisPlan.WorkoutPlanName ?? "Workout Plan", workoutNames, string.Empty); // this is the popup itsself that will display the workoutplan name with the list of workouts
                        await Shell.Current.CurrentPage.ShowPopupAsync(popup); // this initiates the popup view
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Not Found!", "Workout plan could not be found, please try again soon.", "OK"); // error display maessage if an error occured within the process
                    }
                }
                else if (post.MealPlanId != null) // checks if a meal plan is atta
                {
                    var userId = post.UserId;
                    var mealPlans = await new NutritionServiceUI().GetMealPlansByUser(userId);
                    var mealPlan = mealPlans.FirstOrDefault(mp => mp.MealPlanId == post.MealPlanId);

                    if (mealPlan != null)
                    {
                        var mealNames = mealPlan.Meals.Select(m => m.MealName).ToList();
                        var popup = new ViewAddedPlan(mealPlan.MealPlanName ?? "Meal Plan", mealNames, string.Empty);
                        await Shell.Current.CurrentPage.ShowPopupAsync(popup);
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Not Found", "Meal plan could not be found.", "OK");
                    }
                }
                else if (post.MealId != null)
                {
                    var userId = post.UserId;
                    var meals = await new NutritionServiceUI().GetMealsByUser(userId);
                    var meal = meals.FirstOrDefault(m => m.MealId == post.MealId);

                    if (meal != null)
                    {
                        var popup = new ViewAddedPlan(meal.MealName, new List<string>(), meal.Recipe);
                        await Shell.Current.CurrentPage.ShowPopupAsync(popup);
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Not Found", "Meal could not be found.", "OK");
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Oops", "No plan or meal attached to this post.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Could not load plan details: {ex.Message}", "OK");
            }

        }

        private async Task AddPlansToUserAccount(CommunityPost post)
        {
            try
            {
                int userId = Preferences.Get("UserId", 0); // user preferences

                bool addedPlan = await _communityService.AddExternalPlans(post, userId); // if the plus button is pressed, it calls the adding method from the service then presents a success or error message

                if (addedPlan)
                {
                    await Application.Current.MainPage.DisplayAlert("Success!", "The plan has been added to your account", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Unavailable!", "This post doesn't contain any plan attatched.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Something went wrong: {ex.Message}", "OK");
            }
        }

        private async Task ViewUserProfile(CommunityPost post) // viewing user profiles
        {
            if (post == null) return;

            int currentUserId = Preferences.Get("UserId", 0);

            if (post.UserId == currentUserId)
            {
                await Shell.Current.GoToAsync($"./{nameof(ProfilePage)}");
            }
            else
            {
                await Shell.Current.GoToAsync($"./{nameof(ProfilePage)}?userId={post.UserId}");
            }
        }

        private async Task GetsPosts()
        {
            if (IsBusy) return; // prevents the retrieving of data fetching if its already in progress

            IsBusy = true; // to show the loading spinner

            try
            {
                var posts = await _communityService.GetAllCommunityPosts(); // fetches all of the posts

                var sortedPosts = posts
                    .OrderByDescending(p => p.CreatedDate) 
                    .ToList();

                CommunityPosts.Clear(); // this method updates the ui with the new posts, removing old data

                foreach (var post in sortedPosts)
                {
                    var comments = await _communityService.GetCommentsByPost(post.CommunityPostId); // gets comments for each post

                    post.Comments = comments;  // puts the retrieved comments to the commetns section

                    CommunityPosts.Add(post); // adds new posts
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load community posts: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task AddComment(CommunityPost post) // creates a comment on a post
        {
            try
            {
                string username = Preferences.Get("Username", "Unknown"); // gets user preferences so the the comments can be linked to a user

                bool result = await _communityService.CreateComment(post.CommunityPostId, username, Comment); // calls the method from the service to send to api with the inputs from the user

                if (result) // checks if the comment was added successfully
                {
                    await GetsPosts(); // at the moment just focusing on recieving the comments as in other social media apps a message isnt granted to show its been added
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to add new comment.", "OK"); // exception message if comment was not added
                }
            }
            catch (Exception ex) // exception
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Exception: {ex.Message}", "OK"); // basic exception message
            }
        }

        private async Task LikePost(CommunityPost post)
        {
            try
            {
                int userId = Preferences.Get("UserId", 0); // gets user preferences of the logged in user

                bool result = await _communityService.LikePost(post.CommunityPostId, userId); // this calls the service to send data to backend

                if (result)
                {
                    await GetsPosts(); // this forces a refresh of getting posts to see if the posts were liked correctly
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Exception: {ex.Message}", "OK"); 
            }
        }

        private async Task AddExternalWorkoutPlan(CommunityPost post)
        {
            try
            {
                if (post.WorkoutPlanId == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Unavailable!", "This post unfortunately does not have a workout plan.", "OK");  // this if statement displays an unavailable message to the usr if no workoutplan exists
                    return;
                }

                int userId = Preferences.Get("UserId", 0); // gets user preferences

                var response = await _communityService.AddExternalUserWorkoutPlan((int)post.WorkoutPlanId, userId); // call method to add workoutplan for user

                if (response)
                {
                    await Application.Current.MainPage.DisplayAlert("Success!", "Workout plan successfully added to your account.", "Nice!"); // success messsage if added correctly
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Oops!", "Something went wrong while adding the workout plan!", "OK"); // error message
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error!", $"Error adding the workout plan: {ex.Message}", "OK");
            }


        }

    }
}
