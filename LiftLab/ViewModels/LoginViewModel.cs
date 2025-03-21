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

namespace LiftLab.ViewModels
{
    public class LoginViewModel : BaseViewModel // as a viewmodel, it connects the ui with the logic
    {
        #region Variables
        private readonly UserServiceUI _apiUserService;

        private string username;
        private string password;

        #endregion

        #region Actions
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

        #endregion

        #region Login
        public ICommand LoginCommand { get; } // for the button to login in the UI

        public LoginViewModel()
        {
            _apiUserService = new UserServiceUI();
            LoginCommand = new Command(async () => await Login());  // links the button to the method
        }

        private async Task Login()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))  // displays an error if nothing is entered into the fields
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter both username and password.", "OK");
                return;
            }

            var hashInputPassword = Hashing.Hash(Password);

            var user = await _apiUserService.Login(Username, hashInputPassword); // calls this method to authenticate the user

            if (user != null)
            {
                await Application.Current.MainPage.DisplayAlert("Success", $"Welcome, {user.Username}!", "OK"); // successful login

                Application.Current.MainPage = new NavigationPage(new CreateWorkoutPage());
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid username or password.", "OK");  
            }
        }

        #endregion
    }
}
