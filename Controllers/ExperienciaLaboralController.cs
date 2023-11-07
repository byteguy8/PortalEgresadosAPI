using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PortalEgresadosAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ExperienciaLaboralController : ControllerBase
{

    [HttpGet("{egresadoId}")]
    public IResult ExperienciaLaboralEgresado(int egresadoId)
    {

        PortalEgresadosContext? context;

        try
        {
            context = new PortalEgresadosContext();

            var ExperienciaLaborales = context
                    .ExperienciaLaborals
                    .Where(e => e.EgresadoId == egresadoId)
                    .ToList();

                var Experiencias = new List<dynamic>();

                foreach (var experienciaLaboral in ExperienciaLaborales)
                {
                    dynamic e = new
                    {
                        id = experienciaLaboral.ExperienciaLaboralId,
                        organizacion = experienciaLaboral.Organizacion,
                        posicion = experienciaLaboral.Posicion,
                        fechaentrada = experienciaLaboral.FechaEntrada,
                        fechaSalida = experienciaLaboral.FechaSalida

                    };

                    Experiencias.Add(e);
                }

            return Results.Json(
                data: Experiencias,
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

}
