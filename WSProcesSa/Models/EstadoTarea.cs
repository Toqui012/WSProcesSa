using System;
using System.Collections.Generic;

#nullable disable

namespace WSProcesSa.Models
{
    public partial class EstadoTarea
    {
        public EstadoTarea()
        {
            TareaSubordinada = new HashSet<TareaSubordinadum>();
            Tareas = new HashSet<Tarea>();
        }

        public decimal IdEstadoTarea { get; set; }
        public string DescripcionEstado { get; set; }

        public virtual ICollection<TareaSubordinadum> TareaSubordinada { get; set; }
        public virtual ICollection<Tarea> Tareas { get; set; }
    }
}
