using System;
using System.Collections.Generic;

namespace GraduatesPortalAPI;

public partial class TipoParticipante
{
    public int TipoParticipanteId { get; set; }

    public string Nombre { get; set; } = null!;

    public bool? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual ICollection<Participante> Participantes { get; set; } = new List<Participante>();
}
