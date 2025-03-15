using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LiftLab.Services;

namespace LiftLab.ViewModels
{
    public class CreateNutritionPostViewModel : BaseViewModel
    {
        private readonly NutritionPostServiceUI _nutritionPostService;

        private string username;
        private string imageUrl;
        private string caption;

        public ICommand CreatePostCommand { get; } // creates post onclick

        public CreateNutritionPostViewModel()
        {
            _nutritionPostService = new NutritionPostServiceUI(); // creates instance

            CreatePostCommand = new Command(async () => await CreatePost()); // button on click
        }

        public string Username // gets ui data and sets them into the variables
        {
            get => username;
            set => SetProperty(ref username, value);
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

        private async Task CreatePost()
        {
            var newPost = await _nutritionPostService.CreatePost(Username, ImageUrl, Caption); // creates post with the parameters

            if (newPost != null)
            {
                await Application.Current.MainPage.DisplayAlert("Nice!", "Your post has been created successfully!", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "FitnessPost creation has failed.", "OK");
            }
        }

    }
}
