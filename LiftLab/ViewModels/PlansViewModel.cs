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
    public class PlansViewModel : BaseViewModel
    {

        private readonly NutritionServiceUI _nutritionService;
        public ObservableCollection<Meals> UserMeals { get; set; } = new();
        public ObservableCollection<MealSelection> MealList { get; set; } = new();
        public ICommand CreateMealCommand { get; }
        public ICommand CreateMealPlanCommand { get; }

        public ICommand NavigateToCreateMealPlanCommand { get; }
        public ICommand LoadMealsCommand { get; }

        private string mealPlanName;
        public string MealPlanName
        {
            get => mealPlanName;
            set => SetProperty(ref mealPlanName, value);
        }

        private string newMealName;
        public string NewMealName
        {
            get => newMealName;
            set => SetProperty(ref newMealName, value);
        }

        private string newMealType;
        public string NewMealType
        {
            get => newMealType;
            set => SetProperty(ref newMealType, value);
        }

        private int? newMealCalories;
        public int? NewMealCalories
        {
            get => newMealCalories;
            set => SetProperty(ref newMealCalories, value);
        }

        private string newMealRecipe;
        public string NewMealRecipe
        {
            get => newMealRecipe;
            set => SetProperty(ref newMealRecipe, value);
        }

        public PlansViewModel()
        {
            _nutritionService = new NutritionServiceUI();

            NavigateToCreateMealPlanCommand = new Command(async () =>
            {
                await Shell.Current.GoToAsync(nameof(CreateMealPlanPage));
            });

            CreateMealCommand = new Command(async () => await CreateMeal());

            CreateMealPlanCommand = new Command(async () => await CreateMealPlan());

            LoadMealsCommand = new Command(async () => await LoadMeals());
        }

        private async Task CreateMeal()
        {
            try
            {
                int userId = Preferences.Get("UserId", 0);

                var meal = new Meals
                {
                    MealName = NewMealName,
                    Type = NewMealType,
                    Calories = NewMealCalories,
                    Recipe = NewMealRecipe,
                    UserId = userId
                };

                var created = await _nutritionService.CreateMeal(meal);

                UserMeals.Add(created);
                await Application.Current.MainPage.DisplayAlert("Success", "Meal created!", "OK");

                NewMealName = NewMealType = NewMealRecipe = string.Empty; // clears 
                NewMealCalories = null;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task CreateMealPlan()
        {
            try
            {
                int userId = Preferences.Get("UserId", 0);

                var selectedMealIds = MealList
                    .Where(m => m.IsSelected)
                    .Select(m => m.Meal.MealId)
                    .ToList();

                var dto = new CreateMealPlanDTO
                {
                    MealPlanName = MealPlanName,
                    UserId = userId,
                    MealIds = selectedMealIds
                };

                var newPlan = await _nutritionService.CreateMealPlan(dto);
                await Application.Current.MainPage.DisplayAlert("Success", "Meal plan created!", "Nice");

                foreach (var meal in MealList)
                {
                    meal.IsSelected = false;
                }

                MealPlanName = string.Empty;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task LoadMeals()
        {
            try
            {
                int userId = Preferences.Get("UserId", 0);
                var meals = await _nutritionService.GetMealsByUser(userId);

                MealList.Clear();
                foreach (var meal in meals)
                {
                    MealList.Add(new MealSelection
                    {
                        Meal = meal,
                        IsSelected = false
                    });
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
