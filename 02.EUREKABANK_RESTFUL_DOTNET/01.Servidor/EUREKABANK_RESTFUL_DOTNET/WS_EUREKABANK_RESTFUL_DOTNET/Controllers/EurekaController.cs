using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WS_EUREKABANK_RESTFUL_DOTNET.Models;

namespace WS_EUREKABANK_RESTFUL_DOTNET.Controllers
{
    public class EurekaController : Controller
    {
        private eurekabankEntities _context = new eurekabankEntities();

        [HttpGet]
        public JsonResult LeerMovimientos(string cuenta)
        {
            var movimientos = _context.Movimientoes
                .Where(m => m.chr_cuencodigo == cuenta)
                .Select(m => new
                {
                    Cuenta = m.chr_cuencodigo,
                    NroMov = m.int_movinumero,
                    Fecha = m.dtt_movifecha,
                    Tipo = m.TipoMovimiento.vch_tipodescripcion,
                    Accion = m.TipoMovimiento.vch_tipoaccion,
                    Importe = m.dec_moviimporte
                }).ToList();

            return Json(movimientos, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string ProcesarMovimiento()
        {
            try
            {
                // Read the request body
                using (var reader = new System.IO.StreamReader(Request.InputStream))
                {
                    var requestBody = reader.ReadToEnd();
                    var request = Newtonsoft.Json.JsonConvert.DeserializeObject<MovimientoRequest>(requestBody);

                    double importe = double.Parse(request.ValorMovimiento);
                    string codTipo = "";

                    if (request.Tipo.Equals("RET", StringComparison.OrdinalIgnoreCase))
                    {
                        codTipo = "004";
                        if (!ActualizarSaldoCuenta(request.CodigoCuenta, (decimal)-importe))
                            return "No retiro";
                    }
                    else if (request.Tipo.Equals("DEP", StringComparison.OrdinalIgnoreCase))
                    {
                        codTipo = "003";
                        if (!ActualizarSaldoCuenta(request.CodigoCuenta, (decimal)importe))
                            return "No depósito";
                    }
                    else if (request.Tipo.Equals("TRA", StringComparison.OrdinalIgnoreCase))
                    {
                        codTipo = "009";
                        if (string.IsNullOrEmpty(request.CuentaDest))
                            throw new ArgumentException("Cuenta destino es obligatoria para transferencias.");

                        // Deduct from source account
                        if (!ActualizarSaldoCuenta(request.CodigoCuenta, (decimal)-importe))
                            return "No tra1";

                        // Add to destination account
                        if (!ActualizarSaldoCuenta(request.CuentaDest, (decimal)importe))
                        {
                            // Rollback source account update
                            ActualizarSaldoCuenta(request.CodigoCuenta, (decimal)importe);
                            return "No tra2";
                        }
                    }
                    else
                    {
                        throw new ArgumentException($"Tipo de movimiento no soportado: {request.Tipo}");
                    }

                    // Register movement for source account
                    int numeroMovimiento = ObtenerSiguienteNumeroMovimiento(request.CodigoCuenta);
                    var movimiento = new Movimiento
                    {
                        chr_cuencodigo = request.CodigoCuenta,
                        int_movinumero = numeroMovimiento,
                        dtt_movifecha = DateTime.Now,
                        chr_emplcodigo = "0001",
                        chr_tipocodigo = codTipo,
                        dec_moviimporte = (decimal)importe
                    };
                    RegistrarMovimiento(movimiento);

                    if (request.Tipo.Equals("TRA", StringComparison.OrdinalIgnoreCase))
                    {
                        int numeroMovimientoDest = ObtenerSiguienteNumeroMovimiento(request.CuentaDest);
                        var movimientoDest = new Movimiento
                        {
                            chr_cuencodigo = request.CuentaDest,
                            int_movinumero = numeroMovimientoDest,
                            dtt_movifecha = DateTime.Now,
                            chr_emplcodigo = "0001",
                            chr_tipocodigo = "008",
                            dec_moviimporte = (decimal)importe
                        };
                        RegistrarMovimiento(movimientoDest);
                    }

                    return "Tutobenne";
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return ex.ToString();
            }
        }



        public void RegistrarMovimiento(Movimiento movimiento)
        {
            using (eurekabankEntities bd = new eurekabankEntities())
            {
                var nuevoMovimiento = new Movimiento
                {
                    chr_cuencodigo = movimiento.chr_cuencodigo,
                    int_movinumero = movimiento.int_movinumero,
                    dtt_movifecha = movimiento.dtt_movifecha,
                    chr_emplcodigo = movimiento.chr_emplcodigo,
                    chr_tipocodigo = movimiento.chr_tipocodigo,
                    dec_moviimporte = movimiento.dec_moviimporte
                };
                bd.Movimientoes.Add(nuevoMovimiento);
                bd.SaveChanges();
            }
        }
        private int ObtenerSiguienteNumeroMovimiento(string codigoCuenta)
        {
            var movimientos = _context.Movimientoes
                .Where(m => m.chr_cuencodigo == codigoCuenta)
                .OrderByDescending(m => m.int_movinumero)
                .FirstOrDefault();

            return movimientos != null ? movimientos.int_movinumero + 1 : 1;
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
                return bd.Cuentas.SingleOrDefault(c => c.chr_cuencodigo == codigoCuenta);
            }
        }
    }
}