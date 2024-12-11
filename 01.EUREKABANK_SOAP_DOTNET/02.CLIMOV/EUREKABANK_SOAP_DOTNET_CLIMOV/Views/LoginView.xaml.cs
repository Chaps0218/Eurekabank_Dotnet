using CONVUNI_RESTFULL_DOTNET_CLIMOV;
using EUREKABANK_SOAP_DOTNET_CLIMOV.Controller;

namespace EUREKABANK_SOAP_DOTNET_CLIMOV.Views;

public partial class LoginView : ContentPage
{
	private readonly LoginController _loginController;
    public LoginView()
	{
		InitializeComponent();
        _loginController = new LoginController();
    }

    private async Task ShowCustomAlert(string title, string message)
    {
        await Navigation.PushModalAsync(new CustomAlert(title, message));
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string username = UsernameEntry.Text?.Trim();
        string password = PasswordEntry.Text?.Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            await ShowCustomAlert("Error", "Por favor, ingrese tanto el usuario como la contraseña");
            return;
        }

        var result = await _loginController.LoginAsync(username, password);

        if (result)
        {
            await ShowCustomAlert("Éxito", "Login exitoso");
            await Navigation.PushAsync(new NavigationPage(new MovimientoView()));
        }
        else
        {
            await ShowCustomAlert("Error", "Error en el login");
        }
    }
}