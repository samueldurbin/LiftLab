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
using Shared.Utilities;

namespace LiftLab.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public ICommand AccountCommand { get; }
        public ICommand NotificationCommand { get; }
        public ICommand LogOutCommand { get; }
        public ICommand UpdateSettingsCommand { get; }

        public ICommand SaveCommand { get; }

        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }

        private readonly UsersServiceUI _usersService = new();

        private Users _user;

        public string Username { get => _user?.Username; set { _user.Username = value; OnPropertyChanged(); } }
        public string Password { get => _user?.Password; set { _user.Password = value; OnPropertyChanged(); } }
        public string Email { get => _user?.Email; set { _user.Email = value; OnPropertyChanged(); } }
        public string PhoneNumber { get => _user?.MobileNumber; set { _user.MobileNumber = value; OnPropertyChanged(); } }
        public DateTime DateOfBirth { get => _user?.DateOfBirth ?? DateTime.Today; set { _user.DateOfBirth = value; OnPropertyChanged(); } }

        public SettingsViewModel()
        {
            LogOutCommand = new Command(async () => await Logout());
            SaveCommand = new Command(async () => await UpdateUserData());

            UpdateSettingsCommand = new Command(async () => // add button in the ui navigates to create a post
            {
                await Shell.Current.GoToAsync(nameof(UpdateUserSettingsPage));
            });

            LoadUserData();
        }

        private async void LoadUserData()
        {
            var userId = Preferences.Get("UserId", 0); // gets the logged in userId for preferences

            _user = await _usersService.GetUserById(userId); // gets all the data about the user and populates the fields with their data

            OnPropertyChanged(nameof(Username));
            //OnPropertyChanged(nameof(Password));
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(PhoneNumber));
            OnPropertyChanged(nameof(DateOfBirth));
        }

        private async Task UpdateUserData()
        {

            if (string.IsNullOrWhiteSpace(CurrentPassword)) // checks whether the field is empty
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter your current password", "OK");
                return;
            }

            var loggedUser = await _usersService.Login(_user.Username, CurrentPassword); // this checks if the current password matches whats in the database. Reuses the login method as essentially does the same required authentication process

            if (loggedUser == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Current password is incorrect", "OK"); // if the entered password does not match the input, throw an error
                return;
            }

            if (!string.IsNullOrWhiteSpace(NewPassword)) // checks if the new password field is empty
            {
                _user.Password = NewPassword;
            }

            var updatedUserInfo = await _usersService.UpdateUser(_user); // updates the users data and saves changes to the database

            if (updatedUserInfo != null) // does a check whether its a success or not
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Account updated successfully!", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to update account! Please try again", "OK");
            }
        }

        private async Task Logout() // log out method
        {
            Preferences.Remove("UserId"); // removes the logged in preferences
            Preferences.Remove("Username"); // removes the logged in preferences
            await Application.Current.MainPage.DisplayAlert("GoodBye!", "You have been logged out of LiftLab", "OK");

            Application.Current.MainPage = new NavigationPage(new LoginPage()); // redirects to the login page
        }

    }
    
}
