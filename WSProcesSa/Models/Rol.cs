using System;
using System.Collections.Generic;

#nullable disable

namespace WSProcesSa.Models
{
    public partial class Rol
    {
        public Rol()
        {
            Usuarios = new HashSet<Usuario>();
        }

        public decimal IdRol { get; set; }
        public string NombreRol { get; set; }
        public string DescripcionRol { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
