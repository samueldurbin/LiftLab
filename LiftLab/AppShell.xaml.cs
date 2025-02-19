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
            Routing.RegisterRoute(nameof(CreateWorkout), typeof(CreateWorkout));
            Routing.RegisterRoute(nameof(CreatePost), typeof(CreatePost));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));

        }
    }
}
