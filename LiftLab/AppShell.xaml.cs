using LiftLab.Views;

namespace LiftLab
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(Community), typeof(Community));
            Routing.RegisterRoute(nameof(CreatePost), typeof(CreatePost));
            Routing.RegisterRoute(nameof(NutritionPage), typeof(NutritionPage));
            Routing.RegisterRoute(nameof(FriendsPage), typeof(FriendsPage));
            Routing.RegisterRoute(nameof(ViewAllWorkoutPlans), typeof(ViewAllWorkoutPlans));
            Routing.RegisterRoute(nameof(CreateWorkoutPlanPage), typeof(CreateWorkoutPlanPage));
            Routing.RegisterRoute(nameof(ViewAllWorkoutsPage), typeof(ViewAllWorkoutsPage));
            Routing.RegisterRoute(nameof(WorkoutSelectionPage), typeof(WorkoutSelectionPage));
            Routing.RegisterRoute(nameof(ViewMealsPage), typeof(ViewMealsPage));
            Routing.RegisterRoute(nameof(ViewMealPlanPage), typeof(ViewMealPlanPage));
            Routing.RegisterRoute(nameof(PublicProfilePage), typeof(PublicProfilePage));



        }
    }
}
