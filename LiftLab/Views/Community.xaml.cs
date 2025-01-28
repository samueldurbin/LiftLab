using LiftLab.ViewModels;

namespace LiftLab.Views;

public partial class Community : ContentPage
{
	public Community()
	{
		InitializeComponent();
        BindingContext = new CommunityViewModel();
    }

}