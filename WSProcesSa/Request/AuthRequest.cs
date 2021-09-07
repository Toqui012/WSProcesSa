using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WSProcesSa.Request
{
    public class AuthRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public AuthRequest()
        {

        }

        public AuthRequest(Request.AuthRequest authRequest) 
        {
            this.Email = authRequest.Email;
            this.Password = authRequest.Password;
        }
    }
}
