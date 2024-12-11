using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovimientoService;

namespace EUREKABANK_SOAP_DOTNET_CLIMOV.Controller
{
    public class MovimientoController
    {
        private MovimientoServiceClient _serviceClient;

        public MovimientoController()
        {
            _serviceClient = new MovimientoServiceClient();
        }

        public async Task<List<CustomMovimientoModel>> ObtenerMovimientosPorCuenta(string codigoCuenta)
        {
            try
            {
                var movimientosArray = await _serviceClient.ObtenerMovimientosPorCuentaAsync(codigoCuenta);
                return movimientosArray.ToList();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error: {ex.Message}");
                return new List<CustomMovimientoModel>();
            }
        }

        public async Task RegistrarMovimiento(CustomMovimientoModel movimiento)
        {
            try
            {
                await _serviceClient.RegistrarMovimientoAsync(movimiento);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
