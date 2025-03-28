using LiftLab.ViewModels;

namespace LiftLab.Views;

public partial class CreateWorkoutPlanPage : ContentPage
{
    private readonly WorkoutPlansViewModel _viewModel;
    public CreateWorkoutPlanPage()
	{
		InitializeComponent();
    }

    private async void OnAddExercisesClicked(object sender, EventArgs e) // onclick button to navigate to the select workouts list page which uses navigation instead of shell for now
    {
        var viewModel = BindingContext as WorkoutPlansViewModel; // adds data binding
        if (viewModel != null) // stops potential null errors
        {
            await Navigation.PushAsync(new WorkoutSelectionPage(viewModel)); // navigates user
        }
    }

}