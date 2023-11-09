using System;
using System.Collections.Generic;

namespace PortalEgresadosAPI;

public partial class VCiudad
{
    public int Id { get; set; }

    public string NombreCiudad { get; set; } = null!;

    public string Pais { get; set; } = null!;
}
