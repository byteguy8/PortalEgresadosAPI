using System;
using System.Collections.Generic;

namespace PortalEgresadosAPI;

public partial class VContacto
{
    public int ParticipanteId { get; set; }

    public int ContactoId { get; set; }

    public string Contacto { get; set; } = null!;

    public string TipoContacto { get; set; } = null!;
}
