using LiftLab.ViewModels;

namespace LiftLab.Views;


public partial class CreateAccount : ContentPage
{
	public CreateAccount()
	{
		InitializeComponent();
        BindingContext = new CreateAccountViewModel();
    }
}