using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CuentaService;

namespace EUREKABANK_SOAP_DOTNET_CLIMOV.Controller
{
    public class CuentaController
    {
        private CuentaServiceClient _serviceClient;

        public CuentaController()
        {
            _serviceClient = new CuentaServiceClient();
        }

        public async Task<bool> ProcesarCuentaAsync(string codigoCuenta, string valorMovimiento, string tipo, string cuentaDest)
        {
            return await _serviceClient.ActualizarSaldoYRegistrarMovimientoAsync(codigoCuenta, valorMovimiento, tipo, cuentaDest);
        }
    }
}
