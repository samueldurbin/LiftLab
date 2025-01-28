using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LiftLab.Services;
using Shared.Models;

namespace LiftLab.ViewModels
{
    public class CreatePostViewModel : BaseViewModel
    {
        private readonly FitnessPostServiceUI _apiPostService;

        private string username;
        private string imageUrl;
        private string caption;
        private DateTime createdDate;

        public string Username // property bound to username field in ui
        {
            get => username;
            set => SetProperty(ref username, value); // updates ui
        }

        public string ImageUrl
        {
            get => imageUrl;
            set => SetProperty(ref imageUrl, value);
        }

        public string Caption
        {
            get => caption;
            set => SetProperty(ref caption, value);
        }

        public DateTime CreatedDate
        {


            get => createdDate;
            set => SetProperty(ref createdDate, value);
        }

        public ICommand CreatePostCommand => new Command(async () => await CreatePostAsync()); // user actions part of the MVVM architecture


        private async Task CreatePostAsync() // calls api to create a new account
        {
            var apiService = new FitnessPostServiceUI();

            var newPost = await apiService.CreatePostAsync(Username, ImageUrl, Caption, CreatedDate);

            if (newPost != null)
            {
                await Application.Current.MainPage.DisplayAlert("Success", $"You have successfully created your account, !", "Lets go!"); // success message after account creation

            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Account Creation Error.", "Ok"); // displays a warning alert if account activation fails

            }
        }


    }
}

