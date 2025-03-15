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
    public class NutritionViewModel : BaseViewModel
    {
        private readonly NutritionPostServiceUI _nutritionPostService;  // gets fitness posts from api

        public ICommand GetNutritionPostsCommand { get; }  // gets and displays fitnessposts 
        public ICommand NavigationCommand { get; }
        public ICommand CreateCommentCommand { get; }
        public ObservableCollection<NutritionPost> NutritionPosts { get; set; } // collection of posts objects

        private string username;
        private string comment;
        private int nutritionPostId;

        public string Username
        {
            get => username;
            set => SetProperty(ref username, value); // checks if ui has updated and adds input
        }

        public int NutritionPostId
        {
            get => nutritionPostId;
            set => SetProperty(ref nutritionPostId, value);
        }

        public string Comment
        {
            get => comment;
            set => SetProperty(ref comment, value);
        }

        public NutritionViewModel()
        {
            _nutritionPostService = new NutritionPostServiceUI();

            NutritionPosts = new ObservableCollection<NutritionPost>(); // initializes empty to store fitness posts

            GetNutritionPostsCommand = new Command(async () => await GetsPosts());

            CreateCommentCommand = new Command<NutritionPost>(async (post) => await CreateComment(post)); // assigns the function to a on vlick button in the ui

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
                var posts = await _nutritionPostService.GetAllNutritionPosts(); // fetches all of the posts

                NutritionPosts.Clear(); // this method updates the ui with the new posts, removing old data

                foreach (var post in posts)
                {
                    var comments = await _nutritionPostService.GetCommentsByPost(post.NutritionPostId); // gets comments for each post

                    post.Comments = comments;
                    NutritionPosts.Add(post); // adds new posts
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

        private async Task CreateComment(NutritionPost post)
        {
            try
            {
                var newComment = await _nutritionPostService.AddComment(Username, Comment, post.NutritionPostId); // gets method from serviceui to add comment

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
