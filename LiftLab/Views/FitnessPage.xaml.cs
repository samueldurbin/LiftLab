namespace LiftLab.Views;

public partial class FitnessPage : ContentPage
{
	public FitnessPage()
	{
		InitializeComponent();
	}
    private async void OnCreateClicked(object sender, EventArgs e) // simple navigation for creating workout
    {
        await Navigation.PushAsync(new CreateWorkoutPage());
    }
    private async void OnViewClicked(object sender, EventArgs e) // simple navigation for testing
    {
        await Navigation.PushAsync(new CreateWorkoutPage());
    }
}