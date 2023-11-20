using System;
using System.Collections.Generic;

namespace PortalEgresadosAPI;

public partial class VContactoConTipo
{
    public int ContactoId { get; set; }

    public int ParticipanteId { get; set; }

    public int IdInContacto { get; set; }

    public string NombreContacto { get; set; } = null!;

    public bool? ContactoActivo { get; set; }

    public bool? EstadoContacto { get; set; }

    public int TipoContactoId { get; set; }

    public string NombreTipoContacto { get; set; } = null!;

    public bool? EstadoTipoContacto { get; set; }

    public string NombreCompleto { get; set; } = null!;
}
