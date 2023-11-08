using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalEgresadosAPI.Database.DTO;
using System.Diagnostics.Contracts;

namespace PortalEgresadosAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ContactoController : ControllerBase
{

    [HttpGet("{egresadoId}")]
    public IResult DeleteContactoEgresado(int egresadoId)
    {

        PortalEgresadosContext? context;

        try
        {
            context = new PortalEgresadosContext();

            var EgresadoContacto = context
                    .Contactos
                    .Where(ed => ed.ParticipanteId == egresadoId)
                    .ToList();

                return Results.Json(
                data: EgresadoContacto,
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
    public async Task<IResult> CreateContactoEgresado([FromBody] ContactoPOSTDTO contacto)
    {
        try
        {
            var context = new PortalEgresadosContext();

            var toCreateContact = contacto.ToContacto();

            var createdContact = await context.Contactos.AddAsync(toCreateContact);

            await context.SaveChangesAsync();

            return Results.Json(toCreateContact.ContactoId, statusCode: StatusCodes.Status200OK);
        }
        catch ( Exception ex)
        {
            Console.Error.WriteLine(ex.Message);

            return Results.Json(
                data: new ErrorResult(0, "Unexpected server error"),
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
       
    }

    [HttpDelete("{contactoId}")]
    public async Task<IResult> ContactoEgresado(int contactoId)
    {
        try
        {
            var context = new PortalEgresadosContext();

            var contactToDelete = await context.Contactos.FirstOrDefaultAsync(x => x.ContactoId == contactoId) ?? new Contacto();

            context.Contactos.Remove(contactToDelete);

            await context.SaveChangesAsync();

            return Results.Json(true, statusCode: StatusCodes.Status200OK);
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