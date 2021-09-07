using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WSProcesSa.DTO
{
    public class UserResponse
    {
        // Todo lo que se retorna al momento de iniciar sesion
        public string Email { get; set; }
        public decimal Role { get; set; }
        public string Token { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }


        public UserResponse() 
        {

        }


        public UserResponse(UserResponse userResponse) 
        {
            this.Nombre = userResponse.Nombre;
            this.PrimerApellido = userResponse.PrimerApellido;
            this.Email = userResponse.Email;
            this.Role = userResponse.Role;
            this.Token = userResponse.Token;
        }
    }
}
