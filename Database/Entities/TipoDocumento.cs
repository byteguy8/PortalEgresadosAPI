using System;
using System.Collections.Generic;

namespace PortalEgresadosAPI.Database.Entities;

public partial class TipoDocumento
{
    public int TipoDocumentoId { get; set; }

    public string Nombre { get; set; } = null!;

    public bool? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual ICollection<Documento> Documentos { get; set; } = new List<Documento>();
}
