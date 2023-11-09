using System;
using System.Collections.Generic;

namespace PortalEgresadosAPI;

public partial class VNacionalidad
{
    public int Id { get; set; }

    public string Nacionalidad { get; set; } = null!;

    public string Iso { get; set; } = null!;
}
