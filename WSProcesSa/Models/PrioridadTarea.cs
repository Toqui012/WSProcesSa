using System;
using System.Collections.Generic;

#nullable disable

namespace WSProcesSa.Models
{
    public partial class PrioridadTarea
    {
        public PrioridadTarea()
        {
            Tareas = new HashSet<Tarea>();
        }

        public decimal IdPrioridad { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Tarea> Tareas { get; set; }
    }
}
