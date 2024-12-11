using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WS_EUREKABANK_SOAP_DOTNET.Models;

namespace WS_EUREKABANK_SOAP_DOTNET
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MovimientoService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select MovimientoService.svc or MovimientoService.svc.cs at the Solution Explorer and start debugging.
    public class MovimientoService : IMovimientoService
    {
        public List<CustomMovimientoModel> ObtenerMovimientosPorCuenta(string codigoCuenta)
        {
            try
            {
                using (eurekabankEntities bd = new eurekabankEntities())
                {
                    var query = from m in bd.Movimientoes
                                where m.chr_cuencodigo == codigoCuenta
                                orderby m.int_movinumero descending
                                select new CustomMovimientoModel
                                {
                                    CodigoCuenta = m.chr_cuencodigo,
                                    NumeroMovimiento = m.int_movinumero,
                                    FechaMovimiento = m.dtt_movifecha,
                                    CodigoEmpleado = m.chr_emplcodigo,
                                    CodigoTipoMovimiento = m.chr_tipocodigo,
                                    ImporteMovimiento = (double)m.dec_moviimporte
                                };
                    return query.ToList();
                }
            }
            catch (Exception ex)
            {
                // Log exception
                return new List<CustomMovimientoModel>();
            }
        }

        public void RegistrarMovimiento(CustomMovimientoModel movimiento)
        {
            using (eurekabankEntities bd = new eurekabankEntities())
            {
                var nuevoMovimiento = new Movimiento
                {
                    chr_cuencodigo = movimiento.CodigoCuenta,
                    int_movinumero = movimiento.NumeroMovimiento,
                    dtt_movifecha = movimiento.FechaMovimiento,
                    chr_emplcodigo = movimiento.CodigoEmpleado,
                    chr_tipocodigo = movimiento.CodigoTipoMovimiento,
                    dec_moviimporte = (decimal)movimiento.ImporteMovimiento
                };
                bd.Movimientoes.Add(nuevoMovimiento);
                bd.SaveChanges();
            }
        }

    }
}
