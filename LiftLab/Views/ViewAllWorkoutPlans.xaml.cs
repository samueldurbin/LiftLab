using LiftLab.ViewModels;

namespace LiftLab.Views;

public partial class ViewAllWorkoutPlans : ContentPage
{
    private readonly WorkoutPlansViewModel _workoutPlansViewModel;
    public ViewAllWorkoutPlans()
	{
		InitializeComponent();
        _workoutPlansViewModel = new WorkoutPlansViewModel();
        BindingContext = _workoutPlansViewModel; // binding context to the viewmodel
    }

    protected override void OnAppearing() 
    {
        base.OnAppearing();
        // this calls the laoduserworkoutplanscommand from the viewmodel
        _workoutPlansViewModel.LoadUserWorkoutPlansCommand.Execute(null); // this makes the data load when page is pressed
    }
}