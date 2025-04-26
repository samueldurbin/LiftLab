using LiftLab.ViewModels;
using Shared.Models;

namespace LiftLab.Views;

public partial class NutritionPage : ContentPage
{
    private NutritionViewModel _viewModel;
    public NutritionPage()
	{
		InitializeComponent();
        _viewModel = BindingContext as NutritionViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel != null)
        {
            await _viewModel.LoadData();
        }
    }

}