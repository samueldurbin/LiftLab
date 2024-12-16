using System.Collections.ObjectModel;

namespace LiftLab.Views;

public partial class CreateWorkout : ContentPage
{
    public CreateWorkout()
	{
		InitializeComponent();
        BindingContext = this;
    }

}