using System;
using System.Collections.Generic;

namespace PortalEgresadosAPI.Database.Entities;

public partial class VParticipanteDocumento
{
    public int ParticipanteId { get; set; }

    public string TipoDocumento { get; set; } = null!;

    public string DocumentoNo { get; set; } = null!;
}
