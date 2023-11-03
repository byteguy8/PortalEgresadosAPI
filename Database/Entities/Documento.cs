using System;
using System.Collections.Generic;

namespace GraduatesPortalAPI;

public partial class Documento
{
    public int DocumentoId { get; set; }

    public int TipoDocumentoId { get; set; }

    public int ParticipanteId { get; set; }

    public string DocumentoNo { get; set; } = null!;

    public bool? Mostrar { get; set; }

    public bool? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual Participante Participante { get; set; } = null!;

    public virtual TipoDocumento TipoDocumento { get; set; } = null!;
}
