using System;
using System.Collections.Generic;

#nullable disable

namespace WSProcesSa.Models
{
    public partial class EstadoTarea
    {
        public EstadoTarea()
        {
            Tareas = new HashSet<Tarea>();
        }

        public decimal IdEstadoTarea { get; set; }
        public string DescripcionEstado { get; set; }

        public virtual ICollection<Tarea> Tareas { get; set; }
    }
}
