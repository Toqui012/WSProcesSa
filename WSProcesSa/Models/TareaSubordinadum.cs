using System;
using System.Collections.Generic;

#nullable disable

namespace WSProcesSa.Models
{
    public partial class TareaSubordinadum
    {
        public decimal IdTareaSubordinada { get; set; }
        public string NombreSubordinada { get; set; }
        public string DescripcionSubordinada { get; set; }
        public decimal FkIdTarea { get; set; }

        public virtual Tarea FkIdTareaNavigation { get; set; }
    }
}
