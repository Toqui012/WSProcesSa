using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSProcesSa.Models;

namespace WSProcesSa.DTO
{
    public class EmpresaDTO
    {
        public string RutEmpresa { get; set; }
        public string RazonSocial { get; set; }
        public string GiroEmpresa { get; set; }
        public string DireccionEmpresa { get; set; }
        public decimal NumberoTelefono { get; set; }
        public string CorreoElectronicoEmpresa { get; set; }

        public EmpresaDTO()
        {

        }

        public EmpresaDTO(Models.Empresa business)
        {
            this.RutEmpresa = business.RutEmpresa;
            this.RazonSocial = business.RazonSocial;
            this.GiroEmpresa = business.GiroEmpresa;
            this.DireccionEmpresa = business.DireccionEmpresa;
            this.NumberoTelefono = business.NumberoTelefono;
            this.CorreoElectronicoEmpresa = business.CorreoElectronicoEmpresa;
        }
    }
}
