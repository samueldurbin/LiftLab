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
    public class CommunityPostViewModel : BaseViewModel
    {
        private readonly CommunityServiceUI _communityService;  // gets community posts from api

        private string comment;

        public ICommand ViewUserProfileCommand { get; }
        public ICommand GetCommunityPostsCommand { get; }  // gets and displays posts
        public ICommand NavigationCommand { get; }
        public ICommand CreateCommentCommand { get; }
        public ICommand LoadCommunityPostsCommand { get; }
        public ICommand LikePostCommand { get; }

        public ICommand AddFriendsCommand { get; }
        public ICommand AddWorkoutPlanCommand { get; }

        public ObservableCollection<CommunityPost> CommunityPosts { get; set; } // collection of posts objects

        public string Comment // this is where the input of the users comment will be taken
        {
            get => comment;
            set => SetProperty(ref comment, value);
        }

        public CommunityPostViewModel()
        {
            _communityService = new CommunityServiceUI();

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

        }

        private async Task ViewUserProfile(CommunityPost post) // viewing user profiles
        {
            if (post == null || post.UserId <= 0)
            {
                return;
            }

            var profilePage = new PublicProfilePage(post.UserId); // pass userId into constructor

            await Shell.Current.Navigation.PushAsync(profilePage); // page navigation
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
