using System;
using System.Collections.Generic;

#nullable disable

namespace WSProcesSa.Models
{
    public partial class UnidadInterna
    {
        public UnidadInterna()
        {
            Usuarios = new HashSet<Usuario>();
        }

        public decimal IdUnidadInterna { get; set; }
        public string NombreUnidad { get; set; }
        public string DescripcionUnidad { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
