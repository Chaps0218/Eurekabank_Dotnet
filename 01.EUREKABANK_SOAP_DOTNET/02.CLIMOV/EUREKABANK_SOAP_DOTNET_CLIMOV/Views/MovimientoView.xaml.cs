using CONVUNI_RESTFULL_DOTNET_CLIMOV;
using EUREKABANK_SOAP_DOTNET_CLIMOV.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace EUREKABANK_SOAP_DOTNET_CLIMOV.Views
{
    public partial class MovimientoView : ContentPage
    {
        private MovimientoController _movimientoController;

        public MovimientoView()
        {
            InitializeComponent();
            _movimientoController = new MovimientoController();
        }

        private async Task ShowCustomAlert(string title, string message)
        {
            await Navigation.PushModalAsync(new CustomAlert(title, message));
        }

        private async void OnSearchClicked(object sender, EventArgs e)
        {
            await BuscarMovimientos();
        }

        private async Task BuscarMovimientos()
        {
            string numeroCuenta = AccountEntry.Text?.Trim();

            if (string.IsNullOrEmpty(numeroCuenta))
            {
                await DisplayAlert("Error", "Por favor ingrese un número de cuenta", "OK");
                return;
            }

            MovimientosContainer.Children.Clear();
            NoMovimientosLabel.IsVisible = false;

            var movimientos = await _movimientoController.ObtenerMovimientosPorCuenta(numeroCuenta);

            if (movimientos == null || !movimientos.Any())
            {
                NoMovimientosLabel.IsVisible = true;
                return;
            }

            foreach (var movimiento in movimientos)
            {
                await ShowCustomAlert("Info", movimiento.CodigoCuenta);
                var movimientoView = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Padding = new Thickness(12),
                    Margin = new Thickness(0, 0, 0, 8),
                    BackgroundColor = Color.FromArgb("#C9BAE7")
                };

                var leftSection = new StackLayout
                {
                    Orientation = StackOrientation.Vertical
                };

                leftSection.Children.Add(new Label
                {
                    Text = $"Cuenta: {movimiento.CodigoCuenta}",
                    FontFamily = "FredokaBold",
                    TextColor = Colors.Black,
                    FontSize = 14
                });

                leftSection.Children.Add(new Label
                {
                    Text = $"Fecha: {movimiento.FechaMovimiento:dd/MM/yyyy}",
                    FontFamily = "FredokaRegular",
                    TextColor = Color.FromArgb("#4E4D4D"),
                    FontSize = 12
                });

                leftSection.Children.Add(new Label
                {
                    Text = $"Número Movimiento: {movimiento.NumeroMovimiento}",
                    FontFamily = "FredokaRegular",
                    TextColor = Color.FromArgb("#4E4D4D"),
                    FontSize = 12
                });

                var rightSection = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    HorizontalOptions = LayoutOptions.EndAndExpand
                };

                rightSection.Children.Add(new Label
                {
                    Text = $"Tipo: {movimiento.TipoDescripcion}",
                    FontFamily = "FredokaRegular",
                    TextColor = Color.FromArgb("#4E4D4D"),
                    FontSize = 12
                });

                string accion = movimiento.ImporteMovimiento >= 0 ? "Crédito" : "Débito";
                rightSection.Children.Add(new Label
                {
                    Text = $"Acción: {accion}",
                    FontFamily = "FredokaRegular",
                    TextColor = Color.FromArgb("#4E4D4D"),
                    FontSize = 12
                });

                rightSection.Children.Add(new Label
                {
                    Text = $"Importe: ${Math.Abs(movimiento.ImporteMovimiento):F2}",
                    FontFamily = "FredokaBold",
                    TextColor = Colors.Black,
                    FontSize = 14
                });

                movimientoView.Children.Add(leftSection);
                movimientoView.Children.Add(rightSection);

                MovimientosContainer.Children.Add(movimientoView);
            }
        }

        private async void OnAgregarClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(new CuentaView()));
        }
    }
}
