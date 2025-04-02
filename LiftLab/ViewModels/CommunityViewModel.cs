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
    public class CommunityViewModel : BaseViewModel
    {
        private readonly FitnessPostServiceUI _fitnessPostService;  // gets fitness posts from api

        private string comment;

        public ICommand GetFitnessPostsCommand { get; }  // gets and displays fitnessposts 
        public ICommand NavigationCommand { get; }
        public ICommand CreateCommentCommand { get; }
        public ICommand LoadFitnessPostsCommand { get; }
        public ICommand LikePostCommand { get; }
        public ObservableCollection<FitnessPost> FitnessPosts { get; set; } // collection of posts objects

        public string Comment // this is where the input of the users comment will be taken
        {
            get => comment;
            set => SetProperty(ref comment, value);
        }

        public CommunityViewModel()
        {
            _fitnessPostService = new FitnessPostServiceUI();

            FitnessPosts = new ObservableCollection<FitnessPost>(); // initializes empty to store fitness posts

            GetFitnessPostsCommand = new Command(async () => await GetsPosts());

            CreateCommentCommand = new Command<FitnessPost>(async (post) => await AddComment(post)); // assigns the function to a on vlick button in the ui
        
            NavigationCommand = new Command(async () => // add button in the ui navigates to create a post
            {
                await Shell.Current.GoToAsync(nameof(CreatePost));
            });

            LoadFitnessPostsCommand = new Command(async () => await GetsPosts()); // this will be used to load the workout plans on load

            LikePostCommand = new Command<FitnessPost>(async (post) => await LikePost(post));

        }

        private async Task GetsPosts()
        {
            if (IsBusy) // prevents the retrieving of data fetching if its already in progress
                return; 

            IsBusy = true; // to show the loading spinner

            try
            {
                var posts = await _fitnessPostService.GetAllFitnessPosts(); // fetches all of the posts

                FitnessPosts.Clear(); // this method updates the ui with the new posts, removing old data

                foreach (var post in posts)
                {
                    var comments = await _fitnessPostService.GetCommentsByPost(post.FitnessPostId); // gets comments for each post

                    post.Comments = comments; // puts the retrieved comments to the commetns section

                    FitnessPosts.Add(post); // adds new posts
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error Exception", $"Fail to load the fitnessposts: {ex.Message}", "ok");
            }
            finally
            {
                IsBusy = false; 
            }

        }

        private async Task AddComment(FitnessPost post) // creates a comment on a post
        {
            try
            {
                string username = Preferences.Get("Username", "Unknown"); // gets user preferences so the the comments can be linked to a user

                bool result = await _fitnessPostService.CreateComment(post.FitnessPostId, username, Comment); // calls the method from the service to send to api with the inputs from the user

                if (result) // checks if the comment was added successfully
                {
                    await GetsPosts(); // at the moment just focusing on recieving the comments as in other social media apps a message isnt granted to show its been added
                    //await Application.Current.MainPage.DisplayAlert("Success!", "Your comment was added!", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error!", "Failed to add new comment.", "OK"); // exception message if comment was not added
                }
            }
            catch (Exception ex) // exception
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Exception: {ex.Message}", "OK");
            }
        }

        private async Task LikePost(FitnessPost post)
        {
            try
            {
                int userId = Preferences.Get("UserId", 0); // gets user preferences of the logged in user

                bool result = await _fitnessPostService.LikePost(post.FitnessPostId, userId); // this calls the service to send data to backend

                if (result)
                {
                    await GetsPosts(); // this forces a refresh of getting posts to see if the posts were liked correctly
                }
                // no exception message as users dont need to be informed if theyve already liked a post (the colour will change if so)
            }
            catch (Exception ex) // exception
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Exception: {ex.Message}", "OK"); // basic exception message
            }

        }

    }

}
