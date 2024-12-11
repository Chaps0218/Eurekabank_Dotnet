using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WS_EUREKABANK_SOAP_DOTNET
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICuentaService" in both code and config file together.
    [ServiceContract]
    public interface ICuentaService
    {
        [OperationContract]
        bool ActualizarSaldoYRegistrarMovimiento(string codigoCuenta, string valorMovimiento, string tipo, string cuentaDest);
    }
}
