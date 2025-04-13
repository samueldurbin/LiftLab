using CommunityToolkit.Maui.Views;
using System.Windows.Input;

namespace LiftLab.Views;

public partial class ViewAddedPlan : Popup
{
    public ICommand CloseCommand { get; }

    // constructor for the popup view
    public ViewAddedPlan(string title, List<string> items, string? description = null)
	{
        InitializeComponent();

        BindingContext = new
        {
            PlanTitle = title,
            ItemNames = items,
            Description = description,
            CloseCommand = new Command(() => Close())
        };
    }

}