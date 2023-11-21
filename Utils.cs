using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PortalEgresadosAPI;

public class Utils
{
    public static string key = "zFx5iAfzYwzVs6mdamU9GQ3amdx+iftwkLD70Abq8kEfI77TEUzXgLClzMnFOZml09nnpKpeYkdYRzKNkK+nLg==";

    public static IResult HandleError(Exception ex)
    {
        Console.Error.WriteLine(ex.Message);
        Console.Error.WriteLine(ex.ToString());

        if (ex is APIException error)
        {
            return Results.Json(
                data: new ErrorMsg(error.Message),
                statusCode: error.Status
            );
        }

        return Results.Json(
            data: new ErrorMsg("Hubo un error al procesar la solicitud. Intentelo de nuevo"),
            statusCode: StatusCodes.Status500InternalServerError
        );
    }

    public static APIException APIError(string message, int status)
    {
        return new APIException(status, message);
    }

    public static byte[] GenerateSalt(int size)
    {
        byte[] salt = new byte[size];

        using var rand = RandomNumberGenerator.Create();

        rand.GetBytes(salt);

        return salt;
    }

    public static byte[] GenerateSalt()
    {
        return GenerateSalt(32);
    }

    public static byte[] HashPasswordWithSalt(string password, byte[] salt)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(
            password,
            salt,
            10000,
            HashAlgorithmName.SHA512
        );

        return pbkdf2.GetBytes(32);
    }

    public static byte[] HASH256(string input)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        return SHA256.HashData(inputBytes);
    }

    public static bool isTokenAdministrator(ClaimsPrincipal claims)
    {
        var claimRol = claims.FindFirst("email")
            ?? throw APIError(
                "Error inesperado al procesar identificacion",
                StatusCodes.Status400BadRequest
            );

        return claimRol.ValueType == "Administrador";
    }

    public static bool IsTokenAuthorizedEmail(PortalEgresadosContext context, int egresadoId, ClaimsPrincipal claims)
    {
        var claimEmail = claims.FindFirst("identificacion")
            ?? throw APIError(
                "Error inesperado al procesar identificacion",
                StatusCodes.Status400BadRequest
            );

        var rawContacto = context
            .Contactos
            .Include(c => c.Participante)
            .ThenInclude(p => p.Egresado)
            .FirstOrDefault(c => c.Nombre == claimEmail.Value)
            ?? throw APIError(
                "No se pudo verificar la informacion del egresado",
                StatusCodes.Status400BadRequest
            );

        var rawParticipante = rawContacto.Participante;
        var rawEgresado = rawParticipante.Egresado
            ?? throw APIError(
                "No se pudo verificar la informacion del egresado",
                StatusCodes.Status400BadRequest
            );

        return rawEgresado.EgresadoId == egresadoId;
    }

    public static dynamic ObtenerInfEgresado(PortalEgresadosContext context, Egresado rawEgresado, bool fullDestacado = false)
    {
        // Obteniendo los documentos del egresado
        var rawDocumentos = context
            .Documentos
            .Where(ed => ed.ParticipanteId == rawEgresado.ParticipanteId)
            .Include(ed => ed.TipoDocumento)
            .ToList();

        var documentos = new List<dynamic>();

        foreach (var rawDocumento in rawDocumentos)
        {
            dynamic documento = new
            {
                documentoId = rawDocumento.DocumentoId,
                participanteId = rawDocumento.ParticipanteId,
                tipoDocId = rawDocumento.TipoDocumentoId,
                documentoNo = rawDocumento.DocumentoNo,
                tipoDoc = rawDocumento.TipoDocumento.Nombre
            };

            documentos.Add(documento);
        }

        // Obteniendo los contactos del egresado
        var rawContactos = context
            .Contactos
            .Where(ed => ed.ParticipanteId == rawEgresado.ParticipanteId)
            .Include(ed => ed.TipoContacto)
            .ToList();

        var contactos = new List<dynamic>();

        foreach (var rawContacto in rawContactos)
        {
            dynamic contacto = new
            {
                contactoId = rawContacto.ContactoId,
                participanteId = rawContacto.ParticipanteId,
                tipoContactoId = rawContacto.TipoContactoId,
                nombre = rawContacto.Nombre,
                tipoContacto = rawContacto.TipoContacto.Nombre
            };

            contactos.Add(contacto);
        }

        // Obteniendo los idiomas del egresado
        var rawIdiomas = context
            .EgresadoIdiomas
            .Where(i => i.EgresadoId == rawEgresado.EgresadoId)
            .Include(i => i.Idioma)
            .ToList();

        var idiomas = new List<dynamic>();

        foreach (var rawIdioma in rawIdiomas)
        {
            dynamic idioma = new
            {
                id = rawIdioma.IdiomaId,
                nombre = rawIdioma.Idioma.Nombre,
                egresadIdiomaId = rawIdioma.EgresadoIdiomaId
            };

            idiomas.Add(idioma);
        }

        // Obteniendo las experiencias laborales del egresado
        var rawExperienciaLaborales = context
            .ExperienciaLaborals
            .Where(e => e.EgresadoId == rawEgresado.EgresadoId)
            .ToList();

        var experiencias = new List<dynamic>();

        foreach (var rawExperienciaLaboral in rawExperienciaLaborales)
        {
            dynamic experienciaLaboral = new
            {
                id = rawExperienciaLaboral.ExperienciaLaboralId,
                organizacion = rawExperienciaLaboral.Organizacion,
                posicion = rawExperienciaLaboral.Posicion,
                fechantrada = rawExperienciaLaboral.FechaEntrada,
                fechaSalida = rawExperienciaLaboral.FechaSalida
            };

            experiencias.Add(experienciaLaboral);
        }

        // Obteniendo las educaciones del egresado
        var rawEducaciones = context
            .Educacions
            .Where(e => e.EgresadoId == rawEgresado.EgresadoId)
            .Include(e => e.Formacion)
            .ToList();

        var educaciones = new List<dynamic>();

        foreach (var rawEducacion in rawEducaciones)
        {
            dynamic educacion = new
            {
                id = rawEducacion.EducacionId,
                organizacion = rawEducacion.Organizacion,
                formacionId = rawEducacion.Formacion.FormacionId,
                nivel = rawEducacion.Formacion.Nombre,
            };

            educaciones.Add(educacion);
        }

        // Obteniendo las habilidades del egresado
        var rawHabilidades = context
            .EgresadoHabilidads
            .Where(eh => eh.EgresadoId == rawEgresado.EgresadoId)
            .Include(eh => eh.Habilidad)
            .ToList();

        var habilidades = new List<dynamic>();

        foreach (var rawHabilidad in rawHabilidades)
        {
            dynamic habilidad = new
            {
                id = rawHabilidad.HabilidadId,
                valor = rawHabilidad.Habilidad.Nombre,
                egresadoHabilidadId = rawHabilidad.EgresadoHabilidadId
            };

            habilidades.Add(habilidad);
        }

        // Determinando si el egresado es destacado
        var destacado = false;
        var infDestacado = new List<dynamic>();

        if (fullDestacado)
        {
            var rawEgresadoDestacado = context
                .EgresadoDestacados
                .Where(ed => ed.EgresadoId == rawEgresado.EgresadoId)
                .ToList();

            if (rawEgresadoDestacado.Any())
            {
                foreach (var edestacado in rawEgresadoDestacado)
                {
                    var egresadoHasta = edestacado.FechaHasta;

                    if (egresadoHasta >= DateTime.Now)
                    {
                        destacado = true;
                    }

                    dynamic ed = new
                    {
                        Observacion = edestacado.Observacion,
                        FechaDesde = edestacado.FechaDesde,
                        FechaHasta = edestacado.FechaHasta,
                        Destacado = destacado
                    };

                    infDestacado.Add(ed);
                }
            }
        }
        else
        {
            destacado = context
                .EgresadoDestacados
                .Where(d =>
                    d.EgresadoId == rawEgresado.EgresadoId &&
                    d.FechaHasta >= DateTime.Now
                ).Any();
        }

        var ciudadDelEgresado = context
            .Egresados
            .Where(e => e.EgresadoId == rawEgresado.EgresadoId)
            .Select(e => e.Participante.Direccion.LocalidadPostal.Ciudad)
            .FirstOrDefault();

        var ciudad = "";

        if (ciudadDelEgresado != null)
        {
            ciudad = ciudadDelEgresado.Nombre;
        }
        else
        {
            ciudad = "Ciudad No Registrada";
        }

        var egresadoId = rawEgresado.EgresadoId;
        var primerNombre = rawEgresado.PrimerNombre;
        var segundoNombre = rawEgresado.SegundoNombre;
        var primerApellido = rawEgresado.PrimerApellido;
        var segundoApellido = rawEgresado.SegundoApellido;
        var genero = rawEgresado.Genero;
        var fechaNac = rawEgresado.FechaNac;
        var fotoPerfilUrl = rawEgresado.Participante.FotoPerfilUrl;
        var acerca = rawEgresado.Acerca;
        var activo = rawEgresado.Estado;
        var nacionalidad = rawEgresado.NacionalidadNavigation.Nombre;

        dynamic egresado = new
        {
            EgresadoId = egresadoId,
            PrimerNombre = primerNombre,
            SegundoNombre = segundoNombre,
            PrimerApellido = primerApellido,
            SegundoApellido = segundoApellido,
            DocumentoEgresados = documentos,
            Genero = genero,
            FechaNac = fechaNac,
            FotoPerfilUrl = fotoPerfilUrl,
            Acerca = acerca,
            Estado = activo,
            Destacado = (object)(fullDestacado ? infDestacado : destacado),
            Nacionalidad = nacionalidad,
            EgresadoIdiomas = idiomas,
            ExperienciaLaborals = experiencias,
            Educacions = educaciones,
            Contacto = contactos,
            Habilidades = habilidades,
            Ciudad = ciudad
        };

        return egresado;
    }

    public static TipoDocumento ObtenerTipoDocumento(
        PortalEgresadosContext context,
        TipoIdentidad tipo
    )
    {
        string strTipoDocumento;

        if (tipo == TipoIdentidad.CEDULA)
        {
            strTipoDocumento = "Documento de Identidad";
        }
        else
        {
            strTipoDocumento = "Pasaporte";
        }

        var rawTipoDocumento = context
            .TipoDocumentos
            .FirstOrDefault(t =>
                t.Nombre == strTipoDocumento
        )
        ?? throw APIError(
            "Hubo un error al procesar la solicitud. Intentelo de nuevo",
            StatusCodes.Status500InternalServerError
        );

        return rawTipoDocumento;
    }

    public static void ValidarExistenciaDocumento(
        PortalEgresadosContext context,
        TipoIdentidad tipo,
        string documento
    )
    {
        var existe = context
            .Documentos
            .Where(d =>
                d.DocumentoNo == documento
            )
            .Any();

        if (existe)
        {
            var msg = "La cedula suministrada no esta disponible para su uso";

            if (tipo == TipoIdentidad.PASAPORTE)
            {
                msg = "El pasaporte suministrado no esta disponible para su uso";
            }

            throw APIError(msg, StatusCodes.Status400BadRequest);
        }
    }

    public static Documento CrearDocumento(
        PortalEgresadosContext context,
        int participanteId,
        TipoIdentidad tipo,
        string documento
    )
    {
        if (documento.Length == 0)
        {
            throw APIError(
                "La informacion de los documentos de identidad no puede estar vacia",
                StatusCodes.Status400BadRequest
            );
        }

        ValidarExistenciaDocumento(context, tipo, documento);

        var rawTipoDocumento = ObtenerTipoDocumento(context, tipo);

        var resultCrearDocumento = context
            .Documentos
            .Add(new Documento
            {
                DocumentoNo = documento,
                ParticipanteId = participanteId,
                TipoDocumentoId = rawTipoDocumento.TipoDocumentoId
            })
            ?? throw APIError(
                "Hubo un error al procesar la solicitud. Intentelo de nuevo",
                StatusCodes.Status500InternalServerError
            );

        context.SaveChanges();

        return resultCrearDocumento.Entity;
    }

    public static Documento ModificarDocumento(
        PortalEgresadosContext context,
        int participanteId,
        TipoIdentidad tipo,
        string documento
    )
    {
        if (documento.Length == 0)
        {
            throw APIError(
                "La informacion de los documentos de identidad no puede estar vacia",
                StatusCodes.Status400BadRequest
            );
        }

        var rawTipoDocument = ObtenerTipoDocumento(context, tipo);

        var rawDocumento = context
            .Documentos
            .FirstOrDefault(d =>
                d.ParticipanteId == participanteId &&
                d.TipoDocumentoId == rawTipoDocument.TipoDocumentoId
            );

        if (rawDocumento == null)
        {
            return CrearDocumento(context, participanteId, tipo, documento);
        }

        ValidarExistenciaDocumento(context, tipo, documento);

        rawDocumento.DocumentoNo = documento;

        context.SaveChanges();

        return rawDocumento;
    }

    public static void EliminarDocumento(
        PortalEgresadosContext context,
        int ParticipanteId,
        TipoIdentidad tipo
    )
    {
        var rawTipoDocumento = ObtenerTipoDocumento(context, tipo);

        var rawDocumento = context
            .Documentos
            .FirstOrDefault(d =>
                d.ParticipanteId == ParticipanteId &&
                d.TipoDocumentoId == rawTipoDocumento.TipoDocumentoId
            );

        if (rawDocumento != null)
        {
            context
                .Documentos
                .Remove(rawDocumento);

            context.SaveChanges();
        }
    }

    public static LocalidadPostal ObtenerMunicipio(
        PortalEgresadosContext context,
        string provincia
    )
    {
        // Verificando si existe la provincia.
        var rawProvincia = context
            .Ciudads
            .FirstOrDefault(c =>
                c.Nombre == provincia
            )
            ?? throw APIError(
                $"No existe la provincia con el nombre '{provincia}'",
                StatusCodes.Status400BadRequest
            );

        // Verificando si existe el municipio
        var rawMunicipio = context
            .LocalidadPostals
            .FirstOrDefault(m =>
                m.CiudadId == rawProvincia.CiudadId
            )
            ?? throw APIError(
                $"No se pudo obtener informacion del municipio",
                StatusCodes.Status500InternalServerError
            );

        return rawMunicipio;
    }

    public static Direccion CrearProvincia(
        PortalEgresadosContext context,
        string provincia
    )
    {
        var rawMunicipio = ObtenerMunicipio(context, provincia);

        var resultCrearDireccion = context
            .Direccions
            .Add(new Direccion
            {
                LocalidadPostalId = rawMunicipio.LocalidadPostalId,
                DireccionPrincipal = ""
            })
            ?? throw APIError(
                "Hubo un error al procesar la solicitud. Intentelo de nuevo",
                StatusCodes.Status500InternalServerError
            );

        context.SaveChanges();

        return resultCrearDireccion.Entity;
    }

    public static Usuario CrearUsuario(
        PortalEgresadosContext context,
        string rol,
        string username,
        string password
    )
    {
        // Verificando si existe el rol.
        var rawRol = context
            .Rols
            .FirstOrDefault(r =>
                r.Nombre.ToUpper() == rol.ToUpper()
            )
            ?? throw APIError(
                $"No existe el rol '{rol}'",
                StatusCodes.Status400BadRequest
            );

        // Verificando si ya existe un usuario con el mismo nombre.
        var existeUsuario = context
            .Usuarios
            .Where(u =>
                EF.Functions.Like(u.UserName, $"%{username}%")
            )
            .Any();

        if (existeUsuario)
        {
            throw APIError(
                $"El nombre usuario suministrado no esta disponible",
                StatusCodes.Status400BadRequest
            );
        }

        byte[] byteSalt = GenerateSalt();
        byte[] byteHashPassword = HashPasswordWithSalt(password, byteSalt);

        // Creando el usuario
        var resultCrearUsuario = context
            .Usuarios
            .Add(new Usuario
            {
                RolId = rawRol.RolId,
                UserName = username,
                Password = Convert.ToHexString(byteHashPassword),
                Salt = Convert.ToHexString(byteSalt)
            })
            ?? throw APIError(
                "Hubo un error al procesar la solicitud. Intentelo de nuevo",
                StatusCodes.Status500InternalServerError
            );

        context.SaveChanges();

        return resultCrearUsuario.Entity;
    }
}