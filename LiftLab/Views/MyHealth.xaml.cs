using LiftLab.ViewModels;
namespace LiftLab.Views;

public partial class MyHealth : ContentPage
{
	public MyHealth()
	{
		InitializeComponent();
		BindingContext = new WorkoutPlansViewModel();
	}


}