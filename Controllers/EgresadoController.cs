using GraduatesPortalAPI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace UsersAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class EgresadoController : Controller
{
    [HttpGet("Busqueda")]
    public IResult Busqueda(String valor, int limit,int offset){

        PortalEgresadosContext? context = null;
        IDbContextTransaction? transaction = null;

        try
        {
            context = new PortalEgresadosContext();
            transaction = context.Database.BeginTransaction();
            String empty = "";

            var busqueda = context
                .Egresados
                .Where(b => EF.Functions.Like(b.PrimerNombre, $"%{valor}%") || EF.Functions.Like (b.PrimerApellido, $"%{valor}%"))
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

                var error = new ErrorMsg(0, $"No existe el Egresado con Nombre '{valor}'");

                return Results.Json(data: error, statusCode: StatusCodes.Status400BadRequest);
            }

            var Egresado = new List<dynamic>();

            foreach (var item in busqueda){

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
                        tipodocId = documentos.TipoDocumentoId,
                        documentoNo = documentos.DocumentoNo,
                        tipodoc = documentos.TipoDocumento.Nombre

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
                        tipocontactoId = contactos.TipoContactoId,
                        nombre = contactos.Nombre,
                        tipocontacto = contactos.TipoContacto.Nombre

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
                        fechaentrada = experienciaLaboral.FechaEntrada,
                        fechasalida = experienciaLaboral.FechaSalida

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

                if (ciudadDelEgresado != null){

                     ciudad = ciudadDelEgresado.Nombre;

                }else{

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

    [HttpGet("BusquedaId")]
    public IResult BusquedaId(int IdEgresado){

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

                var error = new ErrorMsg(0, $"No existe el Egresado con el ID '{IdEgresado}'");

                return Results.Json(data: error, statusCode: StatusCodes.Status400BadRequest);
            }

            var Egresado = new List<dynamic>();

            foreach (var item in busqueda){

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
                        tipodocId = documentos.TipoDocumentoId,
                        documentoNo = documentos.DocumentoNo,
                        tipodoc = documentos.TipoDocumento.Nombre

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
                        tipocontactoId = contactos.TipoContactoId,
                        nombre = contactos.Nombre,
                        tipocontacto = contactos.TipoContacto.Nombre

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
                        fechaentrada = experienciaLaboral.FechaEntrada,
                        fechasalida = experienciaLaboral.FechaSalida

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

                if (ciudadDelEgresado != null){

                     ciudad = ciudadDelEgresado.Nombre;

                }else{

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

    [HttpGet("EgresadoIdioma")]
    public IResult EgresadoIdioma(int IdEgresado){

        PortalEgresadosContext? context = null;

        try
        {
            context = new PortalEgresadosContext();

            var EgresadoIdiomas = context
                    .EgresadoIdiomas
                    .Where(i => i.EgresadoId == IdEgresado)
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

            return Results.Json(
                data: Idiomas,
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

    [HttpPost("EgresadoIdioma")]
    public IResult PostEgresadoIdioma(EgresadoIdiomaPOSTDTO egresadoIdioma){

        PortalEgresadosContext? context = null;
        IDbContextTransaction? transaction = null;

        try
        {

            context = new PortalEgresadosContext();
            transaction = context.Database.BeginTransaction();

            var AgregaIdioma = new EgresadoIdioma{

             EgresadoId = egresadoIdioma.EgresadoId,
             IdiomaId = egresadoIdioma.IdiomaId

           };

        var resultAddIdioma = context
            .EgresadoIdiomas
            .Add(AgregaIdioma);

        if (resultAddIdioma == null)
        {
            transaction.Rollback();

            var error = new ErrorMsg(0, $"Error inesperado");

            return Results.Json(data: error, statusCode: StatusCodes.Status500InternalServerError);
        }

        context.SaveChanges();

        transaction.Commit();

        return Results.Ok(resultAddIdioma.Entity.EgresadoIdiomaId);

    }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.ToString());

            transaction?.Rollback();

            var error = new ErrorMsg(0, "Error no esperado");

            return Results.Json(
                data: error,
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
        finally{

            transaction?.Dispose();
            context?.Dispose();
        }
    }

    [HttpDelete("EgresadoIdioma")]
    public IResult DeleteEgresadoIdioma(EgresadoIdiomaDeleteDTO egresadoIdioma){

        PortalEgresadosContext? context = null;
        IDbContextTransaction? transaction = null;

        try
        {

            context = new PortalEgresadosContext();
            transaction = context.Database.BeginTransaction();

            var EliminaIdioma = new EgresadoIdioma{

                EgresadoIdiomaId = egresadoIdioma.EgresadoIdiomaId

           };

        var resultEliminaIdioma = context
            .EgresadoIdiomas
            .Remove(EliminaIdioma);

        if (resultEliminaIdioma == null)
        {
            transaction.Rollback();

            var error = new ErrorMsg(0, $"Error inesperado");

            return Results.Json(data: error, statusCode: StatusCodes.Status500InternalServerError);
        }

        context.SaveChanges();

        transaction.Commit();

        return Results.Ok();

    }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.ToString());

            transaction?.Rollback();

            var error = new ErrorMsg(0, "Error no esperado");

            return Results.Json(
                data: error,
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
        finally{

            transaction?.Dispose();
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

            // Verificando si existe la nacionalidad. Si no existe, la operacion debe ser abortada
            var nacionalidad = context
                .Pais
                .FirstOrDefault(n => n.Nombre == egresado.Nacionalidad);

            if (nacionalidad == null)
            {
                transaction.Rollback();

                var error = new ErrorMsg(0, $"No existe la nacionalidad con el nombre '{egresado.Nacionalidad}'");

                return Results.Json(data: error, statusCode: StatusCodes.Status400BadRequest);
            }

            // Verificando si existe la provincia. Si no existe, la operacion debe ser abortada
            var provincia = context
                .Ciudad
                .FirstOrDefault(c => c.Nombre == egresado.Provincia.ToUpper());

            if (provincia == null)
            {
                transaction.Rollback();

                var error = new ErrorMsg(0, $"No existe la provincia con el nombre '{egresado.Provincia}'");

                return Results.Json(data: error, statusCode: StatusCodes.Status400BadRequest);
            }

            // Verificando si existe el rol. Si no existe, la operacion debe ser abortada
            var rol = context
                .Rols
                .FirstOrDefault(r => r.Nombre == egresado.Rol.ToUpper());

            if (rol == null)
            {
                transaction.Rollback();

                var error = new ErrorMsg(0, $"No existe el rol '{egresado.Rol}'");

                return Results.Json(data: error, statusCode: StatusCodes.Status400BadRequest);
            }


            // Verificando si existe el tipo de participante. Si no existe, la operacion debe ser abortada
            var tipoParticipante = context
                .TipoParticipantes
                .FirstOrDefault(t => t.Nombre == egresado.TipoParticipante);

            if (tipoParticipante == null)
            {
                transaction.Rollback();

                var error = new ErrorMsg(0, $"No existe el tipo de participante '{egresado.TipoParticipante}'");

                return Results.Json(data: error, statusCode: StatusCodes.Status400BadRequest);
            }

            // Verificando si ya existe un usuario con el mismo nombre. En dicho caso, la operacion debe ser abortada
            var existeUsuario = context
                .Usuarios
                .Where(u => EF.Functions.Like(u.UserName, $"%{egresado.UserName}%"))
                .ToList()
                .Count >= 1;

            if (existeUsuario)
            {
                transaction.Rollback();

                var error = new ErrorMsg(0, $"Ya existe un usuario con el nombre '{egresado.UserName}'");

                return Results.Json(data: error, statusCode: StatusCodes.Status400BadRequest);
            }

            // Creando el usuario
            var usuario = new Usuario
            {
                RolId = rol.RolId,
                UserName = egresado.UserName,
                Password = egresado.Password
            };

            var resultUsuario = context
                .Usuarios
                .Add(usuario);

            if (resultUsuario == null)
            {
                transaction.Rollback();

                var error = new ErrorMsg(0, $"Error inesperado");

                return Results.Json(data: error, statusCode: StatusCodes.Status500InternalServerError);
            }

            context.SaveChanges();

            usuario.UsuarioId = resultUsuario.Entity.UsuarioId;

            // Creando la direccion
            var municipio = context
                .LocalidadPostal
                .FirstOrDefault(m => m.CiudadId == provincia.CiudadId);


            if (municipio == null)
            {
                transaction.Rollback();

                var error = new ErrorMsg(0, $"No se pudo obtener informacion del municipio");

                return Results.Json(data: error, statusCode: StatusCodes.Status500InternalServerError);
            }

            var direccion = new Direccion
            {
                LocalidadPostalId = municipio.LocalidadPostalId
            };

            var resultDireccion = context
                .Direccions
                .Add(direccion);

            context.SaveChanges();

            if (resultDireccion == null)
            {
                transaction.Rollback();

                var error = new ErrorMsg(0, $"Error inesperado");

                return Results.Json(data: error, statusCode: StatusCodes.Status500InternalServerError);
            }

            direccion.DireccionId = resultDireccion.Entity.DireccionId;

            // Creando participante
            var participante = new Participante
            {
                TipoParticipanteId = tipoParticipante.TipoParticipanteId,
                UsuarioId = usuario.UsuarioId,
                EsEgresado = false,
                DireccionId = direccion.DireccionId
            };

            var resultParticipante = context
                .Participantes
                .Add(participante);

            if (resultParticipante == null)
            {
                transaction.Rollback();

                var error = new ErrorMsg(0, $"Error inesperado");

                return Results.Json(data: error, statusCode: StatusCodes.Status500InternalServerError);
            }

            context.SaveChanges();

            participante.ParticipanteId = resultParticipante.Entity.ParticipanteId;

            // Creando egresado
            var nuevoEgresado = new Egresado
            {
                ParticipanteId = participante.ParticipanteId,
                Nacionalidad = nacionalidad.PaisId,
                PrimerNombre = egresado.PrimerNombre,
                SegundoNombre = egresado.SegundoNombre,
                PrimerApellido = egresado.PrimerApellido,
                SegundoApellido = egresado.SegundoApellido,
                Genero = egresado.Genero,
                FechaNac = egresado.FechaNacimiento,
                MatriculaEgresado = egresado.MatriculaEgresado,
                MatriculaGrado = egresado.MatriculaGrado
            };

            var resultEgresado = context
                .Egresados
                .Add(nuevoEgresado);

            if (resultEgresado == null)
            {
                transaction.Rollback();

                var error = new ErrorMsg(0, $"Error inesperado");

                return Results.Json(data: error, statusCode: StatusCodes.Status500InternalServerError);
            }

            context.SaveChanges();

            transaction.Commit();

            return Results.Ok(resultEgresado.Entity.EgresadoId);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.ToString());

            transaction?.Rollback();

            var error = new ErrorMsg(
                0,
                "Error no esperado"
            );

            return Results.Json(
                data: error,
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
        finally
        {
            transaction?.Dispose();
            context?.Dispose();
        }
    }

}

