using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WSProcesSa.Models.Response
{
	public class UsuarioResponse
	{
		public decimal Rol { get; set; }
		public string Email { get; set; }
		public string Token { get; set; }
	}
}
