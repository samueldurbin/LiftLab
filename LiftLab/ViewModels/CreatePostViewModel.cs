using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LiftLab.Services;
using Shared.Models;

namespace LiftLab.ViewModels
{
    public class CreatePostViewModel : BaseViewModel
    {
        private readonly CommunityPostServiceUI _communityService;
        private readonly NutritionServiceUI _nutritionService;

        public ObservableCollection<WorkoutPlans> WorkoutPlans { get; set; }
        public ObservableCollection<MealPlans> MealPlans { get; set; }
        public ObservableCollection<Meals> Meals { get; set; }

        #region Variables
        private WorkoutPlans selectedWorkoutPlan;
        private MealPlans selectedMealPlan;
        private Meals selectedMeals;
        private bool showWorkoutPlans;
        private bool showMealPlans;
        private bool showMeals;
        private string caption;
        #endregion

        #region ICommands

        public ICommand ToggleWorkoutPlansCommand { get; }
        public ICommand ToggleMealPlansCommand { get; }
        public ICommand ToggleMealsCommand { get; }
        public ICommand ClearWorkoutPlanCommand { get; }
        public ICommand ClearMealPlanCommand { get; }
        public ICommand ClearMealsCommand { get; }
        public ICommand CreatePostCommand { get; }

        #endregion

        public CreatePostViewModel()
        {
            _nutritionService = new NutritionServiceUI();
            #region Instance Setting
            _communityService = new CommunityPostServiceUI();
            WorkoutPlans = new ObservableCollection<WorkoutPlans>();
            MealPlans = new ObservableCollection<MealPlans>();
            Meals = new ObservableCollection<Meals>();

            #endregion

            #region ToggleVisbility
            ToggleWorkoutPlansCommand = new Command(async () =>
            {
                ClearOtherSelections("Workout");

                ShowWorkoutPlans = !ShowWorkoutPlans;

                if (ShowWorkoutPlans && WorkoutPlans.Count == 0)
                {
                    await LoadWorkoutPlans();
                }
            });

            ToggleMealPlansCommand = new Command(async () =>
            {
                ClearOtherSelections("MealPlan");

                ShowMealPlans = !ShowMealPlans;

                await LoadMealPlans();

            });

            ToggleMealsCommand = new Command(async () =>
            {
                ClearOtherSelections("Meal");

                ShowMeals = !ShowMeals;

                await LoadMeals();

            });
            #endregion

            #region Bin Clear
            ClearWorkoutPlanCommand = new Command(() => // bin icon to clear the entry of the user input
            {
                SelectedWorkoutPlan = null;
                ShowWorkoutPlans = false;
            });

            ClearMealPlanCommand = new Command(() =>
            {
                SelectedMealPlan = null;
                ShowMealPlans = false;
            });

            ClearMealsCommand = new Command(() =>
            {
                SelectedMeals = null;
                ShowMeals = false;
            });
            #endregion

            #region Create Post


            CreatePostCommand = new Command(async () => await CreatePost());
            #endregion
        }

        #region Caption
        public string Caption // gets the user input for the caption and sets the value
        {
            get => caption;
            set => SetProperty(ref caption, value);
        }
        #endregion

        #region Selected Plans

        public WorkoutPlans SelectedWorkoutPlan
        {
            get => selectedWorkoutPlan;
            set
            {
                if (SetProperty(ref selectedWorkoutPlan, value))
                {
                    if (value != null)
                    {
                        ClearOtherSelections("Workout");
                    }
                }
            }
        }

        public MealPlans SelectedMealPlan
        {
            get => selectedMealPlan;
            set
            {
                if (SetProperty(ref selectedMealPlan, value))
                {
                    if (value != null)
                    {
                        ClearOtherSelections("MealPlan");
                    }
                }
            }
        }

        public Meals SelectedMeals
        {
            get => selectedMeals;
            set
            {
                if (SetProperty(ref selectedMeals, value))
                {
                    if (value != null)
                    {
                        ClearOtherSelections("Meal");
                    }
                }
            }
        }

        #endregion

        #region Show
        public bool ShowWorkoutPlans
        {
            get => showWorkoutPlans;
            set => SetProperty(ref showWorkoutPlans, value);
        }

        public bool ShowMealPlans
        {
            get => showMealPlans;
            set => SetProperty(ref showMealPlans, value);
        }

        public bool ShowMeals
        {
            get => showMeals;
            set => SetProperty(ref showMeals, value);
        }

        #endregion

        #region Loading Plans
        private async Task LoadWorkoutPlans()
        {
            int userId = Preferences.Get("UserId", 0);

            var plans = await _communityService.GetWorkoutPlansByUserId(userId);

            WorkoutPlans.Clear();

            foreach (var plan in plans)
            {
                WorkoutPlans.Add(plan);
            }

        }

        private async Task LoadMealPlans()
        {
            int userId = Preferences.Get("UserId", 0);

            var plans = await _nutritionService.GetMealPlansByUser(userId);

            MealPlans.Clear();

            foreach (var plan in plans)
            {
                MealPlans.Add(plan);
            }
        }

        private async Task LoadMeals()
        {
            int userId = Preferences.Get("UserId", 0);

            var meals = await _nutritionService.GetMealsByUser(userId);

            Meals.Clear();

            foreach (var meal in meals)
            {
                Meals.Add(meal);
            }
        }

        //private async Task LoadMealPlans()
        //{
        //    int userId = Preferences.Get("UserId", 0);

        //    var plans = await _communityService.GetMealPlansByUserId(userId);

        //    MealPlans.Clear();

        //    foreach (var plan in plans)
        //    {
        //        MealPlans.Add(plan);
        //    }

        //}

        //private async Task LoadMeals()
        //{
        //    int userId = Preferences.Get("UserId", 0);

        //    var meals = await _communityService.GetMealsByUser(userId);

        //    Meals.Clear();

        //    foreach (var meal in meals)
        //    {
        //        Meals.Add(meal);
        //    }

        //}

        #endregion

        private async Task CreatePost()
        {
            int userId = Preferences.Get("UserId", 0);
            string username = Preferences.Get("Username", "Unknown");

            if (string.IsNullOrWhiteSpace(Caption)) // this makes sure that a caption is added before posting, this is due to preventing spam posts
            {
                await Application.Current.MainPage.DisplayAlert("Missing Caption", "Please enter a caption before posting.", "OK");
                return;
            }

            int? planId = SelectedWorkoutPlan?.WorkoutPlanId; // optional selections
            int? mealPlanId = SelectedMealPlan?.MealPlanId;
            int? mealId = SelectedMeals?.MealId;

            var newPost = await _communityService.CreatePost(userId, username, Caption, planId, mealId, mealPlanId);

            if (newPost != null)
            {
                await Application.Current.MainPage.DisplayAlert("Success!", "Your post has been created!", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to create post.", "OK");
            }

            await Shell.Current.GoToAsync(".."); // this makes the user get redirected back to the community page after posting
        }


        #region Helper Methods

        private void ClearOtherSelections(string clear)
        {
            if (clear != "Workout")
            {
                SelectedWorkoutPlan = null;
                ShowWorkoutPlans = false;
            }

            if (clear != "MealPlan")
            {
                SelectedMealPlan = null;
                ShowMealPlans = false;
            }

            if (clear != "Meal")
            {
                SelectedMeals = null;
                ShowMeals = false;
            }
        }

        #endregion

    }
}

