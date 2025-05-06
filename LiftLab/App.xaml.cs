using LiftLab.Views;


namespace LiftLab
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage()); // application now starts with Navigation instead of Shell as it effects data loading and navigation bar
            // once the user has logged in, the app changes to Shell Navigation

        }
    }
}
