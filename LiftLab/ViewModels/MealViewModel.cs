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
    [QueryProperty(nameof(Meal), "Meal")]
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

        private async Task DeleteMeal()
        {
            int userId = Preferences.Get("UserId", 0);

            if (Meal == null || userId == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Meal not loaded properly or user not logged in.", "OK");
                return;
            }

            var result = await _nutritionService.DeleteMeal(Meal.MealId);

            if (result)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Meal deleted!", "OK");
                await Shell.Current.GoToAsync("//NutritionPage");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to delete meal.", "OK");
            }
        }
    }

}