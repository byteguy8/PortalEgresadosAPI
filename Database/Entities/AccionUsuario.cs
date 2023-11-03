using System;
using System.Collections.Generic;

namespace GraduatesPortalAPI;

public partial class AccionUsuario
{
    public int AccionUsuarioId { get; set; }

    public string Nombre { get; set; } = null!;

    public bool? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual ICollection<LogUsuario> LogUsuarios { get; set; } = new List<LogUsuario>();
}
