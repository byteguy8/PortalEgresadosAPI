using System;
using System.Collections.Generic;

namespace PortalEgresadosAPI.Database.Entities;

public partial class VCiudad
{
    public int Id { get; set; }

    public string NombreCiudad { get; set; } = null!;

    public string Pais { get; set; } = null!;
}
