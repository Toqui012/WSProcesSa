using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WSProcesSa.DTO
{
    public class JustificacionDTO
    {
        public decimal IdJustificacion { get; set; }
        public string Descripcion { get; set; }

        public JustificacionDTO()
        {

        }

        public JustificacionDTO(Models.JustificacionTarea justificacionTarea) 
        {
            this.IdJustificacion = justificacionTarea.IdJustificacion;
            this.Descripcion = justificacionTarea.Descripcion;
        }
    }
}
