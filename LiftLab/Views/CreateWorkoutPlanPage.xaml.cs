using LiftLab.ViewModels;
using System.Text.Json;

namespace LiftLab.Views;

public partial class CreateWorkoutPlanPage : ContentPage
{
    private readonly WorkoutPlansViewModel _viewModel;
    public CreateWorkoutPlanPage()
	{
		InitializeComponent();
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