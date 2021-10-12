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
using Microsoft.AspNetCore.Authorization;

namespace WSProcesSa.Controllers
{
    [Route("api/rol")]
    [Authorize]
    [ApiController]
    public class RolController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly IUrlHelper helper;

        public RolController(IConfiguration config, IWebHostEnvironment hostEnvironment)
        {
            this.config = config;
            this.hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetRol()
        {
            try
            {
                using(ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
                {
                    List<RolDTO> response = db.Rols.Select(r => new RolDTO(r)).ToList();
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
        public async Task<IActionResult> AddRol([FromBody]Rol newRolToAdd)
        {
            using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
            {
                try
                {
                    List<Error> errors = new List<Error>();
                    if (!db.Rols.Any(rol => rol.IdRol == newRolToAdd.IdRol ||
                                            rol.NombreRol == newRolToAdd.NombreRol))
                    {
                        if (newRolToAdd.IdRol < 0)
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'IdRol'",
                                Detail = "The Field 'IdRol' cannot be less 0"
                            });
                        }

                        if (string.IsNullOrWhiteSpace(newRolToAdd.NombreRol))
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'NombreRol'",
                                Detail = "The field 'NombreRol' can't be null or white space"
                            });
                        }

                        if (string.IsNullOrWhiteSpace(newRolToAdd.DescripcionRol))
                        {
                            newRolToAdd.DescripcionRol = string.Empty;
                        }

                        if (errors.Count == 0)
                        {
                            Rol rolToAdd = new Rol()
                            {
                                NombreRol = newRolToAdd.NombreRol,
                                DescripcionRol = newRolToAdd.DescripcionRol,
                            };

                            db.Rols.Add(rolToAdd);
                            db.SaveChanges();
                            return Created($"/detail/{newRolToAdd.IdRol}", new Response()
                            {
                                Data = new RolDTO(rolToAdd)
                            });
                        }
                        else {
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
        [Route("delete/{id}")]
        /// <summary>
        /// Elimina un rol de la base de datos
        /// </summary>
        /// <param name="id">Id del rol</param>
        /// <returns>Retorna la id del rol eliminado</returns>

        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                using(ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
                {
                    Rol role = db.Rols.Where(rol => rol.IdRol == id).FirstOrDefault();

                    if (role != null)
                    {
                        db.Rols.Remove(role);
                        db.SaveChanges();

                        return Ok(new Response()
                        {
                            Data = new { deletedId = id }
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
                                    Detail = "Couldn't find the Role."
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
        [Route("update/{id}")]
        /// <summary>
        /// Actualiza los datos de un rol
        /// </summary>
        /// <param name="id">Id del rol para actualizar</param>
        /// <param name="role">Objeto con los datos del rol actualizados</param>
        /// <returns>Retorna un objeto con los datos del rol actualizados</returns>
        /// 

        public async Task<IActionResult> UpdateRole(int id, [FromBody] Rol role)
        {
            try
            {
                using(ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
                {
                    List<Error> errors = new List<Error>();
                    Rol roleUpdated = db.Rols.Where(rol => rol.IdRol == id).FirstOrDefault();
                    if (roleUpdated != null)
                    {
                        if (!string.IsNullOrWhiteSpace(role.NombreRol))
                        {
                            roleUpdated.NombreRol = role.NombreRol;
                        }
                        else
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid field: NombreRol",
                                Detail = "The field 'NombreRol'does not contain the required format."
                            });
                        }

                        if (!string.IsNullOrWhiteSpace(role.DescripcionRol))
                        {
                            roleUpdated.DescripcionRol = role.DescripcionRol;
                        }
                      
                        db.SaveChanges();

                        return Ok(new Response()
                        {
                            Data = new RolDTO(roleUpdated),
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
                                    Detail = "Couldn´t find the role"
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


    }
}
