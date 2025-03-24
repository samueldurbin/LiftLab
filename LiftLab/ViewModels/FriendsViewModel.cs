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
    public class FriendsViewModel : BaseViewModel
    {
        private readonly FriendServiceUI _friendService; // services instances
        private readonly UsersServiceUI _userService;

        public ObservableCollection<Users> Users { get; set; } // list of users
        public ICommand AddFriendCommand { get; } // adds friend / button to add 

        private int currentUserId = 9; // hardcoded logged in user for the moment

        public FriendsViewModel()
        {
            _friendService = new FriendServiceUI();
            _userService = new UsersServiceUI();

            Users = new ObservableCollection<Users>();
            AddFriendCommand = new Command<int>(async (friendId) => await AddFriend(friendId)); // adds method to add friend to the button

            LoadUsers(); // loads users once page is loaded
        }

        // loads list of all users
        private async void LoadUsers()
        {
            if (IsBusy) return; // prevents doubled ui
            IsBusy = true; // shows loading

            try
            {
                var allUsers = await _userService.GetAllUsers(); // gets a list of all users from the api in services
                Users.Clear(); // clears existing data before fetching

                foreach (var user in allUsers)
                {
                    if (user.UserId != currentUserId) // logged in user cant be added
                    {
                        Users.Add(user); // adds user to the ui
                    }
                }
            }
            catch (Exception ex) // fail from api call
            {
                await Application.Current.MainPage.DisplayAlert("Error!", $"Failed to load users: {ex.Message}", "Please Try Again");
            }
            finally
            {
                IsBusy = false; // hides loading
            }
        }

        private async Task AddFriend(int friendUserId)
        {
            try
            {
                var success = await _friendService.AddFriend(currentUserId, friendUserId); // add friends method from service

                if (success)
                {
                    await Application.Current.MainPage.DisplayAlert("Success!", "User has been added as a friend!", "OK!");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error!", "Friend request failed or it already exists.", "OK!");
                }
            }
            catch (Exception ex) // error with adding friend
            {
                await Application.Current.MainPage.DisplayAlert("Error!", $"Failed to add new friend: {ex.Message}", "OK!");
            }
        }
    }
}
