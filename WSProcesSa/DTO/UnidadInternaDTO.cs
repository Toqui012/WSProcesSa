using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WSProcesSa.DTO
{
    public class UnidadInternaDTO
    {
        public decimal IdUnidadInterna { get; set; }
        public string NombreUnidad { get; set; }
        public string DescripcionUnidad { get; set; }

        public string FkRutEmpresa { get; set; }

        public UnidadInternaDTO() 
        {

        }

        public UnidadInternaDTO(Models.UnidadInterna unidadInterna) 
        {
            this.IdUnidadInterna = unidadInterna.IdUnidadInterna;
            this.NombreUnidad = unidadInterna.NombreUnidad;
            this.FkRutEmpresa = unidadInterna.FkRutEmpresa;
            this.DescripcionUnidad = unidadInterna.DescripcionUnidad;
        }

    }
}
