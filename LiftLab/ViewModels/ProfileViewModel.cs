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
using CommunityToolkit.Maui.Views;

namespace LiftLab.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        private readonly CommunityPostServiceUI _communityService;
        private readonly WorkoutPlansServiceUI _workoutPlansService;
        private readonly NutritionServiceUI _nutritionService;

        private string username;
        public string Username 
        {
            get => username;
            set => SetProperty(ref username, value);
        }

        private ObservableCollection<CommunityPost> userPosts;
        public ObservableCollection<CommunityPost> UserPosts
        {
            get => userPosts;
            set => SetProperty(ref userPosts, value);
        }

        private bool userProfile;
        public bool UserProfile
        {
            get => userProfile;
            set => SetProperty(ref userProfile, value);
        }

        public ICommand UpdateUserInfoCommand { get; }

        public ICommand DeletePostCommand { get; }
        public ICommand ShowPlanDetailsCommand { get; }
        public ICommand CreateCommentCommand { get; }

        public ICommand LogOutCommand { get; }
        public ICommand AddPlansToUserAccountCommand { get; }


        public ProfileViewModel()
        {
            _communityService = new CommunityPostServiceUI();
            _workoutPlansService = new WorkoutPlansServiceUI();
            _nutritionService = new NutritionServiceUI();

            Username = "@" + Preferences.Get("Username", "Unknown"); // this gets the username of the user thats logged in and binds it to the ui

            LogOutCommand = new Command(async () => await Logout());

            CreateCommentCommand = new Command<CommunityPost>(async (post) => await AddComment(post));

            UpdateUserInfoCommand = new Command(async () => // add button in the ui navigates to create a post
            {
                await Shell.Current.GoToAsync(nameof(UpdateUserSettingsPage));
            });

            DeletePostCommand = new Command<int>(async (postId) => await DeletePost(postId));

            AddPlansToUserAccountCommand = new Command<CommunityPost>(async (post) => await AddPlanToAccount(post));

            ShowPlanDetailsCommand = new Command<CommunityPost>(async (post) => await ShowPlanDetails(post));
        }

        private async Task AddPlanToAccount(CommunityPost post) // same method from communitypostvm
        {
            try
            {
                int userId = Preferences.Get("UserId", 0);
                bool added = await _communityService.AddExternalPlans(post, userId);

                if (added)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Item added to your account!", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Unavailable", "No plan or meal attached.", "OK");
                }
                    
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task DeletePost(int postId)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirm", "Are you sure you want to delete this post?", "Yes", "No"); // message to make sure the user knows the delete action

            if (!confirm)
            {
                return;
            }

            IsBusy = true;

            try
            {
                var userId = Preferences.Get("UserId", 0);

                bool success = await _communityService.DeleteCommunityPost(postId, userId);

                if (success)
                {
                    await Application.Current.MainPage.DisplayAlert("Deleted", "Post deleted successfully!", "OK");

                    await LoadUserProfile(userId); // refresh user posts
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Unable to delete post. Please try again", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Something went wrong: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }


        private async Task ShowPlanDetails(CommunityPost post) // same method thats in communitypostvm
        {
            try
            {
                if (post.WorkoutPlanId != null)
                {
                    var userId = post.UserId;
                    var userPlans = await _communityService.GetWorkoutPlansByUserId(userId);
                    var thisPlan = userPlans.FirstOrDefault(p => p.WorkoutPlanId == post.WorkoutPlanId);

                    if (thisPlan != null)
                    {
                        var workoutIds = await _workoutPlansService.GetWorkoutsByPlanIdForPopup(thisPlan.WorkoutPlanId);
                        var allWorkouts = await _workoutPlansService.GetAllWorkouts();

                        var workoutNames = allWorkouts
                            .Where(w => workoutIds.Select(d => d.WorkoutId).Contains(w.WorkoutId))
                            .Select(w => w.WorkoutName)
                            .ToList();

                        var popup = new ViewAddedPlan(thisPlan.WorkoutPlanName ?? "Workout Plan", workoutNames, string.Empty);
                        await Shell.Current.CurrentPage.ShowPopupAsync(popup);
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Not Found!", "Workout plan could not be found.", "OK");
                    }
                }
                else if (post.MealPlanId != null)
                {
                    var userId = post.UserId;
                    var mealPlans = await _nutritionService.GetMealPlansByUser(userId);
                    var mealPlan = mealPlans.FirstOrDefault(mp => mp.MealPlanId == post.MealPlanId);

                    if (mealPlan != null)
                    {
                        var mealsInPlan = await _nutritionService.GetMealsByPlanId(mealPlan.MealPlanId);
                        var mealNames = mealsInPlan.Select(m => m.MealName).ToList();

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
                    var userMeals = await _nutritionService.GetMealsByUser(userId);
                    var meal = userMeals.FirstOrDefault(m => m.MealId == post.MealId);

                    if (meal != null)
                    {
                        var popup = new ViewAddedPlan(meal.MealName ?? "Meal", new List<string>(), meal.Recipe ?? "No recipe available.");
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

        private async Task Logout() // log out method
        {
            Preferences.Remove("UserId"); // removes the logged in preferences
            Preferences.Remove("Username"); // removes the logged in preferences
            await Application.Current.MainPage.DisplayAlert("GoodBye!", "You have been logged out of LiftLab", "OK");

            Application.Current.MainPage = new NavigationPage(new LoginPage()); // redirects to the login page
        }

        public async Task LoadUserProfile(int userId)
        {
            IsBusy = true;

            try
            {
                int loggedInUserId = Preferences.Get("UserId", 0);  // get current user

                var user = await _communityService.GetUserById(userId);
                Username = "@" + user?.Username ?? "Unknown"; // adds an @ to the username

                UserProfile = (loggedInUserId == userId); 

                var posts = await _communityService.GetCommunityPostsByUserId(userId);
                UserPosts = new ObservableCollection<CommunityPost>(posts);
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

        private async Task AddComment(CommunityPost post)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(post.CommentText))
                {
                    await Application.Current.MainPage.DisplayAlert("Empty Comment", "Please enter a comment before sending.", "OK");
                    return;
                }

                string username = Preferences.Get("Username", "Unknown");

                bool result = await _communityService.CreateComment(post.CommunityPostId, username, post.CommentText);

                if (result)
                {
                    post.Comments.Add(new CommunityPostComments
                    {
                        Username = username,
                        Comment = post.CommentText
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

    }
}
