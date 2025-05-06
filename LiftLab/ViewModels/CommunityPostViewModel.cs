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
        private readonly CommunityPostServiceUI _communityService;  // gets community posts from api
        private readonly WorkoutPlansServiceUI _workoutPlansService; // services
        private readonly FriendServiceUI _friendsService;
        private readonly NutritionServiceUI _nutritionService;
        #region ICommands - OnClickLisenters

        public ICommand ViewUserProfileCommand { get; }
        public ICommand GetCommunityPostsCommand { get; }  // gets and displays posts
        public ICommand AddPostCommand { get; }
        public ICommand CreateCommentCommand { get; }
        public ICommand LoadCommunityPostsCommand { get; }
        public ICommand AddPlansToUserAccountCommand { get; }
        public ICommand AddFriendsCommand { get; }
        public ICommand AddWorkoutPlanCommand { get; }
        public ICommand ShowPlanDetailsCommand { get; }
        public ICommand ToggleCommentBoxCommand { get; } // shows and hides the comment box

        #endregion

        public ObservableCollection<CommunityPost> CommunityPosts { get; set; } // collection of posts to be displayed

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
            _communityService = new CommunityPostServiceUI();
            _workoutPlansService = new WorkoutPlansServiceUI();
            _friendsService = new FriendServiceUI();
            _nutritionService = new NutritionServiceUI();

            CommunityPosts = new ObservableCollection<CommunityPost>(); // initializes empty to store fitness posts

            GetCommunityPostsCommand = new Command(async () => await GetsPosts());

            CreateCommentCommand = new Command<CommunityPost>(async (post) => await AddComment(post)); // assigns the function to a on vlick button in the ui

            AddPostCommand = new Command(async () => // add button in the ui navigates to create a post
            {
                await Shell.Current.GoToAsync(nameof(CreatePost));
            });

            AddFriendsCommand = new Command(async () => // add button in the ui navigates to create a post
            {
                await Shell.Current.GoToAsync(nameof(FriendsPage));
            });

            LoadCommunityPostsCommand = new Command(async () => await GetsPosts()); // this will be used to load the workout plans on load

            ViewUserProfileCommand = new Command<CommunityPost>(async (post) => await ViewUserProfile(post));

            AddPlansToUserAccountCommand = new Command<CommunityPost>(async (post) => await AddPlanToAccount(post)); // this is the button that adds users plans to an account

            ShowPlanDetailsCommand = new Command<CommunityPost>(async (post) => await ShowPlanDetails(post));

            ToggleCommentBoxCommand = new Command<CommunityPost>((post) => // toggles visibility of the comment input
            {
                if (post != null)
                {
                    post.ShowCommentBox = !post.ShowCommentBox;
                    
                }
            });

        }

        private async Task ShowPlanDetails(CommunityPost post) // shows the popup with details of the shared workout or meal plan to see whats attached to the post before adding
        {
            try
            {
               
                if (post.WorkoutPlanId != null) // checks if the post has an attached workoutplan
                {
                    var userId = post.UserId; // get the userid of the person who posted it
                    var userPlans = await _communityService.GetWorkoutPlansByUserId(userId); // gets the workoutplans by the userid
                    var thisPlan = userPlans.FirstOrDefault(p => p.WorkoutPlanId == post.WorkoutPlanId); // match the plan from the post

                    if (thisPlan != null) // if a workoutplan was found
                    {
                        var workoutIds = await _workoutPlansService.GetWorkoutsByPlanIdForPopup(thisPlan.WorkoutPlanId); // get the list of workouts
                        var allWorkouts = await _workoutPlansService.GetAllWorkouts(); // get all workouts

                        var workoutNames = allWorkouts
                            .Where(w => workoutIds.Select(d => d.WorkoutId).Contains(w.WorkoutId)) // match the ids
                            .Select(w => w.WorkoutName) // get the names of the workouts
                            .ToList(); // into a list

                        var popup = new ViewAddedPlan(thisPlan.WorkoutPlanName ?? "Workout Plan", workoutNames, string.Empty); // creates a popup
                        await Shell.Current.CurrentPage.ShowPopupAsync(popup); // display the popup
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Not Found!", "Workout plan could not be found.", "OK"); // this is the error message if a plan is missing
                    }
                }
                else if (post.MealPlanId != null) // this checks if the post contains a meal plan instead
                {
                    var userId = post.UserId;
                    var mealPlans = await _nutritionService.GetMealPlansByUser(userId); // get user's meal plans
                    var mealPlan = mealPlans.FirstOrDefault(mp => mp.MealPlanId == post.MealPlanId); // find the specific one thats added to the post

                    if (mealPlan != null)
                    {
                        var mealsInPlan = await _nutritionService.GetMealsByPlanId(mealPlan.MealPlanId); // get meals in the plan
                        var mealNames = mealsInPlan.Select(m => m.MealName).ToList(); // get the meal names

                        var popup = new ViewAddedPlan(mealPlan.MealPlanName ?? "Meal Plan", mealNames, string.Empty); // creates the popup
                        await Shell.Current.CurrentPage.ShowPopupAsync(popup); // displys the popup
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Not Found", "Meal plan could not be found.", "OK"); // error message if a meal plan is missing
                    }
                }
                else if (post.MealId != null) // final check is checking if there is a meal attached
                {
                    var userId = post.UserId;
                    var userMeals = await _nutritionService.GetMealsByUser(userId); // get all the meals for that user
                    var meal = userMeals.FirstOrDefault(m => m.MealId == post.MealId); // find the one from the post

                    if (meal != null)
                    {
                        var popup = new ViewAddedPlan(meal.MealName ?? "Meal", new List<string>(), meal.Recipe ?? "No recipe available."); // creates popup
                        await Shell.Current.CurrentPage.ShowPopupAsync(popup); // displays the popup
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Not Found", "Meal could not be found.", "OK"); // if there is no meal attached
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Oops", "No plan or meal attached to this post.", "OK"); // error message if there is no plan attached
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Could not load plan details: {ex.Message}", "OK"); // this catches unexpected errors
            }
        }

        private async Task ViewUserProfile(CommunityPost post) // viewing user profiles
        {
            if (post == null)
            {
                return;
            }
            
            int currentUserId = Preferences.Get("UserId", 0); // gets the current user's id

            if (post.UserId == currentUserId)
            {
                await Shell.Current.GoToAsync($"./{nameof(ProfilePage)}"); // go to own profile 
            }
            else
            {
                await Shell.Current.GoToAsync($"./{nameof(ProfilePage)}?userId={post.UserId}"); // goes to another users profile
            }
        }

        private async Task GetsPosts()
        {
            if (IsBusy) return; // prevents entry while loading

            IsBusy = true;

            try
            {
                int userId = Preferences.Get("UserId", 0); // gets the current user's id to whoever is logged in

                var posts = await _friendsService.GetFriendsPosts(userId); // gets posts from the service

                var sortedPosts = posts
                    .OrderByDescending(p => p.CreatedDate) // sorts newest first
                    .ToList();

                CommunityPosts.Clear(); // clears existing posts

                foreach (var post in sortedPosts)
                {
                    var comments = await _communityService.GetCommentsByPost(post.CommunityPostId); // gets comments for the post
                    post.Comments = new ObservableCollection<CommunityPostComments>(comments); // attach comments to the post

                    CommunityPosts.Add(post); // add to the ui
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load community posts: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false; // resets the loading
            }
        }

        private async Task AddComment(CommunityPost post) // adds a comment to a specific post
        {
            try
            {
                if (string.IsNullOrWhiteSpace(post.CommentText)) // this prevents empty inputs
                {
                    await Application.Current.MainPage.DisplayAlert("Empty Comment", "Please enter a comment before sending.", "OK");
                    return;
                }

                string username = Preferences.Get("Username", "Unknown"); // gets the username of the user logged in

                bool comments = await _communityService.CreateComment(post.CommunityPostId, username, post.CommentText); // calls the service

                if (comments)
                {
                    post.Comments.Add(new CommunityPostComments
                    {
                        Username = username,
                        Comment = post.CommentText // add comment to the post
                    });

                    post.CommentText = string.Empty; // clears the input box for after a comment is added

                    post.ShowCommentBox = false; // hides the comments section
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to add new comment.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Exception: {ex.Message}", "OK");
            }
        }

        private async Task AddPlanToAccount(CommunityPost post) // copies a plan from the post to a user's account
        {
            try
            {
                int userId = Preferences.Get("UserId", 0); // gets the logged in user's id
                bool added = await _communityService.AddExternalPlans(post, userId);

                if (added)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "The attached plan/meal has been added to your account!", "OK"); // display message if the plan or meal has been added to the account

                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Unavailable", "No plan or meal attached.", "OK"); // error display message if there is no meal or plan attached

                }
                    
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
