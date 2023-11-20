using System;
using System.Collections.Generic;

namespace PortalEgresadosAPI;

public partial class VDireccion
{
    public int DireccionId { get; set; }

    public string? Direccion { get; set; }

    public string Localidad { get; set; } = null!;

    public string CodigoPostal { get; set; } = null!;

    public string Ciudad { get; set; } = null!;

    public string Pais { get; set; } = null!;
}
