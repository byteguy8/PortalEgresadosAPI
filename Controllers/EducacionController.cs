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
                    fechaEntrada = educacion.FechaEntrada,
                    fechaSalida = educacion.FechaSalida
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
                data: new ErrorResult(0, "Error no esperado"),
                statusCode: StatusCodes.Status500InternalServerError
            );

        }


    }

    [HttpPost]
    public IResult AgregarEducacion([FromBody] EducacionPOSTDTO nuevaEducacionDto)
    {
        try
        {
            using (var context = new PortalEgresadosContext())
            {
                var educacionExistente = context.Educacions
                    .FirstOrDefault(e => e.EgresadoId == nuevaEducacionDto.EgresadoId && e.FormacionId == nuevaEducacionDto.FormacionId);

                if (educacionExistente != null)
                {
                    return Results.Json(
                        data: new ErrorResult(0, "Ya existe un registro con el mismo EgresadoId y FormacionId"),
                        statusCode: StatusCodes.Status409Conflict
                    );
                }

                var nuevaEducacion = new Educacion
                {
                    EgresadoId = nuevaEducacionDto.EgresadoId,
                    FormacionId = nuevaEducacionDto.FormacionId,
                    FechaEntrada = nuevaEducacionDto.FechaEntrada,
                    FechaSalida = nuevaEducacionDto.FechaSalida,
                    Organizacion = nuevaEducacionDto.Organizacion,
                };

                context.Educacions.Add(nuevaEducacion);
                context.SaveChanges();
            }

            return Results.Json(
                data: nuevaEducacionDto,
                statusCode: StatusCodes.Status201Created
            );
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);

            return Results.Json(
                data: new ErrorResult(0, "Error no esperado"),
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }

    [HttpDelete("{educacionId}")]
    public IResult EliminarEducacion(int educacionId)
    {
        try
        {
            using (var context = new PortalEgresadosContext())
            {
                var educacion = context.Educacions.FirstOrDefault(e => e.EducacionId == educacionId);
                if (educacion != null)
                {
                    context.Educacions.Remove(educacion);
                    context.SaveChanges();
                }
            }

            return Results.Json(
                data: true,
                statusCode: StatusCodes.Status200OK
            );
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);

            return Results.Json(
                data: new ErrorResult(0, "Error no esperado"),
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }

}
