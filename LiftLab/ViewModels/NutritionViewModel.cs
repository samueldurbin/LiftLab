using Shared.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LiftLab.Services;

namespace LiftLab.ViewModels
{
    public class NutritionViewModel : BaseViewModel
    {
        private readonly NutritionServiceUI _nutritionService;

        public ObservableCollection<Meal> UserMeals { get; set; } = new();
        public ObservableCollection<MealPlan> UserMealPlans { get; set; } = new();

        public ICommand LoadUserMealsCommand { get; }

        public NutritionViewModel()
        {
            _nutritionService = new NutritionServiceUI();

            LoadUserMealsCommand = new Command(async () => await LoadData());
        }

        private async Task LoadData()
        {
            if (IsBusy) return;

            IsBusy = true;

            try
            {
                int userId = Preferences.Get("UserId", 0);

                var meals = await _nutritionService.GetMealsByUserId(userId);
                var plans = await _nutritionService.GetMealPlansByUser(userId);

                UserMeals.Clear();
                UserMealPlans.Clear();

                foreach (var meal in meals)
                {
                    UserMeals.Add(meal);
                }

                foreach (var plan in plans)
                {
                    UserMealPlans.Add(plan);
                }
                   
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load nutritional data: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
