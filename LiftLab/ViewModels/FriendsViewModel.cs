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
        private readonly FriendServiceUI _friendService;
        private readonly UsersServiceUI _userService;

        public ObservableCollection<Users> Users { get; set; } // filtered list

        private List<Users> AllUsers { get; set; } // unfiltered lsit

        public ICommand AddFriendCommand { get; } // add friend button

        private string currentUsername; // for preferences

        public FriendsViewModel()
        {
            _friendService = new FriendServiceUI();
            _userService = new UsersServiceUI();

            Users = new ObservableCollection<Users>();

            AllUsers = new List<Users>();

            AddFriendCommand = new Command<int>(async (friendId) => await AddFriend(friendId)); // adds a friend by userid

            currentUsername = Preferences.Get("Username", "Unknown"); // gests the current logged in user

            LoadUsers();
        }

        private async void LoadUsers()
        {
            if (IsBusy) // prevents multiple calls from happening
            {
                return;
            }

            IsBusy = true;

            try
            {
                var fetchedUsers = await _userService.GetAllUsers(); // gets all users from api

                AllUsers = fetchedUsers // this prevents the logged in use from adding themselves
                    .Where(u => !string.Equals(u.Username, currentUsername, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                SearchFilter(); // shows the filtered users
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error!", $"Failed to load the users: {ex.Message}", "Please Try Again");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task AddFriend(int friendUserId)
        {
            try
            {
                var success = await _friendService.AddFriend(GetCurrentUserId(), friendUserId);

                if (success)
                {
                    await Application.Current.MainPage.DisplayAlert("Success!", "User has been added as a friend!", "OK!");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error!", "Friend request failed or it already exists.", "OK!");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error!", $"Failed to add new friend: {ex.Message}", "OK!");
            }
        }

        private int GetCurrentUserId()
        {
            return Preferences.Get("UserId", 0); // gets the current logged in userid
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                    SearchFilter(); // refilter the lsit based on searchbar entry
                }
            }
        }

        private void SearchFilter()
        {
            var filteredUsers = string.IsNullOrWhiteSpace(SearchText) // if search is empty, show all users,if not then show by uersname
                ? AllUsers // quick value checks, essentially if searchtext  is null, return all users else return the new filtered list of users
                : AllUsers.Where(u => u.Username.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList(); // compares the strings and treats both upper and lower cases as the same search characters

            Users.Clear();

            foreach (var user in filteredUsers) // add filtered users to the list
            {
                Users.Add(user);
            }
        }
    }
}
