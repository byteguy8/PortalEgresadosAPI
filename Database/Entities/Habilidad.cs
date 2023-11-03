using System;
using System.Collections.Generic;

namespace GraduatesPortalAPI;

public partial class Habilidad
{
    public int HabilidadId { get; set; }

    public string Nombre { get; set; } = null!;

    public bool? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual ICollection<EgresadoHabilidad> EgresadoHabilidads { get; set; } = new List<EgresadoHabilidad>();
}
