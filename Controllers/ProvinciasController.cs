using Microsoft.AspNetCore.Mvc;

namespace PortalEgresadosAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ProvinciasController : ControllerBase
{
    [HttpGet]
    public IResult ObtenerProvincias()
    {
        PortalEgresadosContext? context = null;

        try
        {
            context = new PortalEgresadosContext();

            var rawProvincias = context
                .Ciudads
                .OrderBy(p => p.Nombre)
                .ToList();

            if (rawProvincias == null)
            {
                return Results.Json(
                    data: "Error interno. Vuelva a intentarlo mas tarde",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }

            var provincias = new List<dynamic>();

            foreach (var rawProvincia in rawProvincias)
            {
                dynamic nacionalidad = new
                {
                    id = rawProvincia.CiudadId,
                    nombre = rawProvincia.Nombre
                };

                provincias.Add(nacionalidad);
            }

            return Results.Ok(provincias);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);

            return Results.Json(
                data: "Error interno. Vuelva a intentarlo mas tarde",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
        finally
        {
            context?.Dispose();
        }
    }
}