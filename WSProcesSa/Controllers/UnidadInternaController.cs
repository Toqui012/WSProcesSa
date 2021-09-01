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
    [Route("api/unidadInterna")]
    [ApiController]
    public class UnidadInternaController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly IUrlHelper helper;

        public UnidadInternaController(IConfiguration config, IWebHostEnvironment hostEnvironment)
        {
            this.config = config;
            this.hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public ActionResult GetUnidadInterna()
        {
            try
            {
                using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
                {
                    List<UnidadInternaDTO> response = db.UnidadInternas.Select(u => new UnidadInternaDTO(u)).ToList();
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
        public ActionResult AddUnidadInterna([FromBody] UnidadInterna newUnidadInternaToAdd)
        {
            using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
            {
                try
                {
                    List<Error> errors = new List<Error>();
                    if (!db.UnidadInternas.Any(unidad => unidad.IdUnidadInterna == newUnidadInternaToAdd.IdUnidadInterna))
                    {
                        if (newUnidadInternaToAdd.IdUnidadInterna < 0)
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'IdUnidadInterna'",
                                Detail = "The Field 'IdUnidadInterna' cannot be less 0"
                            });
                        }

                        if (string.IsNullOrWhiteSpace(newUnidadInternaToAdd.NombreUnidad))
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'NombreUnidadInterna'",
                                Detail = "The field 'NombreUnidadInterna' can't be null or white space"
                            });
                        }

                        if (string.IsNullOrWhiteSpace(newUnidadInternaToAdd.DescripcionUnidad))
                        {
                            newUnidadInternaToAdd.DescripcionUnidad = string.Empty;
                        }

                        if (errors.Count == 0)
                        {
                            UnidadInterna unidadInternaToAdd = new UnidadInterna()
                            {
                                NombreUnidad = newUnidadInternaToAdd.NombreUnidad,
                                DescripcionUnidad = newUnidadInternaToAdd.DescripcionUnidad,
                            };

                            db.UnidadInternas.Add(unidadInternaToAdd);
                            db.SaveChanges();
                            return Created($"/detail/{newUnidadInternaToAdd.IdUnidadInterna}", new Response()
                            {
                                Data = new UnidadInternaDTO(unidadInternaToAdd)
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
                            Title = "The Unidad Interna already exists",
                            Detail = "The Unidad Interna already exists in the database"
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
        /// Elimina una unidad interna de la base de datos
        /// </summary>
        /// <param name="id">Id de la unidad Interna</param>
        /// <returns>Retorna la id de la unidad interna eliminada</returns>

        public ActionResult DeleteUnidadInterna(int id)
        {
            try
            {
                using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
                {
                    UnidadInterna unidadInterna = db.UnidadInternas.Where(unidad => unidad.IdUnidadInterna == id).FirstOrDefault();

                    if (unidadInterna != null)
                    {
                        db.UnidadInternas.Remove(unidadInterna);
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
                                    Detail = "Couldn't find the UnidadInterna."
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
        /// Actualiza los datos de una Unidad Interna
        /// </summary>
        /// <param name="id">Id de la Unidad Interna para actualizar</param>
        /// <param name="unidad">Objeto con los datos de la unidad Interna actualizados</param>
        /// <returns>Retorna un objeto con los datos de la Unidad Interna actualizados</returns>
        /// 

        public ActionResult UpdateRole(int id, [FromBody] UnidadInterna unidad)
        {
            try
            {
                using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
                {
                    List<Error> errors = new List<Error>();
                    UnidadInterna unidadUpdated = db.UnidadInternas.Where(unidad => unidad.IdUnidadInterna == id).FirstOrDefault();
                    if (unidadUpdated != null)
                    {
                        if (!string.IsNullOrWhiteSpace(unidadUpdated.NombreUnidad))
                        {
                            unidadUpdated.NombreUnidad = unidad.NombreUnidad;
                        }
                        else
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid field: NombreUnidad",
                                Detail = "The field 'NombreUnidad'does not contain the required format."
                            });
                        }

                        if (!string.IsNullOrEmpty(unidadUpdated.DescripcionUnidad) || string.IsNullOrEmpty(unidadUpdated.DescripcionUnidad))
                        {
                            unidadUpdated.DescripcionUnidad = unidad.DescripcionUnidad;
                        }
                        else
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid field: DescripcionUnidad",
                                Detail = "The field 'DescripcionUnidad' does not contain the required format."
                            });
                        }
                        db.SaveChanges();

                        return Ok(new Response()
                        {
                            Data = new UnidadInternaDTO(unidadUpdated),
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
                                    Detail = "Couldn´t find the Unidad Interna"
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
