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
    public class ProfileViewModel : BaseViewModel
    {
        private string username;
        public string Username // this changes the incoming username to the UI
        {
            get => username;
            set => SetProperty(ref username, value);
        }

        public ICommand SettingsCommand { get; }

        public ProfileViewModel()
        {
            Username = "@" + Preferences.Get("Username", "Unknown"); // this gets the username of the user thats logged in and binds it to the ui

            SettingsCommand = new Command(async () => // add button in the ui navigates to create a post
            {
                await Shell.Current.GoToAsync(nameof(SettingsPage));
            });
        }
    }
}
