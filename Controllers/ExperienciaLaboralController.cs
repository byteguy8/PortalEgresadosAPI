using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PortalEgresadosAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ExperienciaLaboralController : ControllerBase
{

    [HttpGet("{egresadoId}")]
    public async Task<IResult> ExperienciaLaboralEgresado(int egresadoId)
    {

        PortalEgresadosContext? context;

        try
        {
            context = new PortalEgresadosContext();

            var ExperienciaLaborales = await context
                    .ExperienciaLaborals
                    .Where(e => e.EgresadoId == egresadoId)
                    .ToListAsync();

            return Results.Json(
                data: ExperienciaLaborales,
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
