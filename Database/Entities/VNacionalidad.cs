using System;
using System.Collections.Generic;

namespace PortalEgresadosAPI.Database.Entities;

public partial class VNacionalidad
{
    public int Id { get; set; }

    public string Nacionalidad { get; set; } = null!;

    public string Iso { get; set; } = null!;
}
