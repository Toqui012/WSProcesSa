using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WSProcesSa.DTO
{
    public class UsuarioDTO
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

        public UsuarioDTO()
        {

        }

        public UsuarioDTO(Models.Usuario usuario)
        {
            this.RutUsuario = usuario.RutUsuario;
            this.NombreUsuario = usuario.NombreUsuario;
            this.SegundoNombre = usuario.SegundoNombre;
            this.ApellidoUsuario = usuario.ApellidoUsuario;
            this.SegundoNombre = usuario.SegundoApellido;
            this.NumTelefono = usuario.NumTelefono;
            this.CorreoElectronico = usuario.CorreoElectronico;
            this.Password = usuario.Password;
            this.IdRolUsuario = usuario.IdRolUsuario;
            this.IdUnidadInternaUsuario = usuario.IdUnidadInternaUsuario;
        }
    }
}
