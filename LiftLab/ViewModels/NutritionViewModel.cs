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

        public ICommand NavigateToMealDetailsCommand { get; }
        public ICommand NavigateToMealPlanDetailsCommand { get; }

        public NutritionViewModel()
        {
            _nutritionService = new NutritionServiceUI();

            NavigateToCreateMealPlanCommand = new Command(async () =>
            {
                await Shell.Current.GoToAsync(nameof(CreateMealPlanPage));
            });

            NavigateToCreateMealsCommand = new Command(async () =>
            {
                await Shell.Current.GoToAsync(nameof(CreateMealsPage));
            });


            CreateMealCommand = new Command(async () => await CreateMeal());

            CreateMealPlanCommand = new Command(async () => await CreateMealPlan());

            LoadMealsCommand = new Command(async () => await LoadMeals());

            NavigateToMealDetailsCommand = new Command<Meals>(async (meal) => await NavigateToMealDetails(meal));
            NavigateToMealPlanDetailsCommand = new Command<MealPlans>(async (plan) => await NavigateToMealPlanDetails(plan));
        }


        private async Task NavigateToMealDetails(Meals meal)
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

        //private async Task DeleteMeal(int mealId)
        //{
        //    var result = await _nutritionService.DeleteUserMeal(mealId, currentUserId);  // currentUserId is the logged-in user’s ID
        //    if (result)
        //        await Application.Current.MainPage.DisplayAlert("Success", "Meal deleted from your account!", "OK");
        //    else
        //        await Application.Current.MainPage.DisplayAlert("Error", "Failed to delete meal.", "OK");
        //}

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

        public async Task LoadData()
        {
            if (IsBusy) return;

            IsBusy = true;

            try
            {
                int userId = Preferences.Get("UserId", 0); // user preferences

                var meals = await _nutritionService.GetMealsByUser(userId);
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


        public async Task LoadMeals()
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
        //public async Task LoadMeals()
        //{
        //    try
        //    {
        //        int userId = Preferences.Get("UserId", 0);
        //        var meals = await _nutritionService.GetMealsByUser(userId);

        //        UserMeals.Clear(); // ← important, clear first

        //        foreach (var meal in meals)
        //        {
        //            UserMeals.Add(meal); // ← load into UserMeals for the CollectionView
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        //    }
        //}

        //public async Task LoadMealPlans()
        //{
        //    try
        //    {
        //        int userId = Preferences.Get("UserId", 0);
        //        var mealPlans = await _nutritionService.GetMealPlansByUser(userId);

        //        UserMealPlans.Clear();

        //        foreach (var plan in mealPlans)
        //        {
        //            UserMealPlans.Add(plan);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        //    }
        //}

    }

}

