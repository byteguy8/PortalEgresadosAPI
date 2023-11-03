using System;
using System.Collections.Generic;

namespace GraduatesPortalAPI;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public int RolId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Salt { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<LogUsuario> LogUsuarios { get; set; } = new List<LogUsuario>();

    public virtual Participante? Participante { get; set; }

    public virtual Rol Rol { get; set; } = null!;
}
