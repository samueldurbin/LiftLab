using LiftLab.ViewModels;
using Shared.Models;

namespace LiftLab.Views;

[QueryProperty(nameof(UserId), "userId")] // the userid of the profile
public partial class ProfilePage : ContentPage
{
    public int UserId { get; set; } // holds the userid parameter
    public ProfilePage()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ProfileViewModel viewModel) // binding using viewmodel
        {
            int loggedInUserId = Preferences.Get("UserId", 0); // this gets the preferences from the logged in user

            int userId = UserId == 0 ? loggedInUserId : UserId;

            viewModel.UserProfile = userId == loggedInUserId;

            await viewModel.LoadUserProfile(userId);
        }
    }
}