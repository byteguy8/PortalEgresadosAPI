using System;
using System.Collections.Generic;

namespace GraduatesPortalAPI;

public partial class TipoContacto
{
    public int TipoContactoId { get; set; }

    public string Nombre { get; set; } = null!;

    public bool? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual ICollection<Contacto> Contactos { get; set; } = new List<Contacto>();
}
