using System;
using System.Collections.Generic;

namespace GraduatesPortalAPI;

public partial class Nivel
{
    public int NivelId { get; set; }

    public string Nombre { get; set; } = null!;

    public int? Prioridad { get; set; }

    public bool? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual ICollection<Formacion> Formacions { get; set; } = new List<Formacion>();
}
