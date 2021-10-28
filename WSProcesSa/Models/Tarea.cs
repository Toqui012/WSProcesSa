using System;
using System.Collections.Generic;

#nullable disable

namespace WSProcesSa.Models
{
    public partial class Tarea
    {
        public Tarea()
        {
            FlujoTareas = new HashSet<FlujoTarea>();
            JustificacionTareas = new HashSet<JustificacionTarea>();
            TareaSubordinada = new HashSet<TareaSubordinadum>();
        }

        public decimal IdTarea { get; set; }
        public string NombreTarea { get; set; }
        public string DescripcionTarea { get; set; }
        public DateTime FechaPlazo { get; set; }
        public string ReporteProblema { get; set; }
        public string AsignacionTarea { get; set; }
        public string FkRutUsuario { get; set; }
        public decimal? FkIdJustificacion { get; set; }
        public decimal FkEstadoTarea { get; set; }
        public decimal FkPrioridadTarea { get; set; }

        public virtual EstadoTarea FkEstadoTareaNavigation { get; set; }
        public virtual JustificacionTarea FkIdJustificacionNavigation { get; set; }
        public virtual PrioridadTarea FkPrioridadTareaNavigation { get; set; }
        public virtual Usuario FkRutUsuarioNavigation { get; set; }
        public virtual ICollection<FlujoTarea> FlujoTareas { get; set; }
        public virtual ICollection<JustificacionTarea> JustificacionTareas { get; set; }
        public virtual ICollection<TareaSubordinadum> TareaSubordinada { get; set; }
    }
}
