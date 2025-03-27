using LiftLab.ViewModels;

namespace LiftLab.Views;

public partial class Community : ContentPage
{
    private readonly CommunityViewModel _communityViewModel;
    public Community()
	{
		InitializeComponent();
        _communityViewModel = new CommunityViewModel();
        BindingContext = _communityViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // this calls the laoduserworkoutplanscommand from the viewmodel
        _communityViewModel.LoadFitnessPostsCommand.Execute(null); // this makes the data load when page is pressed
    }

}