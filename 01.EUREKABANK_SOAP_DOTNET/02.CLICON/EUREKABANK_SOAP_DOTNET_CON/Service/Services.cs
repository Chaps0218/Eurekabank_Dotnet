using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUREKABANK_SOAP_DOTNET_CON.Service
{
    public class Services
    {
        private ServiceReference2.LoginServiceClient _loginService;
        private ServiceReference3.CuentaServiceClient _cuentaService;
        private ServiceReference1.MovimientoServiceClient _movimientoService;

        public Services()
        {
            _loginService = new ServiceReference2.LoginServiceClient();
            _cuentaService = new ServiceReference3.CuentaServiceClient();
            _movimientoService = new ServiceReference1.MovimientoServiceClient();
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            return await _loginService.LoginAsync(username, password);
        }

        public async Task<bool> PerformTransactionAsync(string accountCode, double amount, string transactionType, string destinationAccount = null)
        {
            string amountStr = amount.ToString("F2");
            string type = transactionType.ToLower() switch
            {
                "dep" => "DEP",
                "ret" => "RET",
                "tra" => "TRA",
                _ => throw new ArgumentException("Invalid transaction type")
            };

            return await _cuentaService.ActualizarSaldoYRegistrarMovimientoAsync(
                accountCode,
                amountStr,
                type,
                destinationAccount ?? ""
            );
        }

        public async Task<ServiceReference1.CustomMovimientoModel[]> GetAccountMovementsAsync(string accountCode)
        {
            return await _movimientoService.ObtenerMovimientosPorCuentaAsync(accountCode);
        }
    }
}
