using System;
using System.Collections.Generic;

namespace GraduatesPortalAPI;

public partial class Participante
{
    public int ParticipanteId { get; set; }

    public int TipoParticipanteId { get; set; }

    public int UsuarioId { get; set; }

    public int DireccionId { get; set; }

    public bool? EsEgresado { get; set; }

    public string? FotoPerfilUrl { get; set; }

    public bool? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual ICollection<Contacto> Contactos { get; set; } = new List<Contacto>();

    public virtual Direccion Direccion { get; set; } = null!;

    public virtual ICollection<Documento> Documentos { get; set; } = new List<Documento>();

    public virtual Egresado? Egresado { get; set; }

    public virtual TipoParticipante TipoParticipante { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
