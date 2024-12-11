using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WS_EUREKABANK_SOAP_DOTNET.Models;

namespace WS_EUREKABANK_SOAP_DOTNET
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CuentaService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CuentaService.svc or CuentaService.svc.cs at the Solution Explorer and start debugging.
    public class CuentaService : ICuentaService
    {
        private readonly MovimientoService movimientoService = new MovimientoService();

        public string ActualizarSaldoYRegistrarMovimiento(string codigoCuenta, string valorMovimiento, string tipo, string cuentaDest)
        {
            try
            {
                double importe = double.Parse(valorMovimiento);
                string codTipo = "";

                if (tipo.Equals("RET", StringComparison.OrdinalIgnoreCase))
                {
                    codTipo = "004";
                    if (!ActualizarSaldoCuenta(codigoCuenta, (decimal)-importe))
                        return "No retiro";
                }
                else if (tipo.Equals("DEP", StringComparison.OrdinalIgnoreCase))
                {
                    codTipo = "003";
                    if (!ActualizarSaldoCuenta(codigoCuenta, (decimal)importe))
                        return "No depósito";
                }
                else if (tipo.Equals("TRA", StringComparison.OrdinalIgnoreCase))
                {
                    codTipo = "009";
                    if (string.IsNullOrEmpty(cuentaDest))
                        throw new ArgumentException("Cuenta destino es obligatoria para transferencias.");

                    // Deduct from source account
                    if (!ActualizarSaldoCuenta(codigoCuenta, (decimal)-importe))
                        return "No tra1";

                    // Add to destination account
                    if (!ActualizarSaldoCuenta(cuentaDest, (decimal)importe))
                    {
                        // Rollback source account update
                        ActualizarSaldoCuenta(codigoCuenta, (decimal)importe);
                        return "No tra2";
                    }
                }
                else
                {
                    throw new ArgumentException($"Tipo de movimiento no soportado: {tipo}");
                }

                // Register movement for source account
                int numeroMovimiento = ObtenerSiguienteNumeroMovimiento(codigoCuenta);
                var movimiento = new CustomMovimientoModel
                {
                    CodigoCuenta = codigoCuenta,
                    NumeroMovimiento = numeroMovimiento,
                    FechaMovimiento = DateTime.Now,
                    CodigoEmpleado = "0001",
                    CodigoTipoMovimiento = codTipo,
                    ImporteMovimiento = importe
                };
                movimientoService.RegistrarMovimiento(movimiento);

                // Register movement for destination account in case of transfer
                if (tipo.Equals("TRA", StringComparison.OrdinalIgnoreCase))
                {
                    int numeroMovimientoDest = ObtenerSiguienteNumeroMovimiento(cuentaDest);
                    var movimientoDest = new CustomMovimientoModel
                    {
                        CodigoCuenta = cuentaDest,
                        NumeroMovimiento = numeroMovimientoDest,
                        FechaMovimiento = DateTime.Now,
                        CodigoEmpleado = "0001",
                        CodigoTipoMovimiento = "008",
                        ImporteMovimiento = importe
                    };
                    movimientoService.RegistrarMovimiento(movimientoDest);
                }

                return "Tutobenne";
            }
            catch (Exception ex)
            {
                // Log the exception
                return ex.ToString();
            }
        }

        private int ObtenerSiguienteNumeroMovimiento(string codigoCuenta)
        {
            var movimientos = movimientoService.ObtenerMovimientosPorCuenta(codigoCuenta);
            int numeroMovimiento = 1;

            foreach (var movimiento in movimientos)
            {
                if (movimiento.NumeroMovimiento >= numeroMovimiento)
                {
                    numeroMovimiento = movimiento.NumeroMovimiento + 1;
                }
            }
            return numeroMovimiento;
        }

        public bool ActualizarSaldoCuenta(string codigoCuenta, decimal valorMovimiento)
        {
            using (var bd = new eurekabankEntities())
            {
                var cuenta = bd.Cuentas.SingleOrDefault(c => c.chr_cuencodigo == codigoCuenta);

                if (cuenta == null)
                {
                    return false;
                }

                // Update balance and increment movement counter
                cuenta.dec_cuensaldo += valorMovimiento;
                cuenta.int_cuencontmov++;

                try
                {
                    bd.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public Cuenta ObtenerCuentaPorCodigo(string codigoCuenta)
        {
            using (var bd = new eurekabankEntities())
            {
                // Directly return the found account, no need for manual mapping
                return bd.Cuentas.SingleOrDefault(c => c.chr_cuencodigo == codigoCuenta);
            }
        }
    }
}
