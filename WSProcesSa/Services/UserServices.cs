using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSProcesSa.Classes;
using WSProcesSa.Request;
using WSProcesSa.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using WSProcesSa.DTO;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace WSProcesSa.Services
{
    public class UserServices : IUserService
    {
        //Configuracion del Entorno
        private readonly IConfiguration config;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly AppSettings _appSettings;
        public UserServices(IConfiguration config, IWebHostEnvironment hostEnvironment, IOptions<AppSettings> appSettings) 
        {
            this.config = config;
            this.hostEnvironment = hostEnvironment;
            this._appSettings = appSettings.Value;
        }

        // Logica de autehnthificacion de usuario bajo el contexto de la base de datos
        public UserResponse Auth(AuthRequest authRequest)
        {
            List<Error> errors = new List<Error>();
            UserResponse userResponse = new UserResponse();

           using(ModelContext db = new ModelContext(config.GetConnectionString("Acceso"))) 
           {
                string spassword = Encrypt.GetSHA256(authRequest.Password);

                var user = db.Usuarios.Where(u => u.CorreoElectronico == authRequest.Email &&
                                                  u.Password == spassword).FirstOrDefault();
                if (user != null)
                {
                    userResponse.Email = user.CorreoElectronico;
                    userResponse.Role = user.IdRolUsuario;
                    userResponse.Nombre = user.NombreUsuario; 
                    userResponse.PrimerApellido = user.ApellidoUsuario;
                    userResponse.Token = GetToken(user);
                    return userResponse;
                }
                else
                {
                    return null;
                }

           }


        }

        private string GetToken(Usuario user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Key); //Se obtiene la key del cifrado
            var tokenDescriptor = new SecurityTokenDescriptor //Informacion que se guarda dentro del token
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.RutUsuario),
                    new Claim(ClaimTypes.Email, user.CorreoElectronico),
                    new Claim(ClaimTypes.Role, user.IdRolUsuario.ToString()),
                    new Claim(ClaimTypes.Name, user.NombreUsuario+" "+user.ApellidoUsuario),
                }),
                Expires = DateTime.UtcNow.AddDays(60), 
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
