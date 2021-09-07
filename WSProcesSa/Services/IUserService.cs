using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSProcesSa.Classes;
using WSProcesSa.DTO;
using WSProcesSa.Request;

namespace WSProcesSa.Services
{
    public interface IUserService
    {
        UserResponse Auth(AuthRequest authRequest); 
    }
}
