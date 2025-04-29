using System.Windows.Input;
using CommunityToolkit.Maui.Views;


namespace LiftLab.Views;

public partial class ViewTandC : Popup
{
    public ICommand CloseCommand { get; }
    public ViewTandC()
	{
		InitializeComponent();

        BindingContext = new
        {
            CloseCommand = new Command(() => Close())
        };
    }
}