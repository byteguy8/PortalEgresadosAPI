using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PortalEgresadosAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class EgresadosHabilidadController : ControllerBase
{
    [HttpGet]
    public IResult ObtenerHabilidades()
    {
        PortalEgresadosContext? context = null;

        try
        {
            context = new PortalEgresadosContext();

            var rawHabilidades = context
                .Habilidads
                .OrderBy(r => r.Nombre)
                .ToList();

            if (rawHabilidades == null)
            {
                return Results.Json(
                    data: "Hubo un error al procesar la solicitud. Intentelo de nuevo",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }

            var habilidades = new List<dynamic>();

            foreach (var rawHabilidad in rawHabilidades)
            {
                dynamic habilidad = new
                {
                    id = rawHabilidad.HabilidadId,
                    habilidad = rawHabilidad.Nombre
                };

                habilidades.Add(habilidad);
            }

            return Results.Ok(habilidades);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);

            return Results.Json(
                data: "Hubo un error al procesar la solicitud. Intentelo de nuevo",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
        finally
        {
            context?.Dispose();
        }
    }

    [HttpGet("Buscar/{termino}")]
    public IResult BuscarHabilidades(string termino)
    {
        PortalEgresadosContext? context = null;

        try
        {
            context = new PortalEgresadosContext();

            var rawHabilidades = context
                .Habilidads
                .Where(r => EF.Functions.Like(r.Nombre, $"%{termino}%"))
                .OrderBy(r => r.Nombre)
                .ToList();

            if (rawHabilidades == null)
            {
                return Results.Json(
                    data: "Hubo un error al procesar la solicitud. Intentelo de nuevo",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }

            var habilidades = new List<dynamic>();

            foreach (var rawHabilidad in rawHabilidades)
            {
                dynamic habilidad = new
                {
                    id = rawHabilidad.HabilidadId,
                    habilidad = rawHabilidad.Nombre
                };

                habilidades.Add(habilidad);
            }

            return Results.Ok(habilidades);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);

            return Results.Json(
                data: "Hubo un error al procesar la solicitud. Intentelo de nuevo",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
        finally
        {
            context?.Dispose();
        }
    }

    [HttpGet("{egresadoId}")]
    public IResult ObtenerHabilidades(int egresadoId)
    {
        PortalEgresadosContext? context = null;

        try
        {
            context = new PortalEgresadosContext();

            var rawEgresado = context
                .Egresados
                .Find(egresadoId);

            if (rawEgresado == null)
            {
                return Results.Json(
                    data: "No existen registros para el egresado suministrado",
                    statusCode: StatusCodes.Status400BadRequest
                );
            }

            var rawEgresadoHabilidades = context
                .EgresadoHabilidads
                .Where(h => h.EgresadoId == egresadoId)
                .Include(h => h.Habilidad)
                .ToList();

            if (rawEgresadoHabilidades == null)
            {
                return Results.Json(
                    data: "Hubo un error al procesar la solicitud. Intentelo de nuevo",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }

            var habilidades = new List<dynamic>();

            foreach (var rawEgresadoHabilidad in rawEgresadoHabilidades)
            {
                dynamic habilidad = new
                {
                    id = rawEgresadoHabilidad.EgresadoHabilidadId,
                    egresadoId = rawEgresado.EgresadoId,
                    habilidadId = rawEgresadoHabilidad.Habilidad.HabilidadId,
                    habilidad = rawEgresadoHabilidad.Habilidad.Nombre
                };

                habilidades.Add(habilidad);
            }

            return Results.Ok(habilidades);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);

            return Results.Json(
                data: "Hubo un error al procesar la solicitud. Intentelo de nuevo",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
        finally
        {
            context?.Dispose();
        }
    }

    [HttpPost("{habilidadId}/{egresadoId}")]
    public IResult AgregarHabilidad(int habilidadId, int egresadoId)
    {
        PortalEgresadosContext? context = null;

        try
        {
            context = new PortalEgresadosContext();

            var rawHabilidad = context
                .Habilidads
                .Find(habilidadId);

            if (rawHabilidad == null)
            {
                return Results.Json(
                    data: "No existen registros para la habilidad suministrada",
                    statusCode: StatusCodes.Status400BadRequest
                );
            }

            var rawEgresado = context
                .Egresados
                .Find(egresadoId);

            if (rawEgresado == null)
            {
                return Results.Json(
                    data: "No existen registros para el egresado suministrado",
                    statusCode: StatusCodes.Status400BadRequest
                );
            }

            var rawExisteEgresadoHabilidad = context
                .EgresadoHabilidads
                .Include(h => h.Habilidad)
                .FirstOrDefault(h =>
                    h.HabilidadId == habilidadId && h.EgresadoId == egresadoId
                );

            if (rawExisteEgresadoHabilidad != null)
            {
                var nombre = rawExisteEgresadoHabilidad.Habilidad.Nombre;

                return Results.Json(
                    data: "La habilidad suministrada ya esta vinculada al egresado suministrado",
                    statusCode: StatusCodes.Status400BadRequest
                );
            }

            var rawEgresadoHabilidad = context
                .EgresadoHabilidads
                .Add(new EgresadoHabilidad
                {
                    HabilidadId = rawHabilidad.HabilidadId,
                    EgresadoId = rawEgresado.EgresadoId
                });

            if (rawEgresadoHabilidad == null)
            {
                return Results.Json(
                    data: "Hubo un error al procesar la solicitud. Intentelo de nuevo",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }

            context.SaveChanges();

            dynamic result = new
            {
                id = rawEgresadoHabilidad.Entity.EgresadoHabilidadId,
                egresadoId = rawEgresado.EgresadoId,
                habilidadId = rawHabilidad.HabilidadId,
                habilidad = rawHabilidad.Nombre
            };

            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);

            return Results.Json(
                data: "Hubo un error al procesar la solicitud. Intentelo de nuevo",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
        finally
        {
            context?.Dispose();
        }
    }

    [HttpDelete("{habilidadEgresadoId}")]
    public IResult EliminarHabilidad(int habilidadEgresadoId)
    {
        PortalEgresadosContext? context = null;

        try
        {
            context = new PortalEgresadosContext();

            var habilidadEgresadoEntidad = context
                .EgresadoHabilidads
                .Find(habilidadEgresadoId);

            if (habilidadEgresadoEntidad == null)
            {
                return Results.Ok(true);
            }

            context
                .EgresadoHabilidads
                .Remove(habilidadEgresadoEntidad);

            context.SaveChanges();

            return Results.Ok(true);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);

            return Results.Json(
                data: "Hubo un error al procesar la solicitud. Intentelo de nuevo",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
        finally
        {
            context?.Dispose();
        }
    }

}