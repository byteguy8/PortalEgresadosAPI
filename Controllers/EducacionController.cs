using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PortalEgresadosAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class EducacionController : ControllerBase
{

    [HttpGet("{egresadoId}")]
    public IResult EducacionEgresado(int egresadoId)
    {

        PortalEgresadosContext? context;

        try
        {
            context = new PortalEgresadosContext();

            var getEducaciones = context
                .Educacions
                .Where(e => e.EgresadoId == egresadoId)
                .Include(e => e.Formacion)
                .ToList();

            var Educaciones = new List<dynamic>();

            foreach (var educacion in getEducaciones)
            {
                dynamic e = new
                {
                    id = educacion.EducacionId,
                    organizacion = educacion.Organizacion,
                    formacionId = educacion.Formacion.FormacionId,
                    nivel = educacion.Formacion.Nombre,

                };

                Educaciones.Add(e);
            }

            return Results.Json(
                data: Educaciones,
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