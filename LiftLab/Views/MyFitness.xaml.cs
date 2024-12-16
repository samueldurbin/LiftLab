namespace LiftLab.Views;

public partial class MyFitness : ContentPage
{
	public MyFitness()
	{
		InitializeComponent();
	}

    private async void OnButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CreateWorkout());

    }
}