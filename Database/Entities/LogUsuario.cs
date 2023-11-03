using System;
using System.Collections.Generic;

namespace GraduatesPortalAPI;

public partial class LogUsuario
{
    public int LogUsuarioId { get; set; }

    public int UsuarioId { get; set; }

    public int AccionUsuarioId { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public bool? Estado { get; set; }

    public virtual AccionUsuario AccionUsuario { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
