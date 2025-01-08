using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiftLab.Services;
using LiftLab.Models;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Text.Json;

namespace LiftLab.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly ApiUserService _apiUserService;

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(); // Notify the UI of changes
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(); // Notify the UI of changes
            }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            _apiUserService = new ApiUserService();
            LoginCommand = new Command(async () => await LoginAsync());
        }

        private async Task LoginAsync()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please enter both username and password.", "OK");
                return;
            }

            var user = await _apiUserService.LoginAsync(Username, Password);

            if (user != null)
            {
                // Successful login
                await Application.Current.MainPage.DisplayAlert("Success", $"Welcome, {user.Username}!", "OK");
            }
            else
            {
                // Login failed
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid username or password.", "OK");
            }
        }
    }
}
