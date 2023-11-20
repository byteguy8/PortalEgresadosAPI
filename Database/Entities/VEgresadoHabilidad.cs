using System;
using System.Collections.Generic;

namespace PortalEgresadosAPI;

public partial class VEgresadoHabilidad
{
    public int EgresadoId { get; set; }

    public int EgresadoHabilidadId { get; set; }

    public string Habilidad { get; set; } = null!;
}
