using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WSProcesSa.Classes
{
    public class Response
    {
        public object Data { get; set; }
        public List<Error> Errors { get; set; }
        public object Meta { get; set; }

        public Response()
        {
            Errors = new List<Error>();
            Meta = new
            {
                copyright = "Portafolio 2021",
                authors = new string[]{
                    "Álvaro Peñaloza",
                }
            };
        }
    }
}
