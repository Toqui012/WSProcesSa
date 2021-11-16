using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSProcesSa.Classes;
using WSProcesSa.DTO;
using WSProcesSa.Models;

namespace WSProcesSa.Controllers
{
    [Route("api/business")]
    [Authorize]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly IUrlHelper helper;

        public BusinessController(IConfiguration config, IWebHostEnvironment hostEnvironment)
        {
            this.config = config;
            this.hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetBusiness()
        {
            try
            {
                using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
                {
                    List<EmpresaDTO> response = db.Empresas.Select(b => new EmpresaDTO(b)).ToList();
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
        [Route("oneBusiness/{rutEmpresa}")]
        public async Task<IActionResult>GetOneBusiness(string rutEmpresa)
		{
            using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
			{
				try
				{
                    List<Error> errors = new List<Error>();
                    List<Empresa> response = db.Empresas.Where(u => u.RutEmpresa == rutEmpresa).ToList();
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
        public async Task<IActionResult> AddBussine([FromBody] Empresa newEmpresa)
        {
            using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
            {
                try
                {
                    List<Error> errors = new List<Error>();
                    if (!db.Empresas.Any(f => f.RutEmpresa == newEmpresa.RutEmpresa ||
                                                 f.RazonSocial == newEmpresa.RazonSocial))
                    {
                        if (string.IsNullOrWhiteSpace(newEmpresa.RutEmpresa))
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'RutEmpresa'",
                                Detail = "The Field 'RutEmpresa' cannot be null"
                            });
                        }

                        if (string.IsNullOrWhiteSpace(newEmpresa.RazonSocial))
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'RazonSocial'",
                                Detail = "The field 'RazonSocial' can't be null or white space"
                            });
                        }

                        if (string.IsNullOrWhiteSpace(newEmpresa.GiroEmpresa))
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'GiroEmpresa'",
                                Detail = "The field 'GiroEmpresa' can't be null or white space"
                            });
                        }

                        if (string.IsNullOrWhiteSpace(newEmpresa.DireccionEmpresa))
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'DireccionEmpresa'",
                                Detail = "The field 'DireccionEmpresa'  can't be null or white space"
                            });
                        }

                        if (newEmpresa.NumeroTelefono < 9)
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'NumeroTelefono'",
                                Detail = "The field 'NumeroTelefono' can´t be null or different 9"
                            });
                        }

                        if (string.IsNullOrWhiteSpace(newEmpresa.CorreoElectronicoEmpresa))
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field 'CorreoElectronico'",
                                Detail = "The field 'CorreoElectronico'  can't be null or white space"
                            });
                        }

                        if (errors.Count == 0)
                        {
                            Empresa EmpresaToAdd = new Empresa()
                            {
                                RutEmpresa = newEmpresa.RutEmpresa,
                                RazonSocial = newEmpresa.RazonSocial,
                                GiroEmpresa = newEmpresa.GiroEmpresa,
                                DireccionEmpresa = newEmpresa.DireccionEmpresa,
                                NumeroTelefono = newEmpresa.NumeroTelefono,
                                CorreoElectronicoEmpresa = newEmpresa.CorreoElectronicoEmpresa,
                            };

                            db.Empresas.Add(EmpresaToAdd);
                            db.SaveChanges();
                            return Created($"/detail/{newEmpresa.RutEmpresa}", new Response()
                            {
                                Data = new EmpresaDTO(EmpresaToAdd)
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
                            Title = "The Business already exists",
                            Detail = "The Business already exists in the database"
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
        [Route("delete/{rutEmpresa}")]
        /// <summary>
        /// Elimina una empresa de la base de datos
        /// </summary>
        /// <param name="rutEmpresa">Rut de la Empresa</param>
        /// <returns>Retorna el rut de la empresa eliminada</returns>

        public async Task<IActionResult> DeleteEmpresa(string rutEmpresa)
        {
            try
            {
                using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
                {
                    Empresa business = db.Empresas.Where(b => b.RutEmpresa == rutEmpresa).FirstOrDefault();

                    if (business != null)
                    {
                        db.Empresas.Remove(business);
                        db.SaveChanges();

                        return Ok(new Response()
                        {
                            Data = new { deletedId = rutEmpresa }
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
                                    Detail = "Couldn't find the Empresa."
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
        [Route("update/{rutEmpresa}")]

        public async Task<IActionResult> UpdateEmpresa(string rutEmpresa, [FromBody] Empresa business)
        {
            try
            {
                using (ModelContext db = new ModelContext(config.GetConnectionString("Acceso")))
                {
                    List<Error> errors = new List<Error>();
                    Empresa businessUpdated = db.Empresas.Where(b => b.RutEmpresa == rutEmpresa).FirstOrDefault();

                    if (businessUpdated != null)
                    {
                        if (!string.IsNullOrWhiteSpace(business.RutEmpresa))
                        {
                            businessUpdated.RutEmpresa = business.RutEmpresa;
                        }
                        else
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid field: RutEmpresa",
                                Detail = "The field 'RutEmpresa'does not contain the required format."
                            });
                        }

                        if (!string.IsNullOrWhiteSpace(business.RazonSocial))
                        {
                            businessUpdated.RazonSocial = business.RazonSocial;
                        }
                        else
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid field: RazonSocial",
                                Detail = "The field 'RazonSocial'does not contain the required format."
                            });
                        }

                        if (!string.IsNullOrWhiteSpace(business.GiroEmpresa))
                        {
                            businessUpdated.GiroEmpresa = business.GiroEmpresa;
                        }
                        else
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid field: GiroEmpresa",
                                Detail = "The field 'GiroEmpresa'does not contain the required format."
                            });
                        }

                        if (!string.IsNullOrWhiteSpace(business.DireccionEmpresa))
                        {
                            businessUpdated.DireccionEmpresa = business.DireccionEmpresa;
                        }

                        if (business.NumeroTelefono == 9)
                        {
                            businessUpdated.NumeroTelefono = business.NumeroTelefono;
                        }
                        else
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid Field: NumeroTelefono",
                                Detail = "The Field 'NumeroTelefono' can´t be less than 0 "
                            });
                        }

                        if (!string.IsNullOrWhiteSpace(business.CorreoElectronicoEmpresa))
                        {
                            businessUpdated.CorreoElectronicoEmpresa = business.CorreoElectronicoEmpresa;
                        }
                        else
                        {
                            errors.Add(new Error()
                            {
                                Id = errors.Count + 1,
                                Status = "Bad Request",
                                Code = 400,
                                Title = "Invalid field: CorreoElectronicoEmpresas",
                                Detail = "The field 'CorreoElectronicoEmpresas'does not contain the required format."
                            });
                        }

                        db.SaveChanges();

                        return Ok(new Response()
                        {
                            Data = new EmpresaDTO(businessUpdated),
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
                                    Detail = "Couldn´t find the business"
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
