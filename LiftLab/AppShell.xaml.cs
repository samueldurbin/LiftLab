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
            Routing.RegisterRoute(nameof(CreateNutritionPost), typeof(CreateNutritionPost));
            Routing.RegisterRoute(nameof(NutritionHomePage), typeof(NutritionHomePage));
            Routing.RegisterRoute(nameof(FriendsPage), typeof(FriendsPage));
            Routing.RegisterRoute(nameof(ViewAllWorkoutPlans), typeof(ViewAllWorkoutPlans));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));

        }
    }
}
