using System;
using System.Collections.Generic;

namespace GraduatesPortalAPI;

public partial class Pais
{
    public int PaisId { get; set; }

    public string Nombre { get; set; } = null!;

    public string GenticilioNac { get; set; } = null!;

    public string Iso { get; set; } = null!;

    public bool? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual ICollection<Ciudad> Ciudads { get; set; } = new List<Ciudad>();

    public virtual ICollection<Egresado> Egresados { get; set; } = new List<Egresado>();
}
