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
    [Route("api/flujoTarea")]
    [Authorize]
    [ApiController]
    public class FlujoTareasController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly IUrlHelper helper;

        public FlujoTareasController(IConfiguration config, IWebHostEnvironment hostEnvironment)
        {
            this.config = config;
            this.hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetFlujoTarea()
        {
            try
            {
                using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
                {
                    List<FlujoTareasDTO> response = db.FlujoTareas.Select(f => new FlujoTareasDTO(f)).ToList();
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
        public async Task<IActionResult> AddFlujoTareas([FromBody] FlujoTarea newFlujoToAdd)
        {
            using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
            {
                try
                {
                    List<Error> errors = new List<Error>();
                    if (!db.FlujoTareas.Any(f => f.IdFlujoTarea == newFlujoToAdd.IdFlujoTarea ||
                                                 f.NombreFlujoTarea == newFlujoToAdd.NombreFlujoTarea))
                    {
                        if (newFlujoToAdd.IdFlujoTarea < 0)
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'IdFlujoTarea'",
                                Detail = "The Field 'IdFlujoTarea' cannot be less 0"
                            });
                        }

                        if (string.IsNullOrWhiteSpace(newFlujoToAdd.NombreFlujoTarea))
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'NombreFlujoTarea'",
                                Detail = "The field 'NombreFlujoTarea' can't be null or white space"
                            });
                        }

                        if (string.IsNullOrWhiteSpace(newFlujoToAdd.DescripcionFlujoTarea))
                        {
                            newFlujoToAdd.DescripcionFlujoTarea = string.Empty;
                        }

                        if (newFlujoToAdd.FkIdTarea < 0)
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'FkIdTarea'",
                                Detail = "The field 'FkIdTarea' can´t be less than 0"
                            });
                        }

                        if (errors.Count == 0)
                        {
                            FlujoTarea FlujoToAdd = new FlujoTarea()
                            {
                                NombreFlujoTarea = newFlujoToAdd.NombreFlujoTarea,
                                DescripcionFlujoTarea = newFlujoToAdd.DescripcionFlujoTarea,
                            };

                            db.FlujoTareas.Add(FlujoToAdd);
                            db.SaveChanges();
                            return Created($"/detail/{newFlujoToAdd.IdFlujoTarea}", new Response()
                            {
                                Data = new FlujoTareasDTO(FlujoToAdd)
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
                            Title = "The Flujo Tareas already exists",
                            Detail = "The Flujo Tareas already exists in the database"
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
        /// Elimina un flujo de tareas de la base de datos
        /// </summary>
        /// <param name="id">Id del flujo</param>
        /// <returns>Retorna la id del flujo de tareas eliminado</returns>

        public async Task<IActionResult> DeleteFlujoTarea(int id)
        {
            try
            {
                using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
                {
                    FlujoTarea flujoTarea = db.FlujoTareas.Where(f => f.IdFlujoTarea == id).FirstOrDefault();

                    if (flujoTarea != null)
                    {
                        db.FlujoTareas.Remove(flujoTarea);
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
                                    Detail = "Couldn't find the Flujo Tarea."
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
        /// Actualiza los datos de un flujo de tareas
        /// </summary>
        /// <param name="id">Id del flujo de tareas para actualizar</param>
        /// <param name="role">Objeto con los datos del flujo de tareas actualizados</param>
        /// <returns>Retorna un objeto con los datos del flujo de tareas actualizados</returns>
        /// 

        public async Task<IActionResult> UpdateFlujoTareas(int id, [FromBody] FlujoTarea flujoTarea)
        {
            try
            {
                using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
                {
                    List<Error> errors = new List<Error>();
                    FlujoTarea flujoTareaUpdated = db.FlujoTareas.Where(f => f.IdFlujoTarea == id).FirstOrDefault();
                    if (flujoTareaUpdated != null)
                    {
                        if (!string.IsNullOrWhiteSpace(flujoTarea.NombreFlujoTarea))
                        {
                            flujoTareaUpdated.NombreFlujoTarea = flujoTarea.NombreFlujoTarea;
                        }
                        else
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid field: NombreFlujoTarea",
                                Detail = "The field 'NombreFlujoTarea'does not contain the required format."
                            });
                        }

                        if (!string.IsNullOrEmpty(flujoTarea.DescripcionFlujoTarea) || string.IsNullOrEmpty(flujoTareaUpdated.DescripcionFlujoTarea))
                        {
                            flujoTareaUpdated.DescripcionFlujoTarea = flujoTarea.DescripcionFlujoTarea;
                        }
                        else
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid field: DescripcionFlujoTarea",
                                Detail = "The field 'DescripcionFlujoTarea' does not contain the required format."
                            });
                        }

                        if (flujoTarea.FkIdTarea > 0)
                        {
                            flujoTareaUpdated.FkIdTarea = flujoTarea.FkIdTarea;
                        }
                        else
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field: FkIdTarea",
                                Detail = "The Field 'FkTarea' can´t be less than 0 "
                            });
                        } 

                        db.SaveChanges();

                        return Ok(new Response()
                        {
                            Data = new FlujoTareasDTO(flujoTareaUpdated),
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
                                    Detail = "Couldn´t find the flujoTarea"
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
