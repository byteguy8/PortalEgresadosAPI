using System;
using System.Collections.Generic;

namespace GraduatesPortalAPI;

public partial class Formacion
{
    public int FormacionId { get; set; }

    public int NivelId { get; set; }

    public string Nombre { get; set; } = null!;

    public bool? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual ICollection<Educacion> Educacions { get; set; } = new List<Educacion>();

    public virtual Nivel Nivel { get; set; } = null!;
}
