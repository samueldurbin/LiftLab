using LiftLab.ViewModels;
namespace LiftLab.Views;


public partial class WorkoutSelectionPage : ContentPage
{
    public WorkoutSelectionPage(WorkoutPlansViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = viewModel; // this is for data binding
    }

    protected override void OnAppearing() // same on appearing method used by gettings posts and plans, gets used everytime the page is visible
    {
        base.OnAppearing();

        var viewModel = BindingContext as WorkoutPlansViewModel;

        if (viewModel?.WorkoutList.Count == 0)  // if no workouts are there, then use method to get workouts
        {
            viewModel.GetWorkoutsCommand.Execute(null);
        }
    }
}