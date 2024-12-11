namespace CONVUNI_RESTFULL_DOTNET_CLIMOV;

public partial class CustomAlert : ContentPage
{
    public CustomAlert(string title, string message)
    {
        InitializeComponent();
        TitleLabel.Text = title;
        MessageLabel.Text = message;
    }

    private async void OnOkClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}