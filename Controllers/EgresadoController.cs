using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PortalEgresadosAPI;


[ApiController]
[Route("[controller]")]

public class EgresadoController : Controller
{
    private enum TipoIdentidad
    {
        CEDULA,
        PASAPORTE
    }

    private TipoDocumento ObtenerTipoDocumento(
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
        ?? throw Utils.APIError(
            0,
            "Hubo un error al procesar la solicitud. Intentelo de nuevo",
            StatusCodes.Status500InternalServerError
        );

        return rawTipoDocumento;
    }

    private void ValidarExistenciaDocumento(
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

            throw Utils.APIError(0, msg, StatusCodes.Status400BadRequest);
        }
    }

    private Documento CrearDocumento(
        PortalEgresadosContext context,
        int participanteId,
        TipoIdentidad tipo,
        string documento
    )
    {
        if (documento.Length == 0)
        {
            throw Utils.APIError(
                0,
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
            ?? throw Utils.APIError(
                0,
                "Hubo un error al procesar la solicitud. Intentelo de nuevo",
                StatusCodes.Status500InternalServerError
            );

        context.SaveChanges();

        return resultCrearDocumento.Entity;
    }

    private Documento ModificarDocumento(
        PortalEgresadosContext context,
        int participanteId,
        TipoIdentidad tipo,
        string documento
    )
    {
        if (documento.Length == 0)
        {
            throw Utils.APIError(
                0,
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

    private void EliminarDocumento(
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

    private LocalidadPostal ObtenerMunicipio(
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
            ?? throw Utils.APIError(
                0,
                $"No existe la provincia con el nombre '{provincia}'",
                StatusCodes.Status400BadRequest
            );

        // Verificando si existe el municipio
        var rawMunicipio = context
            .LocalidadPostals
            .FirstOrDefault(m =>
                m.CiudadId == rawProvincia.CiudadId
            )
            ?? throw Utils.APIError(
                0,
                $"No se pudo obtener informacion del municipio",
                StatusCodes.Status500InternalServerError
            );

        return rawMunicipio;
    }

    private Direccion CrearProvincia(
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
            ?? throw Utils.APIError(
                0,
                "Hubo un error al procesar la solicitud. Intentelo de nuevo",
                StatusCodes.Status500InternalServerError
            );

        context.SaveChanges();

        return resultCrearDireccion.Entity;
    }

    private Usuario CrearUsuario(
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
            ?? throw Utils.APIError(
                0,
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
            throw Utils.APIError(
                0,
                $"El nombre usuario suministrado no esta disponible",
                StatusCodes.Status400BadRequest
            );
        }

        byte[] byteSalt = Utils.GenerateSalt(32);
        byte[] byteHashPassword = Utils.HashPasswordWithSalt(password, byteSalt);

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
            ?? throw Utils.APIError(
                0,
                "Hubo un error al procesar la solicitud. Intentelo de nuevo",
                StatusCodes.Status500InternalServerError
            );

        context.SaveChanges();

        return resultCrearUsuario.Entity;
    }

    [HttpGet]
    public IResult ObtenerEgresados()
    {
        PortalEgresadosContext? context = null;

        try
        {
            context = new PortalEgresadosContext();

            var rawEgresados = context
                .Egresados
                .OrderBy(e => e.PrimerApellido)
                .ToList();

            if (rawEgresados == null)
            {
                return Results.Json(
                    data: "Hubo un error al procesar la solicitud. Intentelo de nuevo",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }

            var egresados = new List<dynamic>();

            foreach (var rawEgresado in rawEgresados)
            {
                var rawDocumentos = context
                    .Documentos
                    .Where(d =>
                        d.ParticipanteId == rawEgresado.ParticipanteId
                    )
                    .ToList()
                    ?? throw Utils.APIError(
                        0,
                        "Hubo un error al procesar la solicitud. Intentelo de nuevo",
                        StatusCodes.Status500InternalServerError
                    );

                var documentos = new List<string>();

                foreach (var rawDocumento in rawDocumentos)
                {
                    documentos.Add(rawDocumento.DocumentoNo);
                }

                dynamic egresado = new
                {
                    id = rawEgresado.EgresadoId,
                    primerApellido = rawEgresado.PrimerApellido,
                    segundoApellido = rawEgresado.SegundoApellido,
                    primerNombre = rawEgresado.PrimerNombre,
                    segundoNombre = rawEgresado.SegundoNombre,
                    genero = rawEgresado.Genero,
                    matriculaGrado = rawEgresado.MatriculaGrado,
                    matriculaEgresado = rawEgresado.MatriculaEgresado,
                    documentos = documentos
                };

                egresados.Add(egresado);
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

    [HttpPost]
    public IResult Agregar(EgresadoPOSTDTO egresado)
    {
        PortalEgresadosContext? context = null;
        IDbContextTransaction? transaction = null;

        try
        {
            context = new PortalEgresadosContext();
            transaction = context.Database.BeginTransaction();

            // Validando la informacion del genero
            if (egresado.Genero != "M" && egresado.Genero != "F")
            {
                throw Utils.APIError(
                    0,
                    $"Valor para genero incorrecto. Debe ser M (masculino) o F (femenino).",
                    StatusCodes.Status400BadRequest
                );
            }

            // Validando la informacion del pasaporte. Se debe suministrar uno o ambos, pero no ninguno
            if (egresado.Pasaporte == null && egresado.Cedula == null)
            {
                throw Utils.APIError(
                    0,
                    "Se espera informacion de la cedula y/o pasaporte, pero ninguno de los dos esta",
                    StatusCodes.Status400BadRequest
                );
            }

            // Validando longitud de matriculas
            if (egresado.MatriculaEgresado.Length == 0 || egresado.MatriculaGrado.Length == 0 ||
            egresado.MatriculaEgresado.Length > 11 || egresado.MatriculaGrado.Length > 11)
            {
                throw Utils.APIError(
                    0,
                    "Ambas matriculas deben tener una longitud mayor que 0 e igual o menor que 11",
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
                    0,
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
                    0,
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
                    0,
                    $"Ya existe un egresado registrado con alguna de las matriculas suministradas",
                    StatusCodes.Status400BadRequest
                );
            }

            // Creado usuario
            var rawUsuario = CrearUsuario(
                context,
                egresado.Rol,
                egresado.UserName,
                egresado.Password
            );

            var rawDireccion = CrearProvincia(context, egresado.Provincia);

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
                    0,
                    "Hubo un error al procesar la solicitud. Intentelo de nuevo",
                    StatusCodes.Status500InternalServerError
                );

            context.SaveChanges();

            var rawParticipante = resultCrearParticipante.Entity;

            // Creando documentos
            if (egresado.Cedula != null)
            {
                CrearDocumento(
                    context,
                    rawParticipante.ParticipanteId,
                    TipoIdentidad.CEDULA,
                    egresado.Cedula
                );
            }

            if (egresado.Pasaporte != null)
            {
                CrearDocumento(
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
                    0,
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

    [HttpPut("FotoPerfil/{egresadoId}")]
    public async Task<IResult> SubirFotoPerfil(int egresadoId, [FromForm] IFormFile foto)
    {
        PortalEgresadosContext? context = null;

        try
        {
            context = new PortalEgresadosContext();

            var rawEgresado = await context
                .Egresados
                .Include(e => e.Participante)
                .FirstOrDefaultAsync(e =>
                    e.EgresadoId == egresadoId
                )
                ?? throw Utils.APIError(
                    0,
                    "No existen registros para el egresado suministrado",
                    StatusCodes.Status400BadRequest
                );

            if (foto == null)
            {
                throw Utils.APIError(
                    0,
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
                0,
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

    [HttpPut]
    public IResult ModificarEgresado(EgresadoPUTDTO egresado)
    {
        PortalEgresadosContext? context = null;
        IDbContextTransaction? transaction = null;

        try
        {
            context = new PortalEgresadosContext();
            transaction = context.Database.BeginTransaction();

            if (egresado.Genero != "M" && egresado.Genero != "F")
            {
                throw Utils.APIError(
                    0,
                    "Valor para genero incorrecto. Debe ser M (masculino) o F (femenino).",
                    StatusCodes.Status400BadRequest
                );
            }

            if (egresado.Pasaporte == null && egresado.Cedula == null)
            {
                throw Utils.APIError(
                    0,
                    "Se espera informacion de la cedula y/o pasaporte, pero ninguno de los dos esta",
                    StatusCodes.Status400BadRequest
                );
            }

            if (egresado.Cedula != null && egresado.Cedula.Length == 0 &&
            egresado.Pasaporte != null && egresado.Pasaporte.Length == 0)
            {
                throw Utils.APIError(
                    0,
                    "Se espera informacion de la cedula y/o pasaporte, pero ninguno de los dos esta",
                    StatusCodes.Status400BadRequest
                );
            }

            var rawEgresado = context
                .Egresados
                .Include(e => e.Participante)
                .ThenInclude(p => p.Direccion)
                .FirstOrDefault(e => e.EgresadoId == egresado.Id);

            if (rawEgresado == null)
            {
                return Results.Json(
                    data: "No existen registros para el egresado suministrado",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }

            var rawParticipante = rawEgresado.Participante;

            if (egresado.Cedula != null)
            {
                if (egresado.Cedula.Length > 0)
                {
                    ModificarDocumento(
                        context,
                        rawParticipante.ParticipanteId,
                        TipoIdentidad.CEDULA,
                        egresado.Cedula
                    );
                }
                else
                {
                    EliminarDocumento(
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
                    ModificarDocumento(
                        context,
                        rawParticipante.ParticipanteId,
                        TipoIdentidad.PASAPORTE,
                        egresado.Pasaporte
                    );
                }
                else
                {
                    EliminarDocumento(
                        context,
                        rawParticipante.ParticipanteId,
                        TipoIdentidad.PASAPORTE
                    );
                }
            }

            var rawMunicipio = ObtenerMunicipio(context, egresado.Provincia);

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

    [HttpGet("{valor}/{limit}/{offset}")]
    public IResult Busqueda(String valor, int limit, int offset)
    {

        PortalEgresadosContext? context = null;
        IDbContextTransaction? transaction = null;

        try
        {
            context = new PortalEgresadosContext();
            transaction = context.Database.BeginTransaction();

            var busqueda = context
                .Egresados
                .Where(b => EF.Functions.Like(b.PrimerNombre, $"%{valor}%") || EF.Functions.Like(b.PrimerApellido, $"%{valor}%"))
                .Include(e => e.Participante)
                .Include(e => e.NacionalidadNavigation)
                .OrderBy(b => b.PrimerNombre)
                .Skip(offset * limit)
                .Take(limit)
                .ToList();

            // Verificando si existe algun usuario con ese valor. Si no existe, la operacion debe ser abortada
            if (!busqueda.Any())
            {
                transaction.Rollback();

                return Results.Json(data: busqueda, statusCode: StatusCodes.Status400BadRequest);
            }

            var Egresado = new List<dynamic>();

            foreach (var item in busqueda)
            {

                var EgresadoDocumento = context
                    .Documentos
                    .Where(ed => ed.ParticipanteId == item.ParticipanteId)
                    .Include(ed => ed.TipoDocumento)
                    .ToList();

                var documento = new List<dynamic>();

                foreach (var documentos in EgresadoDocumento)
                {
                    dynamic d = new
                    {
                        documentoId = documentos.DocumentoId,
                        participanteId = documentos.ParticipanteId,
                        tipoDocId = documentos.TipoDocumentoId,
                        documentoNo = documentos.DocumentoNo,
                        tipoDoc = documentos.TipoDocumento.Nombre

                    };

                    documento.Add(d);

                }


                var EgresadoContacto = context
                    .Contactos
                    .Where(ed => ed.ParticipanteId == item.ParticipanteId)
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

                var EgresadoIdiomas = context
                    .EgresadoIdiomas
                    .Where(i => i.EgresadoId == item.EgresadoId)
                    .Include(i => i.Idioma)
                    .ToList();

                var Idiomas = new List<dynamic>();

                foreach (var idioma in EgresadoIdiomas)
                {
                    dynamic i = new
                    {
                        id = idioma.IdiomaId,
                        nombre = idioma.Idioma.Nombre,
                        egresadIdiomaId = idioma.EgresadoIdiomaId
                    };

                    Idiomas.Add(i);
                }

                var ExperienciaLaborales = context
                    .ExperienciaLaborals
                    .Where(e => e.EgresadoId == item.EgresadoId)
                    .ToList();

                var Experiencias = new List<dynamic>();

                foreach (var experienciaLaboral in ExperienciaLaborales)
                {
                    dynamic e = new
                    {
                        id = experienciaLaboral.ExperienciaLaboralId,
                        organizacion = experienciaLaboral.Organizacion,
                        posicion = experienciaLaboral.Posicion,
                        fechantrada = experienciaLaboral.FechaEntrada,
                        fechaSalida = experienciaLaboral.FechaSalida

                    };

                    Experiencias.Add(e);
                }

                var getEducaciones = context
                    .Educacions
                    .Where(e => e.EgresadoId == item.EgresadoId)
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

                var EgresadoHabilidades = context
                    .EgresadoHabilidads
                    .Where(eh => eh.EgresadoId == item.EgresadoId)
                    .Include(eh => eh.Habilidad)
                    .ToList();

                var Habilidades = new List<dynamic>();

                foreach (var habilidad in EgresadoHabilidades)
                {
                    dynamic h = new
                    {
                        id = habilidad.HabilidadId,
                        valor = habilidad.Habilidad.Nombre,
                        egresadoHabilidadId = habilidad.EgresadoHabilidadId
                    };

                    Habilidades.Add(h);
                }

                var EgresadoDestacado = context
                    .EgresadoDestacados
                    .Where(ed => ed.EgresadoId == item.EgresadoId)
                    .ToList();

                var destacado = new List<dynamic>();
                var egresadoDestacado = false;

                if (EgresadoDestacado.Any())
                {
                    foreach (var edestacado in EgresadoDestacado)
                    {
                        var egresadoHasta = edestacado.FechaHasta;

                        if (egresadoHasta > DateTime.Now)
                        {
                            egresadoDestacado = true;
                        }

                        dynamic ed = new
                        {
                            Observacion = edestacado.Observacion,
                            FechaDesde = edestacado.FechaDesde,
                            FechaHasta = edestacado.FechaHasta,
                            egresadoDestacado
                        };

                        destacado.Add(ed);

                    }
                }

                var ciudadDelEgresado = context
                    .Egresados
                    .Where(e => e.EgresadoId == item.EgresadoId)
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

                var EgresadoId = item.EgresadoId;
                var PrimerNombre = item.PrimerNombre;
                var SegundoNombre = item.SegundoNombre;
                var PrimerApellido = item.PrimerApellido;
                var SegundoApellido = item.SegundoApellido;
                var Genero = item.Genero;
                var FechaNac = item.FechaNac;
                var FotoPerfilUrl = item.Participante.FotoPerfilUrl;
                var about = item.Acerca;
                var activo = item.Estado;
                var Nacionalidad = item.NacionalidadNavigation.Nombre;


                dynamic Egresados = new
                {
                    EgresadoId = EgresadoId,
                    PrimerNombre = PrimerNombre,
                    SegundoNombre = SegundoNombre,
                    PrimerApellido = PrimerApellido,
                    SegundoApellido = SegundoApellido,
                    DocumentoEgresados = documento,
                    Genero = Genero,
                    FechaNac = FechaNac,
                    FotoPerfilUrl = FotoPerfilUrl,
                    Acerca = about,
                    Estado = activo,
                    Destacado = destacado,
                    Nacionalidad = Nacionalidad,
                    EgresadoIdiomas = Idiomas,
                    ExperienciaLaborals = Experiencias,
                    Educacions = Educaciones,
                    Contacto = contacto,
                    Habilidades = Habilidades,
                    Ciudad = ciudad
                };

                Egresado.Add(Egresados);
            }

            return Results.Json(
                data: Egresado,
                statusCode: StatusCodes.Status200OK
            );
        }

        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.ToString());

            return Results.Json(
                data: new ErrorResult(0, "Unexpected server error"),
                statusCode: StatusCodes.Status500InternalServerError
            );
        }


    }

    [HttpGet("{IdEgresado}")]
    public IResult BusquedaId(int IdEgresado)
    {

        PortalEgresadosContext? context = null;
        IDbContextTransaction? transaction = null;

        try
        {
            context = new PortalEgresadosContext();
            transaction = context.Database.BeginTransaction();

            var busqueda = context
                .Egresados
                .Where(b => b.EgresadoId == IdEgresado)
                .Include(e => e.Participante)
                .Include(e => e.NacionalidadNavigation)
                .OrderBy(b => b.PrimerNombre)
                .ToList();

            if (!busqueda.Any())
            {
                transaction.Rollback();

                return Results.Json(data: busqueda, statusCode: StatusCodes.Status400BadRequest);
            }

            var Egresado = new List<dynamic>();

            foreach (var item in busqueda)
            {

                var EgresadoDocumento = context
                    .Documentos
                    .Where(ed => ed.ParticipanteId == item.ParticipanteId)
                    .Include(ed => ed.TipoDocumento)
                    .ToList();

                var documento = new List<dynamic>();

                foreach (var documentos in EgresadoDocumento)
                {
                    dynamic d = new
                    {
                        documentoId = documentos.DocumentoId,
                        participanteId = documentos.ParticipanteId,
                        tipoDocId = documentos.TipoDocumentoId,
                        documentoNo = documentos.DocumentoNo,
                        tipoDoc = documentos.TipoDocumento.Nombre

                    };

                    documento.Add(d);

                }


                var EgresadoContacto = context
                    .Contactos
                    .Where(ed => ed.ParticipanteId == item.ParticipanteId)
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

                var EgresadoIdiomas = context
                    .EgresadoIdiomas
                    .Where(i => i.EgresadoId == item.EgresadoId)
                    .Include(i => i.Idioma)
                    .ToList();

                var Idiomas = new List<dynamic>();

                foreach (var idioma in EgresadoIdiomas)
                {
                    dynamic i = new
                    {
                        id = idioma.IdiomaId,
                        nombre = idioma.Idioma.Nombre,
                        egresadIdiomaId = idioma.EgresadoIdiomaId
                    };

                    Idiomas.Add(i);
                }

                var ExperienciaLaborales = context
                    .ExperienciaLaborals
                    .Where(e => e.EgresadoId == item.EgresadoId)
                    .ToList();

                var Experiencias = new List<dynamic>();

                foreach (var experienciaLaboral in ExperienciaLaborales)
                {
                    dynamic e = new
                    {
                        id = experienciaLaboral.ExperienciaLaboralId,
                        organizacion = experienciaLaboral.Organizacion,
                        posicion = experienciaLaboral.Posicion,
                        fechaEntrada = experienciaLaboral.FechaEntrada,
                        fechaSalida = experienciaLaboral.FechaSalida

                    };

                    Experiencias.Add(e);
                }

                var getEducaciones = context
                    .Educacions
                    .Where(e => e.EgresadoId == item.EgresadoId)
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

                var EgresadoHabilidades = context
                    .EgresadoHabilidads
                    .Where(eh => eh.EgresadoId == item.EgresadoId)
                    .Include(eh => eh.Habilidad)
                    .ToList();

                var Habilidades = new List<dynamic>();

                foreach (var habilidad in EgresadoHabilidades)
                {
                    dynamic h = new
                    {
                        id = habilidad.HabilidadId,
                        valor = habilidad.Habilidad.Nombre,
                        egresadoHabilidadId = habilidad.EgresadoHabilidadId
                    };

                    Habilidades.Add(h);
                }

                var EgresadoDestacado = context
                    .EgresadoDestacados
                    .Where(ed => ed.EgresadoId == item.EgresadoId)
                    .ToList();

                var destacado = new List<dynamic>();
                var egresadoDestacado = false;

                if (EgresadoDestacado.Any())
                {
                    foreach (var edestacado in EgresadoDestacado)
                    {
                        var egresadoHasta = edestacado.FechaHasta;

                        if (egresadoHasta > DateTime.Now)
                        {
                            egresadoDestacado = true;
                        }

                        dynamic ed = new
                        {
                            observacion = edestacado.Observacion,
                            FechaDesde = edestacado.FechaDesde,
                            FechaHasta = edestacado.FechaHasta,
                            egresadoDestacado
                        };

                        destacado.Add(ed);

                    }
                }

                var ciudadDelEgresado = context
                    .Egresados
                    .Where(e => e.EgresadoId == item.EgresadoId)
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

                var EgresadoId = item.EgresadoId;
                var PrimerNombre = item.PrimerNombre;
                var SegundoNombre = item.SegundoNombre;
                var PrimerApellido = item.PrimerApellido;
                var SegundoApellido = item.SegundoApellido;
                var Genero = item.Genero;
                var FechaNac = item.FechaNac;
                var FotoPerfilUrl = item.Participante.FotoPerfilUrl;
                var about = item.Acerca;
                var activo = item.Estado;
                var Nacionalidad = item.NacionalidadNavigation.Nombre;


                dynamic Egresados = new
                {
                    EgresadoId = EgresadoId,
                    PrimerNombre = PrimerNombre,
                    SegundoNombre = SegundoNombre,
                    PrimerApellido = PrimerApellido,
                    SegundoApellido = SegundoApellido,
                    DocumentoEgresados = documento,
                    Genero = Genero,
                    FechaNac = FechaNac,
                    FotoPerfilUrl = FotoPerfilUrl,
                    Acerca = about,
                    Estado = activo,
                    Destacado = destacado,
                    Nacionalidad = Nacionalidad,
                    EgresadoIdiomas = Idiomas,
                    ExperienciaLaborals = Experiencias,
                    Educacions = Educaciones,
                    Contacto = contacto,
                    Habilidades = Habilidades,
                    Ciudad = ciudad
                };

                Egresado.Add(Egresados);
            }

            return Results.Json(
                data: Egresado,
                statusCode: StatusCodes.Status200OK
            );
        }

        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.ToString());

            return Results.Json(
                data: new ErrorResult(0, "Unexpected server error"),
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }

}