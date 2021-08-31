using System;
using System.Collections.Generic;

#nullable disable

namespace WSProcesSa.Models
{
    public partial class Usuario
    {
        public string RutUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string SegundoNombre { get; set; }
        public string ApellidoUsuario { get; set; }
        public string SegundoApellido { get; set; }
        public decimal NumTelefono { get; set; }
        public string CorreoElectronico { get; set; }
        public string Password { get; set; }
        public decimal IdRolUsuario { get; set; }
        public decimal IdUnidadInternaUsuario { get; set; }

        public virtual Rol IdRolUsuarioNavigation { get; set; }
        public virtual UnidadInterna IdUnidadInternaUsuarioNavigation { get; set; }
    }
}
