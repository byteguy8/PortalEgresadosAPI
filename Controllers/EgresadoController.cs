using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace PortalEgresadosAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class EgresadoController : Controller
{
    [Authorize]
    [HttpPut("FotoPerfil/{egresadoId}")]
    public async Task<IResult> SubirFotoPerfil(int egresadoId, [FromForm] IFormFile foto)
    {
        PortalEgresadosContext? context = null;

        try
        {
            context = new PortalEgresadosContext();

            if (!Utils.IsTokenAuthorizedEmail(context, egresadoId, User))
            {
                throw Utils.APIError(
                    "Sin autorizacion para realizar la accion",
                    StatusCodes.Status400BadRequest
                );
            }

            var rawEgresado = await context
                .Egresados
                .Include(e => e.Participante)
                .FirstOrDefaultAsync(e =>
                    e.EgresadoId == egresadoId
                )
            ?? throw Utils.APIError(
                "No existen registros para el egresado suministrado",
                StatusCodes.Status400BadRequest
            );

            if (foto == null)
            {
                throw Utils.APIError(
                    "No se suministro los binarios de la foto de perfil",
                    StatusCodes.Status400BadRequest
                );
            }

            using var inputStream = new MemoryStream();

            await foto.CopyToAsync(inputStream);
            inputStream.Position = 0;

            var account = new Account(
                "dbxrtahks",
                "789383338239912",
                "Em6hfrsc5y6qzIqQFc6HSqCVswg"
            );

            var cloudinary = new Cloudinary(account);

            var nombreFotoBytes = Utils.HASH256($"{rawEgresado.EgresadoId}");
            var nombreFoto = Convert.ToHexString(nombreFotoBytes);

            var uploadParameters = new ImageUploadParams()
            {
                File = new FileDescription("Foto perfil egresado", inputStream),
                PublicId = nombreFoto
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParameters);

            var urlFoto = uploadResult.Url ?? throw Utils.APIError(
                "Hubo un error al procesar la solicitud. Intentelo de nuevo",
                StatusCodes.Status400BadRequest
            );

            rawEgresado
                .Participante
                .FotoPerfilUrl = urlFoto.ToString();

            await context.SaveChangesAsync();

            return Results.Ok(urlFoto.ToString());
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

    [HttpGet("{IdEgresado}")]
    public IResult BuscarEgresado(int IdEgresado)
    {
        PortalEgresadosContext? context = null;

        try
        {
            context = new PortalEgresadosContext();

            var rawEgresado = context
                .Egresados
                .Where(b => b.EgresadoId == IdEgresado)
                .Include(e => e.Participante)
                .Include(e => e.NacionalidadNavigation)
                .OrderBy(b => b.PrimerNombre)
                .FirstOrDefault()
            ?? throw Utils.APIError(
                "No existen registros para el egresado suministrado",
                StatusCodes.Status400BadRequest
            );

            return Results.Ok(Utils.ObtenerInfEgresado(context, rawEgresado, true));
        }

        catch (Exception ex)
        {
            return Utils.HandleError(ex);
        }
    }

    [HttpGet("{limit}/{offset}")]
    public IResult BuscarEgresados([FromQuery] string? valor, int limit, int offset)
    {
        PortalEgresadosContext? context = null;

        try
        {
            context = new PortalEgresadosContext();

            var rawEgresados = context
                .Egresados
                .Where(e => EF.Functions.Like(e.PrimerNombre, $"%{valor}%") ||
                    EF.Functions.Like(e.PrimerApellido, $"%{valor}%"))
                .Include(e => e.Participante)
                .Include(e => e.NacionalidadNavigation)
                .OrderBy(b => b.PrimerNombre)
                .Skip(offset * limit)
                .Take(limit)
                .ToList()
            ?? throw Utils.APIError(
                "Hubo un error al procesar la solicitud. Intentelo de nuevo",
                StatusCodes.Status500InternalServerError
            );

            var egresados = new List<dynamic>();

            foreach (var rawEgresado in rawEgresados)
            {
                var egresado = Utils.ObtenerInfEgresado(context, rawEgresado);
                egresados.Add(egresado);
            }

            return Results.Ok(egresados);
        }

        catch (Exception ex)
        {
            return Utils.HandleError(ex);
        }
    }

    [Authorize]
    [HttpPost]
    public IResult Agregar(EgresadoPOSTDTO egresado)
    {
        PortalEgresadosContext? context = null;
        IDbContextTransaction? transaction = null;

        try
        {
            context = new PortalEgresadosContext();
            transaction = context.Database.BeginTransaction();

            if (!Utils.isTokenAdministrator(User))
            {
                throw Utils.APIError(
                    "Sin autorizacion para realizar la accion",
                    StatusCodes.Status400BadRequest
                );
            }

            // Validando la informacion del genero
            if (egresado.Genero != "M" && egresado.Genero != "F")
            {
                throw Utils.APIError(
                    $"Valor para genero incorrecto. Debe ser M (masculino) o F (femenino).",
                    StatusCodes.Status400BadRequest
                );
            }

            // Validando la informacion del pasaporte. Se debe suministrar uno o ambos, pero no ninguno
            if (egresado.Pasaporte == null && egresado.Cedula == null)
            {
                throw Utils.APIError(
                    "Se espera informacion de la cedula y/o pasaporte, pero ninguno de los dos esta",
                    StatusCodes.Status400BadRequest
                );
            }

            // Validando longitud de matriculas
            if (egresado.MatriculaEgresado.Length == 0 || egresado.MatriculaGrado.Length == 0 ||
            egresado.MatriculaEgresado.Length > 11 || egresado.MatriculaGrado.Length > 11)
            {
                throw Utils.APIError(
                    "Ambas matriculas deben tener una longitud mayor que 0 e igual o menor que 11",
                    StatusCodes.Status400BadRequest
                );
            }

            // Verificando si existe el email
            var existeEmail = context
                .Contactos
                .Where(c => c.Nombre == egresado.Email)
                .Count() == 1;

            if (existeEmail)
            {
                throw Utils.APIError(
                    $"El email '${egresado.Email}' no se encuentra disponible",
                    StatusCodes.Status400BadRequest
                );
            }

            // Verificando si existe la nacionalidad.
            var rawNacionalidad = context
                .Pais
                .FirstOrDefault(n =>
                    n.Nombre == egresado.Nacionalidad
                )
            ?? throw Utils.APIError(
                $"No existe la nacionalidad con el nombre '{egresado.Nacionalidad}'",
                StatusCodes.Status400BadRequest
            );

            // Verificando si existe el tipo de participante.
            var rawTipoParticipante = context
                .TipoParticipantes
                .FirstOrDefault(t =>
                    t.Nombre == egresado.TipoParticipante
                )
            ?? throw Utils.APIError(
                $"No existe el tipo de participante '{egresado.TipoParticipante}'",
                StatusCodes.Status400BadRequest
            );

            // Verificando si existe alguna o ambas matriculas
            var existenMatriculas = context
                .Egresados
                .Where(e =>
                    e.MatriculaEgresado == egresado.MatriculaEgresado ||
                    e.MatriculaEgresado == egresado.MatriculaGrado ||
                    e.MatriculaGrado == egresado.MatriculaEgresado ||
                    e.MatriculaGrado == egresado.MatriculaGrado
                )
                .Any();

            if (existenMatriculas)
            {
                throw Utils.APIError(
                    $"Ya existe un egresado registrado con alguna de las matriculas suministradas",
                    StatusCodes.Status400BadRequest
                );
            }

            // Creado usuario
            var rawUsuario = Utils.CrearUsuario(
                context,
                egresado.Rol,
                egresado.UserName,
                egresado.Password
            );

            var rawDireccion = Utils.CrearProvincia(context, egresado.Provincia);

            // Creando Participante
            var resultCrearParticipante = context
                .Participantes
                .Add(new Participante
                {
                    TipoParticipanteId = rawTipoParticipante.TipoParticipanteId,
                    UsuarioId = rawUsuario.UsuarioId,
                    EsEgresado = true,
                    DireccionId = rawDireccion.DireccionId
                })
            ?? throw Utils.APIError(
                "Hubo un error al procesar la solicitud. Intentelo de nuevo",
                StatusCodes.Status500InternalServerError
            );

            context.SaveChanges();

            var rawParticipante = resultCrearParticipante.Entity;

            // Creando documentos
            if (egresado.Cedula != null)
            {
                Utils.CrearDocumento(
                    context,
                    rawParticipante.ParticipanteId,
                    TipoIdentidad.CEDULA,
                    egresado.Cedula
                );
            }

            if (egresado.Pasaporte != null)
            {
                Utils.CrearDocumento(
                    context,
                    rawParticipante.ParticipanteId,
                    TipoIdentidad.PASAPORTE,
                    egresado.Pasaporte
                );
            }

            // Creando Egresado
            var nuevoEgresado = new Egresado
            {
                ParticipanteId = rawParticipante.ParticipanteId,
                Nacionalidad = rawNacionalidad.PaisId,
                PrimerNombre = egresado.PrimerNombre,
                SegundoNombre = egresado.SegundoNombre,
                PrimerApellido = egresado.PrimerApellido,
                SegundoApellido = egresado.SegundoApellido,
                Genero = egresado.Genero,
                FechaNac = egresado.FechaNacimiento,
                MatriculaEgresado = egresado.MatriculaEgresado,
                MatriculaGrado = egresado.MatriculaGrado
            };

            var resultCrearEgresado = context
                .Egresados
                .Add(nuevoEgresado)
            ?? throw Utils.APIError(
                "Hubo un error al procesar la solicitud. Intentelo de nuevo",
                StatusCodes.Status500InternalServerError
            );

            context.SaveChanges();

            transaction.Commit();

            return Results.Ok(resultCrearEgresado.Entity.EgresadoId);
        }
        catch (Exception ex)
        {
            transaction?.Rollback();
            return Utils.HandleError(ex);
        }
        finally
        {
            transaction?.Dispose();
            context?.Dispose();
        }
    }

    [Authorize]
    [HttpPut]
    public IResult ModificarEgresado(EgresadoPUTDTO egresado)
    {
        PortalEgresadosContext? context = null;
        IDbContextTransaction? transaction = null;

        try
        {
            context = new PortalEgresadosContext();
            transaction = context.Database.BeginTransaction();

            if (!Utils.IsTokenAuthorizedEmail(context, egresado.Id, User))
            {
                throw Utils.APIError(
                    "Sin autorizacion para realizar la accion",
                    StatusCodes.Status400BadRequest
                );
            }

            if (egresado.Genero != "M" && egresado.Genero != "F")
            {
                throw Utils.APIError(
                    "Valor para genero incorrecto. Debe ser M (masculino) o F (femenino).",
                    StatusCodes.Status400BadRequest
                );
            }

            if (egresado.Pasaporte == null && egresado.Cedula == null)
            {
                throw Utils.APIError(
                    "Se espera informacion de la cedula y/o pasaporte, pero ninguno de los dos esta",
                    StatusCodes.Status400BadRequest
                );
            }

            if (egresado.Cedula != null && egresado.Cedula.Length == 0 &&
            egresado.Pasaporte != null && egresado.Pasaporte.Length == 0)
            {
                throw Utils.APIError(
                    "Se espera informacion de la cedula y/o pasaporte, pero ninguno de los dos esta",
                    StatusCodes.Status400BadRequest
                );
            }

            var rawEgresado = context
                .Egresados
                .Include(e => e.Participante)
                .ThenInclude(p => p.Direccion)
                .FirstOrDefault(e => e.EgresadoId == egresado.Id)
            ?? throw Utils.APIError(
                "No existen registros para el egresado suministrado",
                StatusCodes.Status400BadRequest
            );

            var rawParticipante = rawEgresado.Participante;

            if (egresado.Cedula != null)
            {
                if (egresado.Cedula.Length > 0)
                {
                    Utils.ModificarDocumento(
                        context,
                        rawParticipante.ParticipanteId,
                        TipoIdentidad.CEDULA,
                        egresado.Cedula
                    );
                }
                else
                {
                    Utils.EliminarDocumento(
                        context,
                        rawParticipante.ParticipanteId,
                        TipoIdentidad.CEDULA
                    );
                }
            }

            if (egresado.Pasaporte != null)
            {
                if (egresado.Pasaporte.Length > 0)
                {
                    Utils.ModificarDocumento(
                        context,
                        rawParticipante.ParticipanteId,
                        TipoIdentidad.PASAPORTE,
                        egresado.Pasaporte
                    );
                }
                else
                {
                    Utils.EliminarDocumento(
                        context,
                        rawParticipante.ParticipanteId,
                        TipoIdentidad.PASAPORTE
                    );
                }
            }

            var rawMunicipio = Utils.ObtenerMunicipio(context, egresado.Provincia);

            rawParticipante
                .Direccion
                .LocalidadPostalId = rawMunicipio.LocalidadPostalId;

            rawEgresado.PrimerApellido = egresado.PrimerApellido;
            rawEgresado.SegundoApellido = egresado.SegundoApellido;
            rawEgresado.PrimerNombre = egresado.PrimerNombre;
            rawEgresado.SegundoNombre = egresado.SegundoNombre;
            rawEgresado.Genero = egresado.Genero;
            rawEgresado.FechaNac = egresado.FechaNac;
            rawEgresado.Acerca = egresado.Acerca;

            context.SaveChanges();

            transaction.Commit();

            return Results.Ok();
        }
        catch (Exception ex)
        {
            transaction?.Rollback();
            return Utils.HandleError(ex);
        }
        finally
        {
            transaction?.Dispose();
            context?.Dispose();
        }
    }
}