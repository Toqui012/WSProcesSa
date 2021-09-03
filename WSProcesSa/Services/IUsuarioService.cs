using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSProcesSa.Models.Request;
using WSProcesSa.Models.Response;

namespace WSProcesSa.Services
{
	public interface IUsuarioService
	{
		UsuarioResponse Auth(AuthRequest model);
	}
}
