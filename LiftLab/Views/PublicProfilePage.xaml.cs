using LiftLab.ViewModels;

namespace LiftLab.Views;

public partial class PublicProfilePage : ContentPage
{
	public PublicProfilePage(int userId) // the input user id to display
	{
		InitializeComponent();
        BindingContext = new PublicProfileViewModel(userId);
    }
}