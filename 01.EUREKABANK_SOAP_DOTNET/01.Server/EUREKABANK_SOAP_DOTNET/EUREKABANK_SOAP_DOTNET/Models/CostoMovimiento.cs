//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EUREKABANK_SOAP_DOTNET.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CostoMovimiento
    {
        public string chr_monecodigo { get; set; }
        public decimal dec_costimporte { get; set; }
    
        public virtual Moneda Moneda { get; set; }
    }
}
