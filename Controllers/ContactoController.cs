using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PortalEgresadosAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ContactoController : ControllerBase
{

    [HttpGet("{egresadoId}")]
    public IResult ContactoEgresado(int egresadoId)
    {

        PortalEgresadosContext? context;

        try
        {
            context = new PortalEgresadosContext();

                            var EgresadoContacto = context
                    .Contactos
                    .Where(ed => ed.ParticipanteId == egresadoId)
                    .Include(ed => ed.TipoContacto)
                    .ToList();

                var contacto = new List<dynamic>();

                foreach (var contactos in EgresadoContacto)
                {
                    dynamic c = new
                    {
                        contactoId = contactos.ContactoId,
                        participanteId = contactos.ParticipanteId,
                        tipoContactoId = contactos.TipoContactoId,
                        nombre = contactos.Nombre,
                        tipoContacto = contactos.TipoContacto.Nombre

                    };

                    contacto.Add(c);

                }

                return Results.Json(
                data: contacto,
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