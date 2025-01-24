using LiftLab.ViewModels;

namespace LiftLab.Views;

public partial class MyCommunity : ContentPage
{
	public MyCommunity()
	{
		InitializeComponent();
        BindingContext = new CommunityViewModel();
    }

    
}