using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WSProcesSa.DTO
{
    public class TareaSubordinadaDTO
    {
        public decimal IdTareaSubordinada { get; set; }
        public string NombreSubordinada { get; set; }
        public string DescripcionSubordinada { get; set; }
        public decimal FkIdTarea { get; set; }

        public TareaSubordinadaDTO() 
        {

        }

        public TareaSubordinadaDTO(Models.TareaSubordinadum tareaSubordinada)
        {
            this.IdTareaSubordinada = tareaSubordinada.IdTareaSubordinada;
            this.NombreSubordinada = tareaSubordinada.NombreSubordinada;
            this.DescripcionSubordinada = tareaSubordinada.DescripcionSubordinada;
            this.FkIdTarea = tareaSubordinada.FkIdTarea;
        }
    }
}
