using Shared.Models;
using LiftLab.ViewModels;

namespace LiftLab.Views;

// Query parameters are used to navigate to pages using input parameters in this case the workoutplandata id: learnt from Microsoft Maui Guidance
[QueryProperty(nameof(WorkoutPlanData), "WorkoutPlanData")] // this is where the workoutplandata id goes so the page opens on the related workouts
public partial class ViewAllWorkoutsPage : ContentPage
{
    private WorkoutPlans workoutPlanData;
    private WorkoutsInPlanViewModel viewModel; // variables to be called

    public ViewAllWorkoutsPage()
    {
        InitializeComponent();
        BindingContext = viewModel = new WorkoutsInPlanViewModel(); // this binds the context with the viewmodel methods
    }

    public WorkoutPlans WorkoutPlanData // this is how the selected workout param gets intserted into the queryproperty
    {
        set
        {
            viewModel.SelectedPlan = value;
        }
    }

}