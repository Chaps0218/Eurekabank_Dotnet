using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WS_EUREKABANK_SOAP_DOTNET.Models;

namespace WS_EUREKABANK_SOAP_DOTNET
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMovimientoService" in both code and config file together.
    [ServiceContract]
    public interface IMovimientoService
    {
        [OperationContract]
        List<CustomMovimientoModel> ObtenerMovimientosPorCuenta(string codigoCuenta);

        [OperationContract]
        void RegistrarMovimiento(CustomMovimientoModel movimiento);
    }
}
