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
    public class CommunityViewModel : BaseViewModel
    {
        private readonly FitnessPostServiceUI _fitnessPostService;  // gets fitness posts from api

        public ICommand GetFitnessPostsCommand { get; }  // gets and displays fitnessposts 
        public ICommand NavigationCommand { get; }
        public ICommand CreateCommentCommand { get; }

        public ObservableCollection<FitnessPost> FitnessPosts { get; set; } // collection of posts objects

        private string username;
        private string comment;
        private int fitnessPostId;

        public string Username
        {
            get => username;
            set => SetProperty(ref username, value); // checks if ui has updated and adds input
        }

        public int FitnessPostId
        {
            get => fitnessPostId;
            set => SetProperty(ref fitnessPostId, value);
        }

        public string Comment
        {
            get => comment;
            set => SetProperty(ref comment, value);
        }

        public CommunityViewModel()
        {
            _fitnessPostService = new FitnessPostServiceUI();

            FitnessPosts = new ObservableCollection<FitnessPost>(); // initializes empty to store fitness posts

            GetFitnessPostsCommand = new Command(async () => await GetsPosts());

            CreateCommentCommand = new Command<FitnessPost>(async (post) => await CreateComment(post)); // assigns the function to a on vlick button in the ui
        
            NavigationCommand = new Command(async () => // add button in the ui navigates to create a post
            {
                await Shell.Current.GoToAsync(nameof(CreatePost));
            });
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

        private async Task CreateComment(FitnessPost post)
        {
            try
            {
                var newComment = await _fitnessPostService.AddComment(Username, Comment, post.FitnessPostId); // gets method from serviceui to add comment

                if (newComment != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Success!", "Your comment has been created successfully!", "OK"); // message to inform the users the comment was added successfully
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Comment has not been added.", "OK"); // error message if the new comment is null
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to add comment: {ex.Message}", "OK"); // secomd error message for adding comments function
            }
        }

    }
    
}
