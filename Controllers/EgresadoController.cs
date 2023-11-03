using System.Security.Claims;
using GraduatesPortalAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UsersAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class EgresadoController : Controller
{
    [HttpGet("Busqueda")]
    public IResult Busqueda(String valor, int limit,int offset){

        PortalEgresadosContext? context = null;

        try
        {
            context = new PortalEgresadosContext();

            var busqueda = context
                .Egresados
                .Where(b => EF.Functions.Like(b.PrimerNombre, $"%{valor}%") || EF.Functions.Like (b.PrimerApellido, $"%{valor}%" ))
                .OrderBy(b => b.PrimerNombre)
                .Skip(offset * limit)
                .Take(limit)
                .ToList();

            var Egresado = new List<Egresado>();

            if (Egresado.Any()){

                
                return null;
            }

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

            return Results.Json(
                data: new ErrorResult(0, "Unexpected server error"),
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }

    [HttpGet("BusquedaId")]
    public IResult BusquedaId(int IdEgresado){

        PortalEgresadosContext? context = null;

        try
        {
            context = new PortalEgresadosContext();

            var busqueda = context
                .Egresados
                .Where(b => b.EgresadoId == IdEgresado)
                .OrderBy(b => b.PrimerNombre)
                .ToList();

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
    public IResult PostEgresadoIdioma(int IdEgresado){

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

}

