using LiftLab.ViewModels;
namespace LiftLab.Views;

public partial class CreateMealPlanPage : ContentPage
{
	public CreateMealPlanPage()
	{
		InitializeComponent();
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is NutritionViewModel viewModel)
        {
            await viewModel.LoadMeals();
        }
    }
}