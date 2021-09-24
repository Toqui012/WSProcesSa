using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WSProcesSa.DTO
{
    public class PrioridadTareaDTO
    {
        public decimal IdPrioridad { get; set; }
        public string Descripcion { get; set; }

        public PrioridadTareaDTO()
        {

        }

        public PrioridadTareaDTO(Models.PrioridadTarea prioridadTarea)
        {
            this.IdPrioridad = prioridadTarea.IdPrioridad;
            this.Descripcion = prioridadTarea.Descripcion;
        }

    }
}
