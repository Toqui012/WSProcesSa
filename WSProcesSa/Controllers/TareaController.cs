using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSProcesSa.Classes;
using WSProcesSa.Controllers;
using WSProcesSa.Models;
using WSProcesSa.DTO;
using Microsoft.AspNetCore.Authorization;

namespace WSProcesSa.Controllers
{
    [Route("api/tarea")]
    [Authorize]
    [ApiController]
    public class TareaController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly IUrlHelper helper;

        public TareaController(IConfiguration config, IWebHostEnvironment hostEnvironment)
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
                    List<TareaDTO> response = db.Tareas.Select(f => new TareaDTO(f)).ToList();
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

        [HttpGet]
        [Route("oneTask/{idTarea}")]
        public async Task<IActionResult> GetOneTask(int idTarea)
        {
            using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
            {
                try
                {
                    List<Error> errors = new List<Error>();
                    //List<UsuarioDTO> response = db.Usuarios.Select(u => new UsuarioDTO(u)).ToList();
                    List<Tarea> response = db.Tareas.Where(u => u.IdTarea == idTarea).ToList();
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
        public async Task<IActionResult>AddTask([FromBody] Tarea newTareaToAdd) 
        {
            using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
            {
                try
                {
                    List<Error> errors = new List<Error>();
                    if (!db.Tareas.Any(task => task.IdTarea == newTareaToAdd.IdTarea))
                    {
                        if (newTareaToAdd.IdTarea < 0)
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'IdTarea'",
                                Detail = "The Field 'IdTarea' cannot be less than 0"
                            });
                        }

                        if (string.IsNullOrWhiteSpace(newTareaToAdd.NombreTarea))
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'NombreTarea'",
                                Detail = "The Field 'NombreTarea' cannot be less 0"
                            });
                        }

                        if (string.IsNullOrWhiteSpace(newTareaToAdd.DescripcionTarea))
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'DescripcionTarea'",
                                Detail = "The Field 'DescripcionTarea' can´t be null or whitespace"
                            });
                        }

                        if (newTareaToAdd.FechaPlazo < DateTime.Now)
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'FechaPlazo'",
                                Detail = "The Field 'FechaPlazo' can´t be null or whitespace"
                            });
                        }

                        // Dennisse
                        if (string.IsNullOrWhiteSpace(newTareaToAdd.CreadaPor))
						{
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'FechaPlazo'",
                                Detail = "The Field 'CreadaPor' can´t be null or whitespace"
                            });
                        }


                        if (newTareaToAdd.FkRutUsuario.Length > 12 || newTareaToAdd.FkRutUsuario.Length < 8 || string.IsNullOrWhiteSpace(newTareaToAdd.FkRutUsuario))
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'RutUsuario'",
                                Detail = "The Field 'RutUsuarip' can't be null or white space"
                            });
                        }

                        if (newTareaToAdd.FkPrioridadTarea < 0)
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'FkPrioridadTarea'",
                                Detail = "The Field 'FkPrioridadTarea' can't be less than 0"
                            });
                        }

                        if (errors.Count == 0)
                        {
                            Tarea taskAdd = new Tarea()
                            {
                                IdTarea = newTareaToAdd.IdTarea,
                                NombreTarea = newTareaToAdd.NombreTarea,
                                DescripcionTarea = newTareaToAdd.DescripcionTarea,
                                FechaPlazo = newTareaToAdd.FechaPlazo,
                                PorcentajeAvance = 1,
                                FechaCreacion = DateTime.Now,
                                CreadaPor = newTareaToAdd.CreadaPor,
                                FkRutUsuario = "0.000.000",
                                FkIdJustificacion = newTareaToAdd.FkIdJustificacion,
                                FkEstadoTarea = 1,
                                FkPrioridadTarea = newTareaToAdd.FkPrioridadTarea,
                            };

                            db.Tareas.Add(taskAdd);
                            db.SaveChanges();
                            return Created($"/detail/{newTareaToAdd.IdTarea}", new Response()
                            {
                                Data = new TareaDTO(taskAdd)
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
                            Title = "The Task already exists",
                            Detail = "The Task already exists in the database"
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

        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
                {
                    Tarea task = db.Tareas.Where(f => f.IdTarea == id).FirstOrDefault();

                    if (task != null)
                    {
                        db.Tareas.Remove(task);
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
                                    Detail = "Couldn't find the Task."
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

        public async Task<IActionResult> UpdateTask(int id, [FromBody] Tarea task)
        {
            try
            {
                using ( ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
                {
                    List<Error> errors = new List<Error>();
                    Tarea taskUpdated = db.Tareas.Where(f => f.IdTarea == id).FirstOrDefault();
                    if (taskUpdated != null)
                    {
                        task.FechaPlazo = DateTime.Parse(task.FechaPlazo.ToString("dd/MM/yyyy"));

                        if (string.IsNullOrWhiteSpace(task.NombreTarea))
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'NombreTarea'",
                                Detail = "The Field 'NombreTarea' cannot be less 0"
                            });
                        }
                        else
                        {
                            taskUpdated.NombreTarea = task.NombreTarea;
                        }

                        if (string.IsNullOrWhiteSpace(task.DescripcionTarea))
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'DescripcionTarea'",
                                Detail = "The Field 'DescripcionTarea' can´t be null or whitespace"
                            });
                        }
                        else
                        {
                            taskUpdated.DescripcionTarea = task.DescripcionTarea;
                        }

                        if (task.FechaPlazo < DateTime.Now)
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'FechaPlazo'",
                                Detail = "The Field 'FechaPlazo' can´t be null or whitespace"
                            });
                        }
                        else
                        {
                            taskUpdated.FechaPlazo = task.FechaPlazo;
                        }

                        // Dennisse
                        if (task.PorcentajeAvance < 0 || task.PorcentajeAvance > 100)
						{
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'DescripcionTarea'",
                                Detail = "The Field 'PorcentajeAvance can´t be less than 0 and greater than 100"
                            });
                        } 
                        else
						{
                            taskUpdated.PorcentajeAvance = task.PorcentajeAvance;
						}

                        if (task.FkRutUsuario.Length > 12 || task.FkRutUsuario.Length < 8 || string.IsNullOrWhiteSpace(task.FkRutUsuario))
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
                        else
                        {
                            taskUpdated.FkRutUsuario = task.FkRutUsuario;
                        }


                        if (task.FkEstadoTarea < 0)
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'FkEstadoTarea'",
                                Detail = "The Field 'FkIdJustificación' can't be less than 0"
                            });
                        }
                        else
                        {
                            taskUpdated.FkEstadoTarea = task.FkEstadoTarea;
                        }

                        if (task.FkPrioridadTarea < 0)
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'FkPrioridadTarea'",
                                Detail = "The Field 'FkPrioridadTarea' can't be less than 0"
                            });
                        }
                        else
                        {
                            taskUpdated.FkPrioridadTarea = task.FkPrioridadTarea;
                        }

                        db.SaveChanges();
                        return Ok(new Response()
                        {
                            Data = new TareaDTO(taskUpdated),
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
                                    Detail = "Couldn´t find the Task"
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
        [Route("assignTask/{idTask}/{rutUser}")]
        public async Task<IActionResult> assignTask(int idTask, string rutUser)
        {
            try
            {
                using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
                {
                    List<Error> errors = new List<Error>();
                    Tarea taskUpdated = db.Tareas.FirstOrDefault(t => t.IdTarea == idTask);
                    if (taskUpdated != null)
                    {
                        if (rutUser.Length > 12 || rutUser.Length < 8 || string.IsNullOrWhiteSpace(rutUser))
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'RutUsuario'",
                                Detail = "The Field 'RutUsuaro' can't be null or white space"
                            });
                        }
                        else
                        {
                            taskUpdated.FkRutUsuario = rutUser;
                            taskUpdated.FkEstadoTarea = 2;
                        }

                        db.SaveChanges();
                        return Ok(new Response()
                        {
                            Data = new TareaDTO(taskUpdated),
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
                                    Detail = "Couldn´t find the Task"
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
        [Route("acceptTask/{idTask}")]
        public async Task<IActionResult> acceptTask(int idTask)
        {
            try
            {
                using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
                {
                    List<Error> errors = new List<Error>();
                    //Tarea que yo quiero editar
                    Tarea taskUpdated = db.Tareas.FirstOrDefault(t => t.IdTarea == idTask);

                    if (taskUpdated != null)
                    {
                        if (idTask < 0)
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'IdTask'",
                                Detail = "The Field 'FkPrioridadTarea' can't be less than 0"
                            });
                        }
                        else
                        {
                            //Historial de Estado de tarea
                            //1 = Creada
                            //2 = Asignada
                            //3 = Aceptada
                            //4 = Rechazada
                            taskUpdated.FkEstadoTarea = 3;
                        }
                        db.SaveChanges();
                        return Ok(new Response()
                        {
                            Data = new TareaDTO(taskUpdated),

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
                                    Detail = "Couldn´t find the Task"
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
        [Route("rejectTask/{idTask}")]
        public async Task<IActionResult> rejectTask(int idTask)
        {
            try
            {
                using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
                {
                    List<Error> errors = new List<Error>();
                    //Tarea que yo quiero editar
                    Tarea taskUpdated = db.Tareas.FirstOrDefault(t => t.IdTarea == idTask);

                    if (taskUpdated != null)
                    {
                        if (idTask < 0)
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'IdTask'",
                                Detail = "The Field 'FkPrioridadTarea' can't be less than 0"
                            });
                        }
                        else
                        {
                            //Historial de Estado de tarea
                            //1 = Creada
                            //2 = Asignada
                            //3 = Aceptada
                            //4 = Rechazada
                            taskUpdated.FkEstadoTarea = 4;
                        }
                        db.SaveChanges();
                        return Ok(new Response()
                        {
                            Data = new TareaDTO(taskUpdated),
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
                                    Detail = "Couldn´t find the Task"
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

        [HttpGet]
        [Route("getTaskInProcess")]
        public async Task<IActionResult> GetTaskInProcess()
        {
            try
            {
                using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
                {
                    //Se Obtiene el listado de tareas con el estado de tareas Aceptado
                    int response = db.Tareas.Where(t => t.FkEstadoTarea == 3).Count();
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

        [HttpGet]
        [Route("getFinishTask")]
        public async Task<IActionResult> GetFinishTask()
        {
            try
            {
                using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
                {
                    //Se Obtiene el listado de tareas con el estado de tareas Aceptado
                    int response = db.Tareas.Where(t => t.FkEstadoTarea == 5).Count();
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

        [HttpGet]
        [Route("getRejectTask")]
        public async Task<IActionResult> GetRejectTask()
        {
            try
            {
                using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
                {
                    //Se Obtiene el listado de tareas con el estado de tareas Aceptado
                    int response = db.Tareas.Where(t => t.FkEstadoTarea == 4).Count();
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

        [HttpGet]
        [Route("getTaskCreated")]
        public async Task<IActionResult> GetTaskCreated()
        {
            try
            {
                using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
                {
                    //Se Obtiene el listado de tareas con el estado de tareas Aceptado
                    int response = db.Tareas.Where(t => t.FkEstadoTarea == 1).Count();
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

        [HttpGet]
        [Route("getAssignedTask")]
        public async Task<IActionResult> GetAssignedTask()
        {
            try
            {
                using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
                {
                    //Se Obtiene el listado de tareas con el estado de tareas Aceptado
                    int response = db.Tareas.Where(t => t.FkEstadoTarea == 2).Count();
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
}
