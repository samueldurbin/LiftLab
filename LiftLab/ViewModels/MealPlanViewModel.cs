using LiftLab.Services;
using LiftLab.Views;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LiftLab.ViewModels
{
    [QueryProperty(nameof(MealPlan), "MealPlan")]
    public class MealPlanViewModel : BaseViewModel
    {
        private MealPlans _mealPlan;
        private readonly NutritionServiceUI _nutritionService;
        public ICommand DeleteMealCommand { get; }
        public int MealPlanId => MealPlan?.MealPlanId ?? 0;

        public ICommand DeleteMealPlanCommand { get; }

        public MealPlans MealPlan
        {
            get => _mealPlan;
            set
            {
                _mealPlan = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(MealPlanName));
                OnPropertyChanged(nameof(CreatedAt));
                OnPropertyChanged(nameof(Meals));
            }
        }

        public string MealPlanName => MealPlan?.MealPlanName;
        public DateTime CreatedAt => MealPlan?.CreatedAt ?? DateTime.MinValue;
        public ICollection<Meals> Meals => MealPlan?.Meals ?? new List<Meals>();

        public ICommand NavigateToMealDetailsCommand { get; }

        public MealPlanViewModel()
        {
            _nutritionService = new NutritionServiceUI();
            DeleteMealCommand = new Command<Meals>(async (meal) => await DeleteMeal(meal));
            DeleteMealPlanCommand = new Command<int>(async (mealPlanId) => await DeleteMealPlan(mealPlanId));
            NavigateToMealDetailsCommand = new Command<Meals>(async (meal) => await NavigateToMealDetails(meal));
        }
        private async Task DeleteMeal(Meals meal)
        {
            if (meal == null)
                return;

            int userId = Preferences.Get("UserId", 0); // Fetch logged-in user's ID from Preferences

            if (userId == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "User not logged in properly.", "OK");
                return;
            }

            var result = await _nutritionService.DeleteUserMeal(meal.MealId, userId);
            if (result)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Meal deleted!", "OK");
                MealPlan.Meals.Remove(meal);
                OnPropertyChanged(nameof(Meals));
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to delete meal.", "OK");
            }
        }

        private async Task DeleteMealPlan(int mealPlanId)
        {
            var result = await _nutritionService.DeleteMealPlan(mealPlanId);
            if (result)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Meal plan deleted!", "OK");
                await Shell.Current.GoToAsync($"//NutritionPage");

            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to delete meal plan.", "OK");
            }
               
        }

        private async Task NavigateToMealDetails(Meals meal)
        {
            if (meal == null)
                return;

            await Shell.Current.GoToAsync(nameof(ViewMealsPage), true, new Dictionary<string, object>
            {
                { "Meal", meal }
            });
        }
    }
}
