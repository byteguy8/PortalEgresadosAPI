using System;
using System.Collections.Generic;

namespace GraduatesPortalAPI;

public partial class EgresadoIdioma
{
    public int EgresadoIdiomaId { get; set; }

    public int EgresadoId { get; set; }

    public int IdiomaId { get; set; }

    public bool? Mostrar { get; set; }

    public bool? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual Egresado Egresado { get; set; } = null!;

    public virtual Idioma Idioma { get; set; } = null!;
}
