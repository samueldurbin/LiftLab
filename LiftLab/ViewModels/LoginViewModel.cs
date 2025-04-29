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
using Shared.Models;
using CommunityToolkit.Maui.Views;

namespace LiftLab.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly UsersServiceUI _usersService;

        private string username; // variables for user input
        private string password;

        public ICommand ShowTandCCommand { get; }
        public ICommand LoginCommand { get; } // this will be used as the onclick button for navigation

        public string Username
        {
            get => username;  // receives whats typed into the ui
            set
            {
                username = value.Trim(); // trim prevents the additional whitespace for usernames
                OnPropertyChanged(); // notifies when property changes
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = value.Trim(); // trim prevents the additional whitespace for passwords
                OnPropertyChanged(); // notifies when property changes
            }
        }

        public LoginViewModel()
        {
            _usersService = new UsersServiceUI();
            ShowTandCCommand = new Command(async () => await ShowTandC());
            LoginCommand = new Command(async () => await Login());  // links the button to the function
        }

        private async Task Login() // login function
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))  // this displays an error if the fields are empty
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter both a username and password.", "OK"); // error message for checking whether an entry was empty
                return;
            }

            var loggedUser = await _usersService.Login(Username, Password); // calls this method to authenticate the user details

            if (loggedUser != null) // if both fields are empty and match the credentials in a database, set the preferences and redirect to homepage
            {
                Preferences.Set("UserId", loggedUser.UserId); // this sets the preferences of the application to the userid that has signed in
                Preferences.Set("Username", loggedUser.Username); // this sets the preferences of the application to the username that has signed in, this will be changed in the future

                Application.Current.MainPage = new AppShell(); // logging in initiates AppShell

            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid username or password, Please try again", "OK");  // error message for login fail
            }
        }

        private async Task ShowTandC()
        {
            try
            {
                var popup = new ViewTandC();
                await Application.Current.MainPage.ShowPopupAsync(popup);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Could not load Terms and Conditions: {ex.Message}", "OK");
            }
        }



    }
}
