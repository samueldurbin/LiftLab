using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiftLab.Views;
using System.Windows.Input;
using LiftLab.Services;
using Shared.Models;

namespace LiftLab.ViewModels
{
    public class CommunityViewModel : BaseViewModel
    {
        private readonly FitnessPostServiceUI _apiFitnessPostService;

        public ICommand NavigateCommand { get; }

        public CommunityViewModel()
        {
            NavigateCommand = new Command(async () =>
            {
                // Navigate to CreateWorkout page
                await Shell.Current.GoToAsync(nameof(CreatePost));
            });
        }

    }
}
