using LiftLab.Views;

namespace LiftLab
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage)); // this page is for navigation
            Routing.RegisterRoute(nameof(Community), typeof(Community));
            Routing.RegisterRoute(nameof(CreatePost), typeof(CreatePost));
            Routing.RegisterRoute(nameof(FitnessPage), typeof(FitnessPage));
            Routing.RegisterRoute(nameof(NutritionPage), typeof(NutritionPage));
            Routing.RegisterRoute(nameof(FriendsPage), typeof(FriendsPage));
            Routing.RegisterRoute(nameof(ViewAllWorkoutPlans), typeof(ViewAllWorkoutPlans));
            Routing.RegisterRoute(nameof(CreateWorkoutPlanPage), typeof(CreateWorkoutPlanPage));
            Routing.RegisterRoute(nameof(ViewAllWorkoutsPage), typeof(ViewAllWorkoutsPage));
            Routing.RegisterRoute(nameof(ViewMealsPage), typeof(ViewMealsPage));
            Routing.RegisterRoute(nameof(ViewMealPlanPage), typeof(ViewMealPlanPage));
            Routing.RegisterRoute(nameof(CreateMealPlanPage), typeof(CreateMealPlanPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            Routing.RegisterRoute(nameof(UpdateUserSettingsPage), typeof(UpdateUserSettingsPage));
            Routing.RegisterRoute(nameof(CreateMealsPage), typeof(CreateMealsPage));

        }
    }
}
