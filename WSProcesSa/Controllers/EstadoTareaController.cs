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
using WSProcesSa.DTO;
using WSProcesSa.Classes;

namespace WSProcesSa.Controllers
{
    [Route("api/estadoTarea")]
    [Authorize]
    [ApiController]
    public class EstadoTareaController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly IUrlHelper helper;

        public EstadoTareaController(IConfiguration config , IWebHostEnvironment hostEnvironment) 
        {
            this.config = config;
            this.hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetEstadoTarea()
        {
            try
            {
                using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso"))) 
                {
                    List<EstadoTareaDTO> response = db.EstadoTareas.Select(task => new EstadoTareaDTO(task)).ToList();
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
        public async Task<IActionResult> AddEstadoTarea([FromBody] EstadoTarea newEstadoTarea)
        {
            using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
            {
                try
                {
                    List<Error> errors = new List<Error>();
                    if (!db.EstadoTareas.Any(task => task.IdEstadoTarea == newEstadoTarea.IdEstadoTarea ||
                                                     task.DescripcionEstado == newEstadoTarea.DescripcionEstado))
                    {
                        if (newEstadoTarea.IdEstadoTarea < 0)
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'IdEstadoTarea'",
                                Detail = "The Field 'IdEstadoTarea' cannot be less 0"
                            });
                        }

                        if (string.IsNullOrWhiteSpace(newEstadoTarea.DescripcionEstado))
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'DescripcionEstadoTarea'",
                                Detail = "The field 'DescripcionEstadoTarea' can't be null or white space"
                            });
                        }

                        if (errors.Count == 0)
                        {
                            EstadoTarea estadoTareaToAdd = new EstadoTarea()
                            {
                                IdEstadoTarea = newEstadoTarea.IdEstadoTarea,
                                DescripcionEstado = newEstadoTarea.DescripcionEstado
                            };

                            db.EstadoTareas.Add(estadoTareaToAdd);
                            db.SaveChanges();
                            return Created($"/detail/{ newEstadoTarea.IdEstadoTarea}", new Response()
                            {
                                Data = new EstadoTareaDTO(estadoTareaToAdd)
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
                            Title = "The Estado Tarea already exists",
                            Detail = "The Estado Tarea already exists in the database"
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
        public async Task<IActionResult> DeleteTaskState(int id)
        {
            try
            {
                using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
                {
                    EstadoTarea estadoTarea = db.EstadoTareas.FirstOrDefault(task => task.IdEstadoTarea == id);
                    if (estadoTarea != null)
                    {
                        db.EstadoTareas.Remove(estadoTarea);
                        db.SaveChanges();

                        return Ok(new Response()
                        {
                            Data = new { deleteId = id }
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
                                    Detail = "Couldn't find the Estado Tarea."
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
        public async Task<IActionResult> UpdateEstadoTarea(int id, [FromBody]EstadoTarea estadoTarea)
        {
            try
            {
                using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso"))) 
                {
                    List<Error> errors = new List<Error>();
                    EstadoTarea estadoTareaUpdated = db.EstadoTareas.FirstOrDefault(task => task.IdEstadoTarea == id);
                    if (estadoTarea != null)
                    {
                        if (estadoTarea.IdEstadoTarea > 0)
                        {
                            estadoTareaUpdated.IdEstadoTarea = estadoTarea.IdEstadoTarea;
                        }
                        else
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field: 'IdEstadoTarea'",
                                Detail = "The field IdEstadoTarea can´t be less than 0"
                            });
                        }

                        if (string.IsNullOrWhiteSpace(estadoTarea.DescripcionEstado))
                        {
                            estadoTareaUpdated.DescripcionEstado = estadoTarea.DescripcionEstado;
                        }
                        else
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field: 'DescripcionEstado'",
                                Detail = "The fild DescripcionEstado can´t be null or white space"
                            });
                        }

                        db.SaveChanges();

                        return Ok(new Response() {
                            Data = new EstadoTareaDTO(estadoTareaUpdated),
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
                                    Detail = "Couldn´t find the EstadoTarea"
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
