using LiftLab.ViewModels;

namespace LiftLab.Views;

public partial class ViewMealPlanPage : ContentPage
{
    private MealPlanViewModel _viewModel;

    public ViewMealPlanPage()
    {
        InitializeComponent();
        _viewModel = BindingContext as MealPlanViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel != null)
        {
            await _viewModel.LoadMealsForMealPlan();
        }
    }

}