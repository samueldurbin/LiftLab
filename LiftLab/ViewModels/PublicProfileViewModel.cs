using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiftLab.Services;
using Shared.Models;

namespace LiftLab.ViewModels
{
    public class PublicProfileViewModel : BaseViewModel
    {
        private readonly CommunityServiceUI _communityService = new();

        private Users _user;
        public Users User // user profile data
        {
            get => _user; // returns user object
            set
            {
                if (SetProperty(ref _user, value)) // checks if the value has changed then changes the properties
                {
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        public string Username => "@" + User?.Username; // shows the username wit hthe @ symbol

        public PublicProfileViewModel(int userId)
        {
            LoadUserProfile(userId);
        }

        private async void LoadUserProfile(int userId)
        {
            var user = await _communityService.GetUserById(userId); // calls the method from the serviced
            User = user;
        }
    }
}
