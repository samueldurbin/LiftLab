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
    public class NutritionViewModel : BaseViewModel
    {
        private readonly NutritionServiceUI _nutritionService;
        public ObservableCollection<Meals> UserMeals { get; set; } = new();
        public ObservableCollection<MealPlans> UserMealPlans { get; set; } = new();
        public ObservableCollection<MealSelection> MealList { get; set; } = new();

        public ICommand CreateMealCommand { get; }
        public ICommand CreateMealPlanCommand { get; }
        public ICommand NavigateToCreateMealsCommand { get; }
        public ICommand NavigateToCreateMealPlanCommand { get; }
        public ICommand LoadMealsCommand { get; }
        public ICommand NavigateToMealDetailsCommand { get; }
        public ICommand NavigateToMealPlanDetailsCommand { get; }

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

        public NutritionViewModel()
        {
            _nutritionService = new NutritionServiceUI();

            NavigateToCreateMealPlanCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(CreateMealPlanPage)));
            NavigateToCreateMealsCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(CreateMealsPage)));

            CreateMealCommand = new Command(async () => await CreateMeal());
            CreateMealPlanCommand = new Command(async () => await CreateMealPlan());
            LoadMealsCommand = new Command(async () => await LoadMeals());

            NavigateToMealDetailsCommand = new Command<Meals>(async (meal) => await NavigateToMealDetails(meal));
            NavigateToMealPlanDetailsCommand = new Command<MealPlans>(async (plan) => await NavigateToMealPlanDetails(plan));
        }

        private async Task CreateMeal() // creates an individual meal
        {
            try
            {
                int userId = Preferences.Get("UserId", 0); // gets the logged in user

                var meal = new Meals // creates a meal
                {
                    MealName = NewMealName,
                    Type = NewMealType,
                    Calories = NewMealCalories,
                    Recipe = NewMealRecipe,
                    UserId = userId
                };

                var created = await _nutritionService.CreateMeal(meal); // sends to api

                UserMeals.Add(created); // adds to local collection
                await Application.Current.MainPage.DisplayAlert("Success", "Meal created successfully!", "OK");

                NewMealName = NewMealType = NewMealRecipe = string.Empty; // clears inputs for next meal
                NewMealCalories = null;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task CreateMealPlan() // creates meal plan
        {
            try
            {
                int userId = Preferences.Get("UserId", 0); // gets the logged in user

                var selectedMealIds = MealList // gets the selected meals
                    .Where(m => m.IsSelected)
                    .Select(m => m.Meal.MealId)
                    .ToList();

                if (string.IsNullOrWhiteSpace(MealPlanName) || !selectedMealIds.Any()) // prevents blank meal plan names or blank meal plans
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Please provide a meal plan name and select at least one meal.", "OK");
                    return;
                }

                var dto = new CreateMealPlanDTO // dto to the api
                {
                    MealPlanName = MealPlanName,
                    UserId = userId,
                    MealIds = selectedMealIds
                };

                var createdPlan = await _nutritionService.CreateMealPlan(dto);

                UserMealPlans.Add(createdPlan); // adds to the ui list

                await Application.Current.MainPage.DisplayAlert("Success", "Meal plan created!", "OK");

                MealPlanName = string.Empty; // clears name and selected list
                foreach (var item in MealList)
                {
                    item.IsSelected = false;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        public async Task LoadData()
        {
            if (IsBusy) 
            {
                return;
            } 

            IsBusy = true;

            try
            {
                int userId = Preferences.Get("UserId", 0); // gets logged in userid

                var meals = await _nutritionService.GetMealsByUser(userId);
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
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load nutrition data: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task LoadMeals()
        {
            try
            {
                int userId = Preferences.Get("UserId", 0);

                var meals = await _nutritionService.GetMealsByUser(userId);

                MealList.Clear();
                foreach (var meal in meals) // loops through the meals
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

        private async Task NavigateToMealDetails(Meals meal)
        {
            if (meal == null) 
            {
                return;

            }
 

            await Shell.Current.GoToAsync(nameof(ViewMealsPage), true, new Dictionary<string, object>
            {
                { "Meal", meal } // query parameters
            });
        }

        private async Task NavigateToMealPlanDetails(MealPlans plan)
        {
            if (plan == null)
            {
                return;
            }
            

            await Shell.Current.GoToAsync(nameof(ViewMealPlanPage), true, new Dictionary<string, object>
            {
                { "MealPlan", plan } // mealplan as the query parameter
            });
        }
    }

}

