using System;
using System.Collections.Generic;

#nullable disable

namespace WSProcesSa.Models
{
    public partial class Empresa
    {
        public Empresa()
        {
            UnidadInternas = new HashSet<UnidadInterna>();
        }

        public string RutEmpresa { get; set; }
        public string RazonSocial { get; set; }
        public string GiroEmpresa { get; set; }
        public string DireccionEmpresa { get; set; }
        public decimal NumberoTelefono { get; set; }
        public string CorreoElectronicoEmpresa { get; set; }

        public virtual ICollection<UnidadInterna> UnidadInternas { get; set; }
    }
}
