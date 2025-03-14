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
        public ObservableCollection<FitnessPost> FitnessPosts { get; set; } // collection of posts objects

        public CommunityViewModel()
        {
            _fitnessPostService = new FitnessPostServiceUI();

            FitnessPosts = new ObservableCollection<FitnessPost>(); // initializes empty to store fitness posts

            GetFitnessPostsCommand = new Command(async () => await GetsPosts());

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

    }
    
}
