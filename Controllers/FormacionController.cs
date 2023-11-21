using Microsoft.AspNetCore.Mvc;

namespace PortalEgresadosAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FormacionController : ControllerBase
{
    [HttpPost]
    public IResult CrearFormacion(CrearFormacionDTO formacion)
    {
        PortalEgresadosContext? context = null;

        try
        {
            context = new PortalEgresadosContext();

            var rawNivel = context
                .Nivels
                .FirstOrDefault(n => n.Nombre == formacion.Nivel);

            if (rawNivel == null)
            {
                var niveles = string.Join(", ", Utils.ObtenerNiveles(context));

                throw Utils.APIError(
                   $"Nivel incorrecto. Los valores aceptados son: {niveles}",
                    StatusCodes.Status400BadRequest
                );
            }

            var existe = context
                .Formacions
                .Where(f =>
                    f.NivelId == rawNivel.NivelId &&
                    f.Nombre == formacion.Nombre
                )
                .Any();

            if (existe)
            {
                throw Utils.APIError(
                   "Ya existe una formacion con los valores suministrados",
                    StatusCodes.Status400BadRequest
                );
            }

            var rawFormacion = new Formacion
            {
                NivelId = rawNivel.NivelId,
                Nombre = formacion.Nombre
            };

            context.Add(rawFormacion);
            context.SaveChanges();

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