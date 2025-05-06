using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LiftLab.Services;
using Shared.Models;

namespace LiftLab.ViewModels
{
    [QueryProperty(nameof(Meal), "Meal")] // query parameters
    public class MealViewModel : BaseViewModel
    {
        private readonly NutritionServiceUI _nutritionService;
        private Meals _meal;

        public Meals Meal
        {
            get => _meal;
            set
            {
                _meal = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(MealName));
                OnPropertyChanged(nameof(Type));
                OnPropertyChanged(nameof(Calories));
                OnPropertyChanged(nameof(Recipe));
            }
        }

        public string MealName => Meal?.MealName;
        public string Type => Meal?.Type;
        public int? Calories => Meal?.Calories;
        public string Recipe => Meal?.Recipe;

        public ICommand DeleteMealCommand { get; }

        public MealViewModel()
        {
            _nutritionService = new NutritionServiceUI();
            DeleteMealCommand = new Command(async () => await DeleteMeal());
        }


        private async Task DeleteMeal() // deletes the meal
        {
            if (Meal == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Meal not loaded properly.", "OK");
                return;
            }

            bool confirmed = await Application.Current.MainPage.DisplayAlert(
                "Confirm Deletion",
                $"Are you sure you want to delete '{Meal.MealName}'?",
                "Delete",
                "Cancel"
            );

            if (!confirmed)
            {
                return;

            }
                

            try
            {
                bool success = await _nutritionService.DeleteMeal(Meal.MealId);

                if (success)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Meal deleted successfully.", "OK");
                    await Shell.Current.GoToAsync("//NutritionPage"); // Go back to main nutrition page
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to delete meal. Please try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Deletion failed: {ex.Message}", "OK");
            }
        }
    }
    

}