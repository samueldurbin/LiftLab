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
        public ICommand SettingsCommand { get; }

        public ProfileViewModel()
        {
            SettingsCommand = new Command(async () => // add button in the ui navigates to create a post
            {
                await Shell.Current.GoToAsync(nameof(SettingsPage));
            });
        }
    }
}
