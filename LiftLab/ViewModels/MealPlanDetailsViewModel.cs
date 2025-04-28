using Shared.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LiftLab.Services;
using LiftLab.Views;
using LiftLab.Models;
using System.Net.Http.Json;

namespace LiftLab.ViewModels
{
    [QueryProperty(nameof(MealPlanId), "mealPlanId")]
    public class MealPlanDetailsViewModel : BaseViewModel
    {
        private readonly NutritionServiceUI _nutritionService;

        public int MealPlanId { get; set; }
        public MealPlans MealPlan { get; set; }
        public List<Meals> Meals { get; set; }

        public string MealPlanName => MealPlan?.MealPlanName ?? string.Empty;
        public string CreatedAt => MealPlan?.CreatedAt.ToString("MMMM dd, yyyy") ?? string.Empty;

        public ICommand DeleteMealCommand { get; }
        public ICommand DeleteMealPlanCommand { get; }

        public MealPlanDetailsViewModel()
        {
            _nutritionService = new NutritionServiceUI();

            DeleteMealCommand = new Command<int>(async (mealId) => await DeleteMeal(mealId));
            DeleteMealPlanCommand = new Command<int>(async (mealPlanId) => await DeleteMealPlan(mealPlanId));
        }

        public async Task LoadMealPlanDetails(int mealPlanId)
        {
            if (IsBusy) return;

            IsBusy = true;

            try
            {
                var plans = await _nutritionService.GetMealPlansByUser(Preferences.Get("UserId", 0));

                MealPlan = plans.FirstOrDefault(mp => mp.MealPlanId == mealPlanId);

                if (MealPlan != null)
                {
                    Meals = await _nutritionService.GetMealsByPlanId(mealPlanId);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Not Found", "Meal Plan not found.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error loading meal plan details: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task DeleteMeal(int mealId)
        {
            var result = await _nutritionService.DeleteMeal(mealId);
            if (result)
                await Application.Current.MainPage.DisplayAlert("Success", "Meal deleted!", "OK");
            else
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to delete meal.", "OK");
        }

        private async Task DeleteMealPlan(int mealPlanId)
        {
            try
            {
                if (mealPlanId == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No meal plan selected.", "OK");
                    return;
                }

                var response = await _nutritionService.DeleteMealPlan(mealPlanId);

                if (response)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", "Meal plan deleted!", "OK");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to delete meal plan.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to delete meal plan: {ex.Message}", "OK");
            }
        }
    }
}
