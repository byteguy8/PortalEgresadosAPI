using System;
using System.Collections.Generic;

namespace GraduatesPortalAPI;

public partial class DocumentoEgresado
{
    public int DocumentoEgresadoId { get; set; }

    public int TipoDocumentoId { get; set; }

    public int EgresadoId { get; set; }

    public string DocumentoNo { get; set; } = null!;

    public bool? Mostrar { get; set; }

    public bool? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual Egresado Egresado { get; set; } = null!;

    public virtual TipoDocumento TipoDocumento { get; set; } = null!;
}
