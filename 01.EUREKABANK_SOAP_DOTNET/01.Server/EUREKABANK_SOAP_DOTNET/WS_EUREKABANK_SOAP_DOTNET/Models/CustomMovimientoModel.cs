using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace WS_EUREKABANK_SOAP_DOTNET.Models
{
    [DataContract]
    public class CustomMovimientoModel
    {
        [DataMember]
        public string CodigoCuenta { get; set; }

        [DataMember]
        public int NumeroMovimiento { get; set; }

        [DataMember]
        public DateTime FechaMovimiento { get; set; }

        [DataMember]
        public string CodigoEmpleado { get; set; }

        [DataMember]
        public string CodigoTipoMovimiento { get; set; }

        [DataMember]
        public string TipoDescripcion { get; set; }

        [DataMember]
        public double ImporteMovimiento { get; set; }

        [DataMember]
        public string CuentaReferencia { get; set; }

        // Parameterless constructor required for WCF serialization
        public CustomMovimientoModel() { }

        public CustomMovimientoModel(string codigoCuenta, int numeroMovimiento, DateTime fechaMovimiento, string codigoEmpleado, string codigoTipoMovimiento, string tipoDescripcion, double importeMovimiento, string cuentaReferencia)
        {
            CodigoCuenta = codigoCuenta;
            NumeroMovimiento = numeroMovimiento;
            FechaMovimiento = fechaMovimiento;
            CodigoEmpleado = codigoEmpleado;
            CodigoTipoMovimiento = codigoTipoMovimiento;
            TipoDescripcion = tipoDescripcion;
            ImporteMovimiento = importeMovimiento;
            CuentaReferencia = cuentaReferencia;
        }
    }
}