using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSProcesSa.Models;
using WSProcesSa.DTO;
using WSProcesSa.Classes;
using WSProcesSa.Models.Request;
using WSProcesSa.Services;
using WSProcesSa.Models.Response;
using Microsoft.AspNetCore.Authorization;

namespace WSProcesSa.Controllers
{
    [Route("api/usuario")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private IUsuarioService _usuarioService;
        private readonly IConfiguration config;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly IUrlHelper helper;

        public UsuarioController(IUsuarioService usuarioService, IConfiguration config, IWebHostEnvironment hostEnvironment)
        {
            _usuarioService = usuarioService;
            this.config = config;
            this.hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public ActionResult GetUser()
        {
            try
            {
                using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
                {
                    List<UsuarioDTO> response = db.Usuarios.Select(u => new UsuarioDTO(u)).ToList();
                    if (response.Count == 0)
                    {
                        return NotFound(new Response()
                        {
                            Data = response,
                            Errors = new List<Error>()
                            {
                                new Error()
                                {
                                    Id = 1,
                                    Status = "Not Found",
                                    Code = 404 ,
                                    Title = "No Data Found",
                                    Detail = "There is no data on database"
                                }
                            }
                        });
                    }
                    else
                    {
                        return Ok(new Response() { Data = response });
                    }
                }
            }
            catch (Exception err)
            {
                Response response = new Response();
                response.Errors.Add(new Error()
                {
                    Id = 1,
                    Status = "Internal Server Error",
                    Code = 500,
                    Title = err.Message,
                    Detail = err.InnerException != null ? err.InnerException.ToString() : err.Message
                });
                return StatusCode(500, response);
            }
        }

        [HttpPost]
        [Route("add")]
        public ActionResult AddUser([FromBody] Usuario newUserToAdd)
        {
            using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
            {
                try
                {
                    List<Error> errors = new List<Error>();
                    if (!db.Usuarios.Any(user => user.RutUsuario == newUserToAdd.RutUsuario))
                    {
                        if (newUserToAdd.RutUsuario.Length > 12 || newUserToAdd.RutUsuario.Length < 8)
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'RutUsuario'",
                                Detail = "The Field 'RutUsuarop' can't be null or white space"
                            });
                        }

                        if (string.IsNullOrWhiteSpace(newUserToAdd.NombreUsuario))
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'NombreRol'",
                                Detail = "The field 'NombreUsuario' can't be null or white space"
                            });
                        }

                        if (string.IsNullOrWhiteSpace(newUserToAdd.SegundoNombre))
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'SegundoNombre'",
                                Detail = "The field 'SegundoNombre' can't be null or white space"
                            });
                        }

                        if (string.IsNullOrWhiteSpace(newUserToAdd.ApellidoUsuario))
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'ApellidoUsuario'",
                                Detail = "The field 'ApellidoUsuario' can't be null or white space"
                            });
                        }

                        if (string.IsNullOrWhiteSpace(newUserToAdd.SegundoApellido))
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'SegundoApellido'",
                                Detail = "The field 'SegundoApellido' can't be null or white space"
                            });
                        }

                        if (string.IsNullOrWhiteSpace(newUserToAdd.CorreoElectronico))
                        {
                             errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'ApellidoUsuario'",
                                Detail = "The field 'ApellidoUsuario' can't be null or white space"
                            });
                        }

                        if (newUserToAdd.NumTelefono.ToString().Length != 9)
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'NumTelefono'",
                                Detail = "The field 'NumTelefono' can't be null or white space"
                            });
                        }

                        if (string.IsNullOrWhiteSpace(newUserToAdd.Password))
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'Password'",
                                Detail = "The field 'Password' can't be null or white space"
                            });
                        }

                        if (newUserToAdd.IdRolUsuario < 1)
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'IdRolUsuario'",
                                Detail = "The field 'IdRolUsuario' cannot be less than 0"
                            });
                        }

                        if (newUserToAdd.IdUnidadInternaUsuario < 1)
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'IdUnidadInternaUsuario'",
                                Detail = "The field 'IdUnidadInternaUsuario' cannot be less than 0"
                            });
                        }

                        if (errors.Count == 0)
                        {
                            Usuario userToAdd = new Usuario()
                            {
                                RutUsuario = newUserToAdd.RutUsuario,
                                NombreUsuario = newUserToAdd.NombreUsuario,
                                SegundoNombre = newUserToAdd.SegundoNombre,
                                ApellidoUsuario = newUserToAdd.ApellidoUsuario,
                                SegundoApellido = newUserToAdd.SegundoApellido,
                                NumTelefono = newUserToAdd.NumTelefono,
                                CorreoElectronico = newUserToAdd.CorreoElectronico,
                                Password = newUserToAdd.Password,
                                IdRolUsuario = newUserToAdd.IdRolUsuario,
                                IdUnidadInternaUsuario = newUserToAdd.IdUnidadInternaUsuario
                            };

                            db.Usuarios.Add(userToAdd);
                            db.SaveChanges();
                            return Created($"/detail/{newUserToAdd.RutUsuario}", new Response()
                            {
                                Data = new UsuarioDTO(userToAdd)
                            });
                        }
                        else
                        {
                            Response response = new Response();
                            response.Errors.Add(new Error()
                            {
                                Id = 1,
                                Status = "Bad Request",
                                Code = 404,
                                Title = "Not Found",
                                Detail = "Not Found"
                            });
                            return BadRequest(response);
                        }

                    }
                    else
                    {
                        Response response = new Response();
                        response.Errors.Add(new Error()
                        {
                            Id = 1,
                            Status = "Bad Request",
                            Code = 400,
                            Title = "The Rol already exists",
                            Detail = "The Rol already exists in the database"
                        });
                        return BadRequest(response);
                    }
                }
                catch (Exception err)
                {
                    Response response = new Response();
                    response.Errors.Add(new Error()
                    {
                        Id = 1,
                        Status = "Internal Server Error",
                        Code = 500,
                        Title = err.Message,
                        Detail = err.InnerException != null ? err.InnerException.ToString() : err.Message
                    });
                    return StatusCode(500, response);
                }
            }
        }

        [HttpDelete]
        [Route("delete/{rut}")]
        /// <summary>
        /// Elimina un usuario de la base de datos
        /// </summary>
        /// <param name="rut">Id del usuario</param>
        /// <returns>Retorna la id del usuario eliminado eliminado</returns>

        public ActionResult DeleteUser(string rut)
        {
            try
            {
                using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
                {
                    Usuario usuario = db.Usuarios.Where(user => user.RutUsuario == rut).FirstOrDefault();

                    if (usuario != null)
                    {
                        db.Usuarios.Remove(usuario);
                        db.SaveChanges();

                        return Ok(new Response()
                        {
                            Data = new { deletedId = rut }
                        });
                    }
                    else
                    {
                        return NotFound(new Response()
                        {
                            Errors = new List<Error>()
                            {
                                new Error()
                                {
                                    Id = 1,
                                    Status = "Not Found",
                                    Code = 404,
                                    Title = "No Data Found",
                                    Detail = "Couldn't find the User."
                                }
                            }
                        });
                    }
                }
            }
            catch (Exception err)
            {
                Response response = new Response();
                response.Errors.Add(new Error()
                {
                    Id = 1,
                    Status = "Internal Server Error",
                    Code = 500,
                    Title = err.Message,
                    Detail = err.InnerException != null ? err.InnerException.ToString() : err.Message
                });
                return StatusCode(500, response);
            }
        }

        [HttpPut]
        [Route("update/{rut}")]
        /// <summary>
        /// Actualiza los datos de un User
        /// </summary>
        /// <param name="rut">Id del usuario para actualizar</param>
        /// <param name="user">Objeto con los datos del Usuario actualizados</param>
        /// <returns>Retorna un objeto con los datos del Usuario actualizados</returns>
        /// 

        public ActionResult UpdateRole(string rut, [FromBody] Usuario user)
        {
            try
            {
                using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
                {
                    List<Error> errors = new List<Error>();
                    Usuario userUpdated = db.Usuarios.Where(u => u.RutUsuario == rut).FirstOrDefault();
                    if (userUpdated != null)
                    {
                        if (string.IsNullOrWhiteSpace(userUpdated.NombreUsuario))
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'NombreRol'",
                                Detail = "The field 'NombreUsuario' can't be null or white space"
                            });
                        }
                        else
                        {
                            userUpdated.NombreUsuario = user.NombreUsuario;
                        }

                        if (string.IsNullOrWhiteSpace(userUpdated.SegundoNombre))
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'SegundoNombre'",
                                Detail = "The field 'SegundoNombre' can't be null or white space"
                            });
                        }
                        else
                        {
                            userUpdated.SegundoNombre = user.SegundoNombre;
                        }

                        if (string.IsNullOrWhiteSpace(userUpdated.ApellidoUsuario))
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'ApellidoUsuario'",
                                Detail = "The field 'ApellidoUsuario' can't be null or white space"
                            });
                        }
                        else
                        {
                            userUpdated.ApellidoUsuario = user.ApellidoUsuario;
                        }

                        if (string.IsNullOrWhiteSpace(userUpdated.SegundoApellido))
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'SegundoApellido'",
                                Detail = "The field 'SegundoApellido' can't be null or white space"
                            });
                        }
                        else
                        {
                            userUpdated.SegundoApellido = user.SegundoApellido;
                        }

                        if (string.IsNullOrWhiteSpace(userUpdated.CorreoElectronico))
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'CorreoElectronico'",
                                Detail = "The field 'CorreoElectronico' can't be null or white space"
                            });
                        }
                        else
                        {
                            userUpdated.CorreoElectronico = user.CorreoElectronico;
                        }

                        if (userUpdated.NumTelefono.ToString().Length != 9)
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'NumTelefono'",
                                Detail = "The field 'NumTelefono' can't be null or white space"
                            });
                        }
                        else
                        {
                            userUpdated.NumTelefono = user.NumTelefono;
                        }

                        if (string.IsNullOrWhiteSpace(userUpdated.Password))
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'Password'",
                                Detail = "The field 'Password' can't be null or white space"
                            });
                        }
                        else
                        {
                            userUpdated.Password = user.Password;
                        }

                        if (userUpdated.IdRolUsuario < 1)
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'IdRolUsuario'",
                                Detail = "The field 'IdRolUsuario' cannot be less than 0"
                            });
                        }
                        else
                        {
                            userUpdated.IdRolUsuario = user.IdRolUsuario;
                        }

                        if (userUpdated.IdUnidadInternaUsuario < 1)
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'IdUnidadInternaUsuario'",
                                Detail = "The field 'IdUnidadInternaUsuario' cannot be less than 0"
                            });
                        }
                        else
                        {
                            userUpdated.IdUnidadInternaUsuario = user.IdUnidadInternaUsuario;
                        }

                        db.SaveChanges();

                        return Ok(new Response()
                        {
                            Data = new UsuarioDTO(userUpdated),
                            Errors = errors
                        });
                    }
                    else
                    {
                        return NotFound(new Response()
                        {
                            Errors = new List<Error>()
                            {
                                new Error()
                                {
                                    Id = 1,
                                    Status = "Not Found",
                                    Code = 404,
                                    Title = "No Data Found",
                                    Detail = "Couldn´t find the user"
                                }
                            }
                        });
                    }
                }
            }
            catch (Exception err)
            {
                Response response = new Response();
                response.Errors.Add(new Error()
                {
                    Id = 1,
                    Status = "Internal Server Error",
                    Code = 500,
                    Title = err.Message,
                    Detail = err.InnerException != null ? err.InnerException.ToString() : err.Message
                });
                return StatusCode(500, response);
            }
        }



        [HttpPost("login")]
        public IActionResult Autentificar([FromBody] AuthRequest model)

        {
            Respuesta  respuesta = new Respuesta();
            var userResponse = _usuarioService.Auth(model);

            if(userResponse == null)
			{
                respuesta.Exito = 0;
                respuesta.Mensaje = "Usuario o Contraseña Incorrecta";
                return BadRequest(respuesta);
			}


            respuesta.Exito = 1;
            respuesta.Data = userResponse;

            return Ok(respuesta);
        }
    }
}
