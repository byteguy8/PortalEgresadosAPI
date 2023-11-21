using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PortalEgresadosAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ExperienciaLaboralController : ControllerBase
{
    /*En los get estas devolviendo todos los datos de la entidad como tal no solo los que te pide 
    el frondent muchos son innecesarios para ellos*/
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

    [HttpPost]
    public async Task<IResult> CreateExperienciaLaboralEgresado([FromBody] ExperienciaLaboralPOSTDTO experienciaLaboral)
    {
        try
        {
            PortalEgresadosContext context = new PortalEgresadosContext();

            ExperienciaLaboral experienciaLaboralToInsert = experienciaLaboral.Convert();

            await context.ExperienciaLaborals.AddAsync(experienciaLaboralToInsert);

            await context.SaveChangesAsync();

            return Results.Json(
                  data: experienciaLaboralToInsert.ExperienciaLaboralId,
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

    [HttpDelete]

    public async Task<IResult> DeleteExperienciaLaboralEgresado([FromQuery] int id)
    {

        try
        {
            PortalEgresadosContext context = new PortalEgresadosContext();

            ExperienciaLaboral experiencia =
                await context.ExperienciaLaborals
                .FirstOrDefaultAsync(experienciaLaboral => experienciaLaboral.ExperienciaLaboralId == id) ?? new();

            context.ExperienciaLaborals.Remove(experiencia);

            await context.SaveChangesAsync();

            return Results.Json(
                  data: true,
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
