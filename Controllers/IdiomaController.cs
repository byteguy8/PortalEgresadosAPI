using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace PortalEgresadosAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class IdiomaController : ControllerBase
{

    [HttpGet]
    public IResult ObtenerIdiomas()
    {
        PortalEgresadosContext? context = null;

        try
        {
            context = new PortalEgresadosContext();

            var rawIdiomas = context
                .Idiomas
                .OrderBy(r => r.Nombre)
                .ToList();

            if (rawIdiomas == null)
            {
                return Results.Json(
                    data: "Hubo un error al procesar la solicitud. Intentelo de nuevo",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }

            var Idiomas = new List<dynamic>();

            foreach (var rawIdioma in rawIdiomas)
            {
                dynamic idiomas = new
                {
                    id = rawIdioma.IdiomaId,
                    idioma = rawIdioma.Nombre
                };

                Idiomas.Add(idiomas);
            }

            return Results.Ok(Idiomas);
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
    public IResult BuscarIdiomas(string termino)
    {
        PortalEgresadosContext? context = null;

        try
        {
            context = new PortalEgresadosContext();

            var rawIdiomas = context
                .Idiomas
                .Where(r => EF.Functions.Like(r.Nombre, $"%{termino}%"))
                .OrderBy(r => r.Nombre)
                .ToList();

            if (rawIdiomas == null)
            {
                return Results.Json(
                    data: "Hubo un error al procesar la solicitud. Intentelo de nuevo",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }

            var Idiomas = new List<dynamic>();

            foreach (var rawIdioma in rawIdiomas)
            {
                dynamic idioma = new
                {
                    id = rawIdioma.IdiomaId,
                    idioma = rawIdioma.Nombre
                };

                Idiomas.Add(idioma);
            }

            return Results.Ok(Idiomas);
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
    public IResult IdiomaEgresado(int egresadoId)
    {

        PortalEgresadosContext? context = null;

        try
        {
            context = new PortalEgresadosContext();

            var EgresadoIdiomas = context
                    .EgresadoIdiomas
                    .Where(i => i.EgresadoId == egresadoId)
                    .Include(i => i.Idioma)
                    .ToList();

            var Idiomas = new List<dynamic>();

            foreach (var idioma in EgresadoIdiomas)
            {
                dynamic i = new
                {
                    id = idioma.IdiomaId,
                    nombre = idioma.Idioma.Nombre,
                    egresadIdiomaId = idioma.EgresadoIdiomaId
                };

                Idiomas.Add(i);
            }

            return Results.Json(
                 data: Idiomas,
                 statusCode: StatusCodes.Status200OK
             );

        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);

            return Results.Json(
                data: new ErrorResult(0, "Unexpected server error"),
                statusCode: StatusCodes.Status500InternalServerError
            );

        }

    }

    [Authorize]
    [HttpPost("{egresadoId}/{idiomaId}")]
    public IResult PostIdiomaEgresado(int egresadoId, int idiomaId)
    {

        PortalEgresadosContext? context = null;
        IDbContextTransaction? transaction = null;

        try
        {

            context = new PortalEgresadosContext();
            transaction = context.Database.BeginTransaction();

            var AgregaIdioma = new EgresadoIdioma
            {

                EgresadoId = egresadoId,
                IdiomaId = idiomaId

            };

            var resultAddIdioma = context
                .EgresadoIdiomas
                .Add(AgregaIdioma);

            if (resultAddIdioma == null)
            {
                transaction.Rollback();

                var error = new ErrorMsg(0, $"Error inesperado");

                return Results.Json(data: error, statusCode: StatusCodes.Status500InternalServerError);
            }

            context.SaveChanges();

            transaction.Commit();

            return Results.Ok(resultAddIdioma.Entity.EgresadoIdiomaId);

        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.ToString());

            transaction?.Rollback();

            var error = new ErrorMsg(0, "Error no esperado");

            return Results.Json(
                data: error,
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
        finally
        {

            transaction?.Dispose();
            context?.Dispose();
        }
    }

    [Authorize]
    [HttpDelete("{EgresadoIdiomaId}")]
    public IResult DeleteIdiomaEgresado(int EgresadoIdiomaId)
    {

        PortalEgresadosContext? context = null;
        IDbContextTransaction? transaction = null;

        try
        {

            context = new PortalEgresadosContext();
            transaction = context.Database.BeginTransaction();

            var EliminaIdioma = new EgresadoIdioma
            {

                EgresadoIdiomaId = EgresadoIdiomaId

            };

            var resultEliminaIdioma = context
                .EgresadoIdiomas
                .Remove(EliminaIdioma);

            if (resultEliminaIdioma == null)
            {
                transaction.Rollback();

                var error = new ErrorMsg(0, $"Error inesperado");

                return Results.Json(data: error, statusCode: StatusCodes.Status500InternalServerError);
            }

            context.SaveChanges();

            transaction.Commit();

            return Results.Ok();

        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.ToString());

            transaction?.Rollback();

            var error = new ErrorMsg(0, "Error no esperado");

            return Results.Json(
                data: error,
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
        finally
        {

            transaction?.Dispose();
            context?.Dispose();
        }

    }

}
