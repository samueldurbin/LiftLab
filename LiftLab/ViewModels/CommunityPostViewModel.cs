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
        private readonly WorkoutPlansServiceUI _workoutPlansService;
        private readonly FriendServiceUI _friendsService;
        #region ICommands - OnClickLisenters

        public ICommand ViewUserProfileCommand { get; }
        public ICommand GetCommunityPostsCommand { get; }  // gets and displays posts
        public ICommand AddPostCommand { get; }
        public ICommand CreateCommentCommand { get; }
        public ICommand LoadCommunityPostsCommand { get; }
        public ICommand LikePostCommand { get; }
        public ICommand AddPlansToUserAccountCommand { get; }
        public ICommand AddFriendsCommand { get; }
        public ICommand AddWorkoutPlanCommand { get; }
        public ICommand ShowPlanDetailsCommand { get; }

        public ICommand ToggleCommentBoxCommand { get; }

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
            _communityService = new CommunityPostServiceUI();
            _workoutPlansService = new WorkoutPlansServiceUI();
            _friendsService = new FriendServiceUI();

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

            LikePostCommand = new Command<CommunityPost>(async (post) => await LikePost(post));

            AddWorkoutPlanCommand = new Command<CommunityPost>(async (post) => await AddExternalWorkoutPlan(post));

            ViewUserProfileCommand = new Command<CommunityPost>(async (post) => await ViewUserProfile(post));

            AddPlansToUserAccountCommand = new Command<CommunityPost>(async (post) => await AddPlanToAccount(post));

            ShowPlanDetailsCommand = new Command<CommunityPost>(async (post) => await ShowPlanDetails(post));

            ToggleCommentBoxCommand = new Command<CommunityPost>((post) =>
            {
                if (post != null)
                {
                    post.ShowCommentBox = !post.ShowCommentBox;
                    
                }
            });

        }

        private async Task ShowPlanDetails(CommunityPost post)
        {
            try
            {
                var nutritionService = new NutritionServiceUI(); // new service instance for meals + mealplans

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
                    var mealPlans = await nutritionService.GetMealPlansByUser(userId);
                    var mealPlan = mealPlans.FirstOrDefault(mp => mp.MealPlanId == post.MealPlanId);

                    if (mealPlan != null)
                    {
                        var mealsInPlan = await nutritionService.GetMealsByPlanId(mealPlan.MealPlanId);
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
                    var userMeals = await nutritionService.GetMealsByUser(userId);
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
            if (IsBusy) return;

            IsBusy = true;

            try
            {
                int userId = Preferences.Get("UserId", 0);

                var posts = await _friendsService.GetFriendsPosts(userId);

                var sortedPosts = posts
                    .OrderByDescending(p => p.CreatedDate)
                    .ToList();

                CommunityPosts.Clear();

                foreach (var post in sortedPosts)
                {
                    var comments = await _communityService.GetCommentsByPost(post.CommunityPostId);
                    post.Comments = new ObservableCollection<CommunityPostComments>(comments);

                    CommunityPosts.Add(post);
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
        //private async Task GetsPosts()
        //{
        //    if (IsBusy) return; // prevents the retrieving of data fetching if its already in progress

        //    IsBusy = true; // to show the loading spinner

        //    try
        //    {
        //        var posts = await _communityService.GetAllCommunityPosts(); // fetches all of the posts

        //        var sortedPosts = posts
        //            .OrderByDescending(p => p.CreatedDate) 
        //            .ToList();

        //        CommunityPosts.Clear(); // this method updates the ui with the new posts, removing old data

        //        foreach (var post in sortedPosts)
        //        {
        //            var comments = await _communityService.GetCommentsByPost(post.CommunityPostId); // gets comments for each post

        //            post.Comments = new ObservableCollection<CommunityPostComments>(comments);

        //            CommunityPosts.Add(post); // adds new posts
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load community posts: {ex.Message}", "OK");
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //    }
        //}

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

        private async Task LikePost(CommunityPost post)
        {
            try
            {
                int userId = Preferences.Get("UserId", 0); // gets user preferences of the logged in user

                bool result = await _communityService.LikePost(post.CommunityPostId, userId); // this calls the service to send data to backend

                if (result)
                {
                    post.LikeCount += 1; 
              
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
        private async Task AddPlanToAccount(CommunityPost post)
        {
            try
            {
                int userId = Preferences.Get("UserId", 0);
                bool added = await _communityService.AddExternalPlans(post, userId);

                if (added)
                    await Application.Current.MainPage.DisplayAlert("Success", "Item added to your account!", "OK");
                else
                    await Application.Current.MainPage.DisplayAlert("Unavailable", "No plan or meal attached.", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
