using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WSProcesSa.DTO
{
    public class TareaDTO
    {
        public decimal IdTarea { get; set; }
        public string NombreTarea { get; set; }
        public string DescripcionTarea { get; set; }
        public DateTime FechaPlazo { get; set; }
        public decimal PorcentajeAvance { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string CreadaPor { get; set; }
        public string FkRutUsuario { get; set; }
        public decimal FkEstadoTarea { get; set; }
        public decimal FkPrioridadTarea { get; set; }
        public string ReporteProblema { get; set; }

        public TareaDTO(Models.Tarea tarea)
        {
            this.IdTarea = tarea.IdTarea;
            this.NombreTarea = tarea.NombreTarea;
            this.DescripcionTarea = tarea.DescripcionTarea;
            this.FechaPlazo = tarea.FechaPlazo;
            this.PorcentajeAvance = tarea.PorcentajeAvance;
            this.FechaCreacion = tarea.FechaCreacion;
            this.CreadaPor = tarea.CreadaPor;
            this.FkRutUsuario = tarea.FkRutUsuario;
            this.FkEstadoTarea = tarea.FkEstadoTarea;
            this.FkPrioridadTarea = tarea.FkPrioridadTarea;
            this.ReporteProblema = tarea.ReporteProblema;
        }

    }
}
