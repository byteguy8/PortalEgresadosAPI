using System;
using System.Collections.Generic;

namespace GraduatesPortalAPI;

public partial class Direccion
{
    public int DireccionId { get; set; }

    public int LocalidadPostalId { get; set; }

    public string DireccionPrincipal { get; set; } = null!;

    public bool? MostrarDireccionPrincipal { get; set; }

    public bool? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual LocalidadPostal LocalidadPostal { get; set; } = null!;

    public virtual ICollection<Participante> Participantes { get; set; } = new List<Participante>();
}
