using LiftLab.Services;
using LiftLab.Views;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LiftLab.ViewModels
{
    [QueryProperty(nameof(MealPlan), "MealPlan")]
    public class MealPlanViewModel : BaseViewModel
    {
        private readonly NutritionServiceUI _nutritionService;
        private MealPlans _mealPlan;

        public ObservableCollection<Meals> Meals { get; set; } = new();

        public ICommand DeleteMealPlanCommand { get; }
        public ICommand NavigateToMealDetailsCommand { get; }

        public MealPlans MealPlan
        {
            get => _mealPlan;
            set
            {
                _mealPlan = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(MealPlanName));
                OnPropertyChanged(nameof(CreatedAt));
            }
        }

        public string MealPlanName => MealPlan?.MealPlanName;
        public DateTime CreatedAt => MealPlan?.CreatedAt ?? DateTime.MinValue;

        public MealPlanViewModel()
        {
            _nutritionService = new NutritionServiceUI();
            DeleteMealPlanCommand = new Command<int>(async (mealPlanId) => await DeleteMealPlan(mealPlanId));
            NavigateToMealDetailsCommand = new Command<Meals>(async (meal) => await NavigateToMealDetails(meal));
        }

        public async Task LoadMealsForMealPlan()
        {
            if (MealPlan == null) return;

            var loadedMeals = await _nutritionService.GetMealsByPlanId(MealPlan.MealPlanId);

            Meals.Clear();
            foreach (var meal in loadedMeals)
            {
                Meals.Add(meal);
            }
        }

        private async Task DeleteMealPlan(int mealPlanId)
        {
            var confirm = await Application.Current.MainPage.DisplayAlert("Confirm", "Are you sure you want to delete this meal plan?", "Yes", "No");

            if (!confirm) return;

            var result = await _nutritionService.DeleteMealPlan(mealPlanId);

            if (result)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Meal plan deleted!", "OK");
                await Shell.Current.GoToAsync("//NutritionPage");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to delete meal plan.", "OK");
            }
        }

        private async Task NavigateToMealDetails(Meals meal)
        {
            if (meal == null) return;

            await Shell.Current.GoToAsync(nameof(ViewMealsPage), true, new Dictionary<string, object>
            {
                { "Meal", meal }
            });
        }
    }

}

