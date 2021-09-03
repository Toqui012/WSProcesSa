using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WSProcesSa.Models;
using WSProcesSa.Models.Common;
using WSProcesSa.Models.Request;
using WSProcesSa.Models.Response;

namespace WSProcesSa.Services
{
	public class UsuarioService : IUsuarioService
	{

		private readonly AppSettings _appSettings;

		public UsuarioService(IOptions<AppSettings> appSettings)
		{
			_appSettings = appSettings.Value;
		}

		public UsuarioResponse Auth(AuthRequest model)
		{
			UsuarioResponse userResponse = new UsuarioResponse();
			//Busqueda en db
			using (var db = new ModelContext())
			{
				//Hasheo del Password
				//string spassword = Encrypt.GetSHA256(model.Password);

				//Password igual a la password del modelo
				string spassword = model.Password;

				//Buscamos el usuario en la base de datos
				var usuario = db.Usuarios.Where(d => d.Password == spassword).FirstOrDefault();

				if (usuario == null) return null;

				// Le pasamos el Nombre del Usuario
				userResponse.Nombre = usuario.NombreUsuario;
				// Le pasamos el Apellido del Usuario
				userResponse.Apellido = usuario.ApellidoUsuario;
				// Le pasamos el Rol asociado al Usuario
				userResponse.Rol = usuario.IdRolUsuario;
				// Email = Correo electronico
				userResponse.Email = usuario.CorreoElectronico;
				// Le pasamos el Token
				userResponse.Token = GetToken(usuario);
			}
			//Retornamos userResponse
			return userResponse;

		}

		private string GetToken(Usuario usuario)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			//Llave 
			var llave = Encoding.ASCII.GetBytes(_appSettings.Secreto);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(
							new Claim[]
							{
								new Claim(ClaimTypes.NameIdentifier, usuario.RutUsuario.ToString()),
								new Claim(ClaimTypes.NameIdentifier, usuario.CorreoElectronico)
							}
					),
				Expires = DateTime.UtcNow.AddDays(60),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(llave), SecurityAlgorithms.HmacSha256Signature)
			};

			ModelContext db = new ModelContext();

			// Regresamos el token en una variable
			var token = tokenHandler.CreateToken(tokenDescriptor);
			//var role = db.Usuarios.Where(x => x.NombreUsuario == "Dennisse").FirstOrDefault();
			return tokenHandler.WriteToken(token);

		}

	}
}
