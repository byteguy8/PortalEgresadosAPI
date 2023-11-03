using System;
using System.Collections.Generic;

namespace GraduatesPortalAPI;

public partial class Contacto
{
    public int ContactoId { get; set; }

    public int ParticipanteId { get; set; }

    public int TipoContactoId { get; set; }

    public string Nombre { get; set; } = null!;

    public bool? Mostrar { get; set; }

    public bool? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual Participante Participante { get; set; } = null!;

    public virtual TipoContacto TipoContacto { get; set; } = null!;
}
