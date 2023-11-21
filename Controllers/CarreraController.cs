using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace PortalEgresadosAPI;

[ApiController]
[Route("[controller]")]

public class CarreraController : Controller
{
    [HttpGet]
    public IResult ObtenerCarreras()
    {
        try
        {
            using (var context = new PortalEgresadosContext())
            {
                var formaciones = context.Formacions
                    .Include(f => f.Nivel)
                    .OrderBy(f => f.Nombre)
                    .ToList();

                var Carreras = new List<dynamic>();

                foreach (var formacion in formaciones)
                {
                    dynamic c = new
                    {
                        id = formacion.FormacionId,
                        NombreCarrera = formacion.Nombre,
                        Nivel = formacion.Nivel.Nombre
                    };

                    Carreras.Add(c);
                }

                return Results.Json(
                    data: Carreras,
                    statusCode: StatusCodes.Status200OK
                );
            }
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