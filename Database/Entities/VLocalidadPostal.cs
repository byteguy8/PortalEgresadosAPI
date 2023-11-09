using System;
using System.Collections.Generic;

namespace PortalEgresadosAPI;

public partial class VLocalidadPostal
{
    public int LocalidadPostalId { get; set; }

    public string Localidad { get; set; } = null!;

    public string CodigoPostal { get; set; } = null!;

    public string Ciudad { get; set; } = null!;

    public string Pais { get; set; } = null!;
}
