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
    public class ProfileViewModel : BaseViewModel
    {
        private readonly CommunityPostServiceUI _communityService;

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

        public ICommand SettingsCommand { get; }
        public ICommand DeletePostCommand { get; }


        public ProfileViewModel()
        {
            _communityService = new CommunityPostServiceUI();

            Username = "@" + Preferences.Get("Username", "Unknown"); // this gets the username of the user thats logged in and binds it to the ui

            SettingsCommand = new Command(async () => // add button in the ui navigates to create a post
            {
                //await Shell.Current.GoToAsync(nameof(SettingsPage));
            });

            DeletePostCommand = new Command<int>(async (postId) => await DeletePostAsync(postId));
        }

        private async Task DeletePostAsync(int postId)
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

        public async Task LoadUserProfile(int userId)
        {
            IsBusy = true;

            try
            {
                var user = await _communityService.GetUserById(userId);
                Username = "@" + user?.Username ?? "Unknown";

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
    }
}
