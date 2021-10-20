using System;
using System.Collections.Generic;

#nullable disable

namespace WSProcesSa.Models
{
    public partial class PrioridadTarea
    {
        public PrioridadTarea()
        {
            TareaSubordinada = new HashSet<TareaSubordinadum>();
            Tareas = new HashSet<Tarea>();
        }

        public decimal IdPrioridad { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<TareaSubordinadum> TareaSubordinada { get; set; }
        public virtual ICollection<Tarea> Tareas { get; set; }
    }
}
