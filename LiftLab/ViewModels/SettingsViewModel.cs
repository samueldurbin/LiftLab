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
    public class SettingsViewModel : BaseViewModel
    {
        public ICommand AccountCommand { get; }
        public ICommand NotificationCommand { get; }
        public ICommand LogOutCommand { get; }

        public SettingsViewModel()
        {
            AccountCommand = new Command(async () => // add button in the ui navigates to create a post
            {
                await Shell.Current.GoToAsync(nameof(SettingsPage));
            });

            NotificationCommand = new Command(async () => // add button in the ui navigates to create a post
            {
                await Shell.Current.GoToAsync(nameof(SettingsPage));
            });

            LogOutCommand = new Command(async () => // add button in the ui navigates to create a post
            {
                await Shell.Current.GoToAsync(nameof(SettingsPage));
            });
        }
    }
    
}
