using System;
using System.Collections.Generic;

namespace GraduatesPortalAPI;

public partial class Egresado
{
    public int EgresadoId { get; set; }

    public int ParticipanteId { get; set; }

    public int Nacionalidad { get; set; }

    public string PrimerNombre { get; set; } = null!;

    public string? SegundoNombre { get; set; }

    public string PrimerApellido { get; set; } = null!;

    public string? SegundoApellido { get; set; }

    public char Genero { get; set; }

    public DateTime FechaNac { get; set; }

    public int? Edad { get; set; }

    public string MatriculaGrado { get; set; } = null!;

    public string MatriculaEgresado { get; set; } = null!;

    public string? Acerca { get; set; }

    public bool? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual ICollection<Educacion> Educacions { get; set; } = new List<Educacion>();

    public virtual ICollection<EgresadoDestacado> EgresadoDestacados { get; set; } = new List<EgresadoDestacado>();

    public virtual ICollection<EgresadoHabilidad> EgresadoHabilidads { get; set; } = new List<EgresadoHabilidad>();

    public virtual ICollection<EgresadoIdioma> EgresadoIdiomas { get; set; } = new List<EgresadoIdioma>();

    public virtual ICollection<ExperienciaLaboral> ExperienciaLaborals { get; set; } = new List<ExperienciaLaboral>();

    public virtual Pais NacionalidadNavigation { get; set; } = null!;

    public virtual Participante Participante { get; set; } = null!;
}
