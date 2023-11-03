using System;
using System.Collections.Generic;

namespace GraduatesPortalAPI;

public partial class EgresadoHabilidad
{
    public int EgresadoHabilidadId { get; set; }

    public int EgresadoId { get; set; }

    public int HabilidadId { get; set; }

    public bool? Mostrar { get; set; }

    public bool? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual Egresado Egresado { get; set; } = null!;

    public virtual Habilidad Habilidad { get; set; } = null!;
}
