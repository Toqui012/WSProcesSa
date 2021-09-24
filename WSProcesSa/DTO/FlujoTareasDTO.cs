using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WSProcesSa.DTO
{
    public class FlujoTareasDTO
    {
        public decimal IdFlujoTarea { get; set; }
        public string NombreFlujoTarea { get; set; }
        public string DescripcionFlujoTarea { get; set; }
        public decimal? FkIdTarea { get; set; }

        public FlujoTareasDTO()
        {

        }

        public FlujoTareasDTO(Models.FlujoTarea flujoTarea)
        {
            this.IdFlujoTarea = flujoTarea.IdFlujoTarea;
            this.NombreFlujoTarea = flujoTarea.NombreFlujoTarea;
            this.DescripcionFlujoTarea = flujoTarea.DescripcionFlujoTarea;
            this.FkIdTarea = flujoTarea.FkIdTarea; 
        }
    }
}
