using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PortalEgresadosAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CandidatoController : ControllerBase
{
    [HttpGet("{egresadoId}")]
    public IResult EsCandidato(int egresadoId)
    {
        PortalEgresadosContext? context = null;

        try
        {
            context = new PortalEgresadosContext();

            var esCandidatoDoctorado = context
                .Egresados
                .Include(e => e.Participante)
                .Include(e => e.NacionalidadNavigation)
                .Where(e =>
                    e.EgresadoId == egresadoId &&
                    e.Educacions.Where(ed => ed.Formacion.Nivel.Nombre == "Doctorado").Any() &&
                    e.EgresadoHabilidads.Count() >= 3 &&
                    e.EgresadoIdiomas.Count() >= 1
                )
                .Any();

            var esCandidatoMaestria = context
                .Egresados
                .Include(e => e.Participante)
                .Include(e => e.NacionalidadNavigation)
                .Where(e =>
                    e.EgresadoId == egresadoId &&
                    e.Educacions.Where(ed => ed.Formacion.Nivel.Nombre == "Maestria").Any() &&
                    e.EgresadoHabilidads.Count() >= 6 &&
                    e.EgresadoIdiomas.Count() >= 2 &&
                    e.ExperienciaLaborals.Count() >= 3
                )
                .Any();

            var esCandidatoGrado = context
                .Egresados
                .Include(e => e.Participante)
                .Include(e => e.NacionalidadNavigation)
                .Where(e =>
                    e.EgresadoId == egresadoId &&
                    e.Educacions.Where(ed => ed.Formacion.Nivel.Nombre == "Grado").Any() &&
                    e.Educacions.Where(ed => ed.Formacion.Nivel.Nombre == "Tecnico Profesional").Any() &&
                    e.EgresadoHabilidads.Count() >= 10 &&
                    e.EgresadoIdiomas.Count() >= 2 &&
                    e.ExperienciaLaborals.Count() >= 5
                )
                .Any();

            return Results.Ok(
                esCandidatoDoctorado ||
                esCandidatoMaestria ||
                esCandidatoGrado
            );
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

    [HttpGet]
    public IResult ObtenerCandidatos()
    {
        PortalEgresadosContext? context = null;

        try
        {
            context = new PortalEgresadosContext();

            var rawCandidadosDoctorado = context
                .Egresados
                .Include(e => e.Participante)
                .Include(e => e.NacionalidadNavigation)
                .Where(e =>
                    e.Educacions.Where(ed => ed.Formacion.Nivel.Nombre == "Doctorado").Any() &&
                    e.EgresadoHabilidads.Count() >= 3 &&
                    e.EgresadoIdiomas.Count() >= 1
                )
                .ToList();

            var rawCandidatosMaestria = context
                .Egresados
                .Include(e => e.Participante)
                .Include(e => e.NacionalidadNavigation)
                .Where(e =>
                    e.Educacions.Where(ed => ed.Formacion.Nivel.Nombre == "Maestria").Any() &&
                    e.EgresadoHabilidads.Count() >= 6 &&
                    e.EgresadoIdiomas.Count() >= 2 &&
                    e.ExperienciaLaborals.Count() >= 3
                )
                .ToList();

            var rawCandidatosGrado = context
                .Egresados
                .Include(e => e.Participante)
                .Include(e => e.NacionalidadNavigation)
                .Where(e =>
                    e.Educacions.Where(ed => ed.Formacion.Nivel.Nombre == "Grado").Any() &&
                    e.Educacions.Where(ed => ed.Formacion.Nivel.Nombre == "Tecnico Profesional").Any() &&
                    e.EgresadoHabilidads.Count() >= 10 &&
                    e.EgresadoIdiomas.Count() >= 2 &&
                    e.ExperienciaLaborals.Count() >= 5
                )
                .ToList();

            var egresados = new List<dynamic>();

            foreach (var rawEgresado in rawCandidadosDoctorado)
            {
                egresados.Add(Utils.ObtenerInfEgresado(context, rawEgresado));
            }

            foreach (var rawEgresado in rawCandidatosMaestria)
            {
                egresados.Add(Utils.ObtenerInfEgresado(context, rawEgresado));
            }

            foreach (var rawEgresado in rawCandidatosGrado)
            {
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