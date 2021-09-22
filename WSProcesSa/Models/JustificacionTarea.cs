using System;
using System.Collections.Generic;

#nullable disable

namespace WSProcesSa.Models
{
    public partial class JustificacionTarea
    {
        public JustificacionTarea()
        {
            Tareas = new HashSet<Tarea>();
        }

        public decimal IdJustificacion { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Tarea> Tareas { get; set; }
    }
}
