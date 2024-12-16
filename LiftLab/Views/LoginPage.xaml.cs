namespace LiftLab.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();



	}

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        // Perform login validation (example logic)
        
            // Navigate to the main app (Shell)
        Application.Current.MainPage = new AppShell();
   
    }

    private async void OnRegisterTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CreateAccount());
    }

}