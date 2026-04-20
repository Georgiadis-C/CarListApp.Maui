namespace CarListApp.Maui.Controls;

public partial class FlyOutHeader : StackLayout
{
	public FlyOutHeader()
	{
		InitializeComponent();
		SetValues();
	}

    private void SetValues()
    {
		if(App.UserInfo != null)
		{
			lblUsername.Text = App.UserInfo.Username;
			lblRole.Text = App.UserInfo.Role;
        }
    }
}