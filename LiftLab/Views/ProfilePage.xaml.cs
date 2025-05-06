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

        if (BindingContext is ProfileViewModel viewModel) // decides what user to load (own profile or someone else's)
        {
            int loggedInUserId = Preferences.Get("UserId", 0);

            int userId = UserId == 0 ? loggedInUserId : UserId;

            viewModel.UserProfile = userId == loggedInUserId;

            // Show loading spinner while loading profile
            viewModel.IsBusy = true;
            await viewModel.LoadUserProfile(userId);
            viewModel.IsBusy = false;
        }
    }
}