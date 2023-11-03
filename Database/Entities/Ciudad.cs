using System;
using System.Collections.Generic;

namespace GraduatesPortalAPI;

public partial class Ciudad
{
    public int CiudadId { get; set; }

    public int PaisId { get; set; }

    public string Nombre { get; set; } = null!;

    public bool? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual ICollection<LocalidadPostal> LocalidadPostals { get; set; } = new List<LocalidadPostal>();

    public virtual Pais Pais { get; set; } = null!;
}
