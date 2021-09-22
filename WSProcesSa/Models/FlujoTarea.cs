using System;
using System.Collections.Generic;

#nullable disable

namespace WSProcesSa.Models
{
    public partial class FlujoTarea
    {
        public decimal IdFlujoTarea { get; set; }
        public string NombreFlujoTarea { get; set; }
        public string DescripcionFlujoTarea { get; set; }
        public decimal? FkIdTarea { get; set; }

        public virtual Tarea FkIdTareaNavigation { get; set; }
    }
}
