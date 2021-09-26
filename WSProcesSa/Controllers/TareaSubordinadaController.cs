﻿using Microsoft.AspNetCore.Authorization;
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
    [Route("api/TareaSubordinada")]
    [Authorize]
    [ApiController]
    public class TareaSubordinadaController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly IUrlHelper helper;

        public TareaSubordinadaController(IConfiguration config, IWebHostEnvironment hostEnvironment)
        {
            this.config = config;
            this.hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetTask()
        {
            try
            {
                using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
                {
                    List<TareaSubordinadaDTO> response = db.TareaSubordinada.Select(task => new TareaSubordinadaDTO(task)).ToList();
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
                                    Detail = "There isno data on database"
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
        public async Task<IActionResult> AddTareaSubordinada([FromBody] TareaSubordinadum newTareaSubordinadaToAdd)
        {
            using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
            {
                try
                {
                    List<Error> errors = new List<Error>();
                    if (!db.TareaSubordinada.Any(task => task.IdTareaSubordinada == newTareaSubordinadaToAdd.IdTareaSubordinada ||
                                                 task.NombreSubordinada == newTareaSubordinadaToAdd.NombreSubordinada))
                    {
                        if (newTareaSubordinadaToAdd.IdTareaSubordinada < 0)
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid FIeld 'IdTareaSubordinada'",
                                Detail = "The Field 'IdTareaSubordinada' can´t be less than 0"
                            });
                        }

                        if (string.IsNullOrWhiteSpace(newTareaSubordinadaToAdd.NombreSubordinada))
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'NombreSubordinada'",
                                Detail = "The field 'NombreSubordinada' can't be null or white space"
                            });
                        }

                        if (string.IsNullOrWhiteSpace(newTareaSubordinadaToAdd.DescripcionSubordinada))
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'DescripcionSubordinada'",
                                Detail = "The field 'DescripcionSubordinada' can't be null or white space"
                            });
                        }

                        if (newTareaSubordinadaToAdd.FkIdTarea < 0)
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
                            TareaSubordinadum taskToAdd = new TareaSubordinadum()
                            {
                                IdTareaSubordinada = newTareaSubordinadaToAdd.IdTareaSubordinada,
                                NombreSubordinada = newTareaSubordinadaToAdd.NombreSubordinada,
                                DescripcionSubordinada = newTareaSubordinadaToAdd.DescripcionSubordinada,
                                FkIdTarea = newTareaSubordinadaToAdd.FkIdTarea,
                            };

                            db.TareaSubordinada.Add(taskToAdd);
                            db.SaveChanges();
                            return Created($"/detaiil/{newTareaSubordinadaToAdd.IdTareaSubordinada}", new Response()
                            {
                                Data = new TareaSubordinadaDTO(taskToAdd)
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
                            Title = "The Task already exist",
                            Detail = "The Task already exist in the database"
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
        [Route("delete/id")]
        public async Task<IActionResult> DeleteTaskSubordinada(int id) 
        {
            try
            {
                using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
                {
                    TareaSubordinadum tareaSubordinada = db.TareaSubordinada.FirstOrDefault(task => task.IdTareaSubordinada == id);

                    if (tareaSubordinada != null)
                    {
                        db.TareaSubordinada.Remove(tareaSubordinada);
                        db.SaveChanges();

                        return Ok(new Response()
                        {
                            Data = new {deleteId = id}
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
                                    Detail = "Couldn't find the TareaSubordinada."
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
                    Title = err.Message,
                    Detail = err.InnerException != null ? err.InnerException.ToString() : err.Message
                });
                return StatusCode(500, response);
            }
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TareaSubordinadum task)
        {
            try
            {
                using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso"))) 
                {
                    List<Error> errors = new List<Error>();
                    TareaSubordinadum taskUpdated = db.TareaSubordinada.FirstOrDefault(task => task.IdTareaSubordinada == id);
                    if (taskUpdated != null)
                    {
                        if (string.IsNullOrWhiteSpace(task.NombreSubordinada))
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'NombreSubordinada'",
                                Detail = "The field 'NombreSubordinada' can't be null or white space"
                            });
                        }
                        else
                        {
                            taskUpdated.NombreSubordinada = task.NombreSubordinada;
                        }

                        if (string.IsNullOrWhiteSpace(task.DescripcionSubordinada))
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'DescripcionSubordinada'",
                                Detail = "The field 'DescripcionSubordinada' can't be null or white space"
                            });
                        }
                        else
                        {
                            taskUpdated.DescripcionSubordinada = task.DescripcionSubordinada;
                        }

                        if (task.FkIdTarea < 0)
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'FkIdTarea'",
                                Detail = "The field 'FkIdTarea' can't be null or equals than 0"
                            });
                        }
                        else
                        {
                            taskUpdated.FkIdTarea = task.FkIdTarea;
                        }

                        db.SaveChanges();

                        return Ok(new Response()
                        {
                            Data = new TareaSubordinadaDTO(taskUpdated),
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
                                    Detail = "Couldn´t find the task"
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