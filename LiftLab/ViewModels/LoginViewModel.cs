using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiftLab.Services;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using LiftLab.Views;
using Shared.Utilities;
using System.Text.Json;
using Microsoft.Maui.Storage;

namespace LiftLab.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {

        private readonly UsersServiceUI _usersService;

        private string username; // variables for user input
        private string password;

        public ICommand LoginCommand { get; } // this will be used as the onclick button for navigation

        public string Username
        {
            get => username;  // recieves whats typed into the ui
            set
            {
                username = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }

        public LoginViewModel()
        {
            _usersService = new UsersServiceUI();

            LoginCommand = new Command(async () => await Login());  // links the button to the function
        }

        private async Task Login() // login function
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))  // this displays an error if the fields are empty
            {
                await Application.Current.MainPage.DisplayAlert("Error!", "Please enter both a username and password.", "OK");
                return;
            }

            var user = await _usersService.Login(Username, Password); // calls this method to authenticate the user details

            if (user != null) // if the user is not empty / successfully authenticated
            {
                Preferences.Set("UserId", user.UserId); // this sets the preferences of the application to the userid that has signed in
                Preferences.Set("Username", user.Username); // this sets the preferences of the application to the username that has signed in, this will be changed in the future

                await Application.Current.MainPage.DisplayAlert("Success!", $"Welcome, {user.Username}!", "OK"); // successful login message

                Application.Current.MainPage = new AppShell();

                //await Shell.Current.GoToAsync("//FitnessPage"); // this starts shell navigation that then shows the navigation bar
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error!", "Invalid username or password, Please try agayun", "OK");   // error message
            }
        }

    }
}
