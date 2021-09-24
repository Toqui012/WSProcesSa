using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WSProcesSa.DTO
{
    public class EstadoTareaDTO
    {
        public decimal IdEstadoTarea { get; set; }
        public string DescripcionEstadoTarea { get; set; }

        public EstadoTareaDTO()
        {

        }

        public EstadoTareaDTO(Models.EstadoTarea estadoTarea)
        {
            this.IdEstadoTarea = estadoTarea.IdEstadoTarea;
            this.DescripcionEstadoTarea = estadoTarea.DescripcionEstado;
        }

    }
}
