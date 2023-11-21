using Microsoft.AspNetCore.Authorization;
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
        PortalEgresadosContext? context = null;

        try
        {
            context = new PortalEgresadosContext();

            var ExperienciaLaborales = await context
                .ExperienciaLaborals
                .Where(e => e.EgresadoId == egresadoId)
                .ToListAsync();

            return Results.Ok(ExperienciaLaborales);
        }
        catch (Exception ex)
        {
            return Utils.HandleError(ex);
        }
        finally
        {
            context?.Dispose();
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IResult> CreateExperienciaLaboralEgresado([FromBody] ExperienciaLaboralPOSTDTO experienciaLaboral)
    {
        PortalEgresadosContext? context = null;

        try
        {
            context = new PortalEgresadosContext();

            ExperienciaLaboral experienciaLaboralToInsert = experienciaLaboral.Convert();

            await context
                .ExperienciaLaborals
                .AddAsync(experienciaLaboralToInsert);

            await context.SaveChangesAsync();

            return Results.Ok(experienciaLaboralToInsert.ExperienciaLaboralId);
        }
        catch (Exception ex)
        {
            return Utils.HandleError(ex);
        }
        finally
        {
            context?.Dispose();
        }
    }

    [Authorize]
    [HttpDelete]
    public async Task<IResult> DeleteExperienciaLaboralEgresado([FromQuery] int id)
    {
        PortalEgresadosContext? context = null;

        try
        {
            context = new PortalEgresadosContext();

            ExperienciaLaboral experiencia = await context.ExperienciaLaborals
                .FirstOrDefaultAsync(e =>
                    e.ExperienciaLaboralId == id
                )
            ?? new();

            context
                .ExperienciaLaborals
                .Remove(experiencia);

            await context.SaveChangesAsync();

            return Results.Ok(true);
        }
        catch (Exception ex)
        {
            return Utils.HandleError(ex);
        }
        finally
        {
            context?.Dispose();
        }
    }
}
