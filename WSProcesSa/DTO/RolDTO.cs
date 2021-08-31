using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WSProcesSa.DTO
{
    public partial class RolDTO
    {
        public decimal RolId { get; set; }
        public string NombreRol { get; set; }
        public string Descripcion { get; set; }

        public RolDTO()
        {

        }

        public RolDTO(Models.Rol rol)
        {
            this.RolId = rol.IdRol;
            this.NombreRol = rol.NombreRol;
            this.Descripcion = rol.DescripcionRol;
        }
    }
}
