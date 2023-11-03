using System;
using System.Collections.Generic;

namespace GraduatesPortalAPI;

public partial class Rol
{
    public int RolId { get; set; }

    public string Nombre { get; set; } = null!;

    public bool? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
