using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WSProcesSa.Classes
{
    public class Error
    {
        public int Id { get; set; }
        public object Links { get; set; }
        public string Status { get; set; }
        public int Code { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }

        public Error()
        {
            Id = 1;
            Status = "";
            Code = 0;
            Title = "";
            Detail = "";
        }
    }
}
