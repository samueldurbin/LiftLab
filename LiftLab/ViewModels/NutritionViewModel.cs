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

namespace LiftLab.ViewModels
{
    public class NutritionViewModel : BaseViewModel
    {
        private readonly NutritionServiceUI _nutritionService;

        public ObservableCollection<Meal> UserMeals { get; set; } = new();
        public ObservableCollection<MealPlans> UserMealPlans { get; set; } = new();

        public ICommand LoadUserMealsCommand { get; }
        public ICommand NavigateToMealDetailsCommand { get; }
        public ICommand NavigateToMealPlanDetailsCommand { get; }

        public NutritionViewModel()
        {
            _nutritionService = new NutritionServiceUI();

            LoadUserMealsCommand = new Command(async () => await LoadData()); // navigation to page

            NavigateToMealDetailsCommand = new Command<Meal>(async (meal) => await NavigateToMealDetails(meal));
            NavigateToMealPlanDetailsCommand = new Command<MealPlans>(async (plan) => await NavigateToMealPlanDetails(plan));
        }

        private async Task NavigateToMealDetails(Meal meal)
        {
            if (meal == null) // checks if the meal actually exists
            {
                return;
            }
            
            await Shell.Current.GoToAsync(nameof(ViewMealsPage), true, new Dictionary<string, object>  // shell navigation to viewmealspage
            {
               { "Meal", meal } // passes the selected meal parameter
            });
        }


        private async Task NavigateToMealPlanDetails(MealPlans plan)
        {
            if (plan == null) return;

            await Shell.Current.GoToAsync(nameof(ViewMealPlanPage), true, new Dictionary<string, object>
            {
              { "MealPlan", plan }
             });
        }

        private async Task LoadData()
        {
            if (IsBusy) return;

            IsBusy = true;

            try
            {
                int userId = Preferences.Get("UserId", 0); // user preferences

                var meals = await _nutritionService.GetMealsByUserId(userId);
                var plans = await _nutritionService.GetMealPlansByUser(userId);

                UserMeals.Clear();
                UserMealPlans.Clear();

                foreach (var meal in meals) // display all the meals
                {
                    UserMeals.Add(meal);
                }

                foreach (var plan in plans) // display all the plans
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
