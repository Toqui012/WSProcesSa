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
    [Route("api/justificacionTarea")]
    [Authorize]
    [ApiController]
    public class JustificacionTareaController : ControllerBase
    {

        private readonly IConfiguration config;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly IUrlHelper helper;

        public JustificacionTareaController(IConfiguration config, IWebHostEnvironment hostEnvironment)
        {
            this.config = config;
            this.hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetJustificacion()
        {
            using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
            {
                try
                {
                    List<JustificacionDTO> response = db.JustificacionTareas.Select(x => new JustificacionDTO(x)).ToList();
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

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddJustificacionTarea([FromBody] JustificacionTarea newJustificacionToAdd)
        {
            using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
            {
                try
                {
                    List<Error> errors = new List<Error>();
                    if (!db.JustificacionTareas.Any(task => task.IdJustificacion == newJustificacionToAdd.IdJustificacion))
                    {
                        if (string.IsNullOrWhiteSpace(newJustificacionToAdd.Descripcion))
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'DescripcionJustificacion'",
                                Detail = "The Field 'DescripcionJustificacion' can´t be null or white space"
                            });
                        }

                        if (errors.Count == 0)
                        {
                            JustificacionTarea justificacionTarea = new JustificacionTarea()
                            {
                                Descripcion = newJustificacionToAdd.Descripcion
                            };

                            db.JustificacionTareas.Add(justificacionTarea);
                            db.SaveChanges();
                            return Created($"/detail/{newJustificacionToAdd.IdJustificacion}", new Response()
                            {
                                Data = new JustificacionDTO(justificacionTarea)
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
        public async Task<IActionResult> DeleteJustificacionTarea(int id) 
        {
            try
            {
                using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
                {
                    JustificacionTarea justificacionTarea = db.JustificacionTareas.FirstOrDefault(task => task.IdJustificacion == id);

                    if (justificacionTarea != null)
                    {
                        db.JustificacionTareas.Remove(justificacionTarea);
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
        public async Task<IActionResult> UpdateJustificacionTarea(int id, [FromBody] JustificacionTarea justificacionTarea)
        {
            try
            {
                using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
                {
                    List<Error> errors = new List<Error>();
                    JustificacionTarea justificacionTareaUpdated = db.JustificacionTareas.FirstOrDefault(task => task.IdJustificacion == id);
                    if (justificacionTareaUpdated != null)
                    {
                        if (string.IsNullOrWhiteSpace(justificacionTarea.Descripcion))
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
                            justificacionTareaUpdated.Descripcion = justificacionTarea.Descripcion;
                        }

                        db.SaveChanges();

                        return Ok(new Response()
                        {
                            Data = new JustificacionDTO(justificacionTareaUpdated),
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
                                    Detail = "Couldn´t find the JustificacionTarea"
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
