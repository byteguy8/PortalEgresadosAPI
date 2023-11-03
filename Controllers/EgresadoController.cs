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

            var busqueda = context
                .Egresados
                .Where(b => EF.Functions.Like(b.PrimerNombre, $"%{valor}%") || EF.Functions.Like (b.PrimerApellido, $"%{valor}%" ))
                .OrderBy(b => b.PrimerNombre)
                .Skip(offset * limit)
                .Take(limit)
                .ToList();

            // Verificando si existe algun usuario con ese valor. Si no existe, la operacion debe ser abortada
            if (busqueda.Any())
            {
                transaction.Rollback();

                var error = new ErrorMsg(0, $"No existe el Egresado con el nombre '{valor}'");

                return Results.Json(data: error, statusCode: StatusCodes.Status400BadRequest);
            }

            var Egresado = new List<Egresado>();

            foreach (var item in busqueda){

                var EgresadoId = item.EgresadoId;
                var PrimerNombre = item.PrimerNombre;
                var SegundoNombre = item.SegundoNombre;
                var PrimerApellido = item.PrimerApellido;
                var SegundoApellido = item.SegundoApellido;
                var DocumentoEgresados = item.Participante.Documentos;
                var Genero = item.Genero;
                var FechaNac = item.FechaNac;
                var FotoPerfilUrl = item.Participante.FotoPerfilUrl;
                var Nacionalidad = item.NacionalidadNavigation.Nombre;
                var EgresadoIdioma = item.EgresadoIdiomas;
                var ExperienciaLaboral = item.ExperienciaLaborals;
                var Educacion = item.Educacions;
                var Contacto = item.Participante.Contactos;
                var EgresadoHabilidad = item.EgresadoHabilidads;


                dynamic Egresados = new
                {
                    EgresadoId = EgresadoId,
                    PrimerNombre = PrimerNombre,
                    SegundoNombre = SegundoNombre,
                    PrimerApellido = PrimerApellido,
                    SegundoApellido = SegundoApellido,
                    DocumentoEgresados = DocumentoEgresados,
                    Genero = Genero,
                    FechaNac = FechaNac,
                    FotoPerfilUrl = FotoPerfilUrl,
                    Nacionalidad = Nacionalidad,
                    EgresadoIdiomas = EgresadoIdioma,
                    ExperienciaLaborals = ExperienciaLaboral,
                    Educacions = Educacion,
                    Contacto = Contacto,
                    EgresadoHabilidads = EgresadoHabilidad
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
            Console.Error.WriteLine(ex.Message);

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

                var EgresadoId = item.EgresadoId;
                var PrimerNombre = item.PrimerNombre;
                var SegundoNombre = item.SegundoNombre;
                var PrimerApellido = item.PrimerApellido;
                var SegundoApellido = item.SegundoApellido;
                var DocumentoEgresados = item.Participante.Documentos;
                var Genero = item.Genero;
                var FechaNac = item.FechaNac;
                var FotoPerfilUrl = item.Participante.FotoPerfilUrl;
                var Nacionalidad = item.NacionalidadNavigation.Nombre;
                var EgresadoIdioma = context.EgresadoIdiomas.Where(i => i.EgresadoId == item.EgresadoId).Include(i => i.Idioma).ToList();
                var ExperienciaLaboral = context.ExperienciaLaborals.Where(e => e.EgresadoId == item.EgresadoId).ToList();
                var Educacion = context.Educacions.Where(e => e.EgresadoId == item.EgresadoId).Include(e => e.Formacion).ToList();
                var EgresadoHabilidad = context.EgresadoHabilidads.Where(eh => eh.EgresadoId == item.EgresadoId).Include(eh => eh.Habilidad).ToList();
                var Contacto = item.Participante.Contactos;


                dynamic Egresados = new
                {
                    EgresadoId = EgresadoId,
                    PrimerNombre = PrimerNombre,
                    SegundoNombre = SegundoNombre,
                    PrimerApellido = PrimerApellido,
                    SegundoApellido = SegundoApellido,
                    DocumentoEgresados = DocumentoEgresados,
                    Genero = Genero,
                    FechaNac = FechaNac,
                    FotoPerfilUrl = FotoPerfilUrl,
                    Nacionalidad = Nacionalidad,
                    EgresadoIdiomas = EgresadoIdioma,
                    ExperienciaLaborals = ExperienciaLaboral,
                    Educacions = Educacion,
                    Contacto = Contacto,
                    EgresadoHabilidads = EgresadoHabilidad
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

            var getidiomas = context
            .Egresados
            .Where(e => e.EgresadoId == IdEgresado)
            .SelectMany(e => e.EgresadoIdiomas)
            .Select(ei => ei.Idioma)
            .ToList();

            var idiomas = new List<Idioma>();

            foreach (var item in getidiomas){

                var idiomaId = item.IdiomaId;
                var Egreidioma = item.Nombre;

                var idioma = new Idioma
                {
                    IdiomaId = idiomaId,
                    Nombre = Egreidioma

                };

                idiomas.Add(idioma);
        
            }

            return Results.Json(
                data: idiomas,
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
    public IResult PostEgresadoIdioma(int EgresadoId, int IdiomaId){

        return null;


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

