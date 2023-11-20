using System;
using System.Collections.Generic;

namespace PortalEgresadosAPI;

public partial class VParticipanteDocumento
{
    public int ParticipanteId { get; set; }

    public string TipoDocumento { get; set; } = null!;

    public string DocumentoNo { get; set; } = null!;
}
