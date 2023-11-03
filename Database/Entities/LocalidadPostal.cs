using System;
using System.Collections.Generic;

namespace GraduatesPortalAPI;

public partial class LocalidadPostal
{
    public int LocalidadPostalId { get; set; }

    public int CiudadId { get; set; }

    public string Nombre { get; set; } = null!;

    public string CodigoPostal { get; set; } = null!;

    public bool? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual Ciudad Ciudad { get; set; } = null!;

    public virtual ICollection<Direccion> Direccions { get; set; } = new List<Direccion>();
}
