using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WSProcesSa.Models.Response
{
	public class UsuarioResponse
	{
		public string Nombre { get; set; }
		public string Apellido { get; set; }
		public decimal Rol { get; set; }
		public string Email { get; set; }
		public string Token { get; set; }
	}
}
