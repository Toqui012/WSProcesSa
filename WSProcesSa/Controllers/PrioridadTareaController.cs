using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSProcesSa.Models;
using WSProcesSa.Classes;
using WSProcesSa.DTO;

namespace WSProcesSa.Controllers
{
    [Route("api/prioridadTarea")]
    [Authorize]
    [ApiController]
    public class PrioridadTareaController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly IUrlHelper helper;

        public PrioridadTareaController(IConfiguration config, IWebHostEnvironment hostEnvironment)
        {
            this.config = config;
            this.hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso"))) 
                {
                    List<PrioridadTareaDTO> response = db.PrioridadTareas.Select(task => new PrioridadTareaDTO(task)).ToList();
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
                                    Code = 404,
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
        public async Task<IActionResult> AddTaskPriority([FromBody] PrioridadTarea newPrioridadTareaToAdd)
        {
            using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
            {
                try
                {
                    List<Error> errors = new List<Error>();
                    if (!db.PrioridadTareas.Any(task => task.IdPrioridad == newPrioridadTareaToAdd.IdPrioridad ||
                                                        task.Descripcion == newPrioridadTareaToAdd.Descripcion))
                    {
                        if (string.IsNullOrWhiteSpace(newPrioridadTareaToAdd.Descripcion))
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code= 400,
                                Title = "Invalid Field 'DescripcionPrioridad'",
                                Detail = "The Field 'DescripcionPrioridad' can´t be null or white space"
                            });
                        }

                        if (errors.Count == 0)
                        {
                            PrioridadTarea prioridadTarea = new PrioridadTarea()
                            {
                                Descripcion = newPrioridadTareaToAdd.Descripcion
                            };

                            db.PrioridadTareas.Add(prioridadTarea);
                            db.SaveChanges();
                            return Created($"/detail/{newPrioridadTareaToAdd.IdPrioridad}", new Response()
                            {
                                Data = new PrioridadTareaDTO(prioridadTarea)
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
                            Title = "The User already exists",
                            Detail = "The User already exists in the database"
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
        public async Task<IActionResult> DeletePriotidadTarea(int id)
        {
            try
            {
                using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
                {
                    PrioridadTarea prioridadTarea = db.PrioridadTareas.FirstOrDefault(task => task.IdPrioridad == id);

                    if (prioridadTarea != null)
                    {
                        db.PrioridadTareas.Remove(prioridadTarea);
                        db.SaveChanges();

                        return Ok(new Response()
                        {
                            Data = new { deletedId = id}
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
                                    Detail = "Couldn't find the task."
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
        public async Task<IActionResult> UpdatePrioridadTarea(int id, [FromBody] PrioridadTarea prioridadTarea)
        {
            try
            {
                using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
                {
                    List<Error> errors = new List<Error>();
                    PrioridadTarea prioridadTareaUpdated = db.PrioridadTareas.FirstOrDefault(task => task.IdPrioridad == id ||
                                                                                             task.Descripcion == prioridadTarea.Descripcion);
                    if (prioridadTareaUpdated != null)
                    {
                        if (string.IsNullOrWhiteSpace(prioridadTarea.Descripcion))
                        {
                            errors.Add(new Error()
                            {

                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'descripcion'",
                                Detail = "The field 'descripcion' can't be null or white space"
                            });
                        }
                        else
                        {
                            prioridadTareaUpdated.Descripcion = prioridadTarea.Descripcion;
                        }

                        db.SaveChanges();

                        return Ok(new Response()
                        {
                            Data = new PrioridadTareaDTO(prioridadTareaUpdated),
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
                                    Detail = "Couldn´t find the prioridadTarea"
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
