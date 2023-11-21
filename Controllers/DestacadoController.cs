using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PortalEgresadosAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class DestacadoController : ControllerBase
{
    [HttpGet]
    public IResult ObtenerDestacados()
    {
        PortalEgresadosContext? context = null;

        try
        {
            context = new PortalEgresadosContext();

            var rawDestacados = context
                .EgresadoDestacados
                .Include(d => d.Egresado.Participante)
                .Include(d => d.Egresado.NacionalidadNavigation)
                .Where(d => d.FechaHasta >= DateTime.Now)
                .OrderBy(d => d.Egresado.PrimerNombre)
                .ToList()
            ?? throw Utils.APIError(
                0,
                "Hubo un error al procesar la solicitud. Intentelo de nuevo",
                StatusCodes.Status500InternalServerError
            );

            var egresados = new List<dynamic>();

            foreach (var rawDestacado in rawDestacados)
            {
                var rawEgresado = rawDestacado.Egresado;
                egresados.Add(Utils.ObtenerInfEgresado(context, rawEgresado));
            }

            return Results.Ok(egresados);
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