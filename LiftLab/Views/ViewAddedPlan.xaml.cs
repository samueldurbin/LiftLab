using CommunityToolkit.Maui.Views;

namespace LiftLab.Views;

public partial class ViewAddedPlan : Popup
{
	public ViewAddedPlan()
	{
		InitializeComponent();
	}

    private void OnCloseClicked(object sender, EventArgs e)
    {
        Close();
    }
}