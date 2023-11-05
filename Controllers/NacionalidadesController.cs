using Microsoft.AspNetCore.Mvc;

namespace PortalEgresadosAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class NacionalidadesController : ControllerBase
{
    [HttpGet]
    public IResult ObtenerNacionalidades()
    {
        PortalEgresadosContext? context = null;

        try
        {
            context = new PortalEgresadosContext();

            var rawNacionalidades = context
                .Pais
                .OrderBy(p => p.Nombre)
                .ToList();

            if (rawNacionalidades == null)
            {
                return Results.Json(
                    data: "Error interno. Vuelva a intentarlo mas tarde",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }

            var nacionalidades = new List<dynamic>();

            foreach (var rawNacionalidad in rawNacionalidades)
            {
                dynamic nacionalidad = new
                {
                    id = rawNacionalidad.PaisId,
                    nombre = rawNacionalidad.Nombre
                };

                nacionalidades.Add(nacionalidad);
            }

            return Results.Ok(nacionalidades);
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