using LiftLab.Views;


namespace LiftLab
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());

            ////Routing.RegisterRoute("HomePage", typeof(HomePage));
            //Routing.RegisterRoute("ProfilePage", typeof(ProfilePage));
        }
    }
}
