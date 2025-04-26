using LiftLab.ViewModels;
namespace LiftLab.Views;

public partial class Community : ContentPage
{
    private readonly CommunityPostViewModel _communityPostViewModel;
    public Community()
    {
        InitializeComponent();
        _communityPostViewModel = new CommunityPostViewModel();
        BindingContext = _communityPostViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _communityPostViewModel.LoadCommunityPostsCommand.Execute(null); // this makes the data load when page is pressed
    }
}