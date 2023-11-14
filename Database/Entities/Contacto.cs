using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PortalEgresadosAPI;

public partial class Contacto
{
    public int ContactoId { get; set; }

    public int ParticipanteId { get; set; }

    public int TipoContactoId { get; set; }

    [JsonIgnore] public string Nombre { get; set; } = null!;

    public bool? Mostrar { get; set; }

    public bool? Estado { get; set; }

    [JsonIgnore] public DateTime? FechaCreacion { get; set; }

    [JsonIgnore] public DateTime? FechaModificacion { get; set; }

    [JsonIgnore] public virtual Participante Participante { get; set; } = null!;

    [JsonIgnore] public virtual TipoContacto TipoContacto { get; set; } = null!;
}
