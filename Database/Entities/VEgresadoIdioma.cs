using System;
using System.Collections.Generic;

namespace PortalEgresadosAPI.Database.Entities;

public partial class VEgresadoIdioma
{
    public int EgresadoId { get; set; }

    public string Idioma { get; set; } = null!;

    public string Iso { get; set; } = null!;
}
