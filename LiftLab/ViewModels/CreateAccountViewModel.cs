using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiftLab.Services;
using System.Windows.Input;
using LiftLab.Views;

namespace LiftLab.ViewModels
{
    public class CreateAccountViewModel : BaseViewModel
    {
        #region Variables
        private string username; // stores data entered by user
        private string password;
        private string email;
        private string phoneNumber;
        private DateTime dateOfBirth;
        #endregion

        #region Properties

        public string Username // property bound to username field in ui
        {
            get => username;
            set => SetProperty(ref username, value); // updates ui
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string PhoneNumber
        {
            get => phoneNumber;
            set => SetProperty(ref phoneNumber, value);
        }

        public DateTime DateOfBirth
        {
            get => dateOfBirth;
            set => SetProperty(ref dateOfBirth, value);
        }

        #endregion

        #region Actions
        public ICommand CreateAccountCommand => new Command(async () => await CreateAccount()); // user actions part of the MVVM architecture
        #endregion

        #region Create Account
        private async Task CreateAccount() // calls api to create a new account
        {
            var userService = new UsersServiceUI();

            var newUser = await userService.CreateAccount(Username, Password, Email, PhoneNumber, DateOfBirth);

            if (newUser != null)
            {
                await Application.Current.MainPage.DisplayAlert("Welcome to LiftLab!","Enjoy what we have to offer, but be respectful towards others :)", "OK"); // success message after account creation
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid username or password.", "OK"); // displays a warning alert if account activation fails

            }
        }

        #endregion
    }
}
