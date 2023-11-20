using System;
using System.Collections.Generic;

namespace PortalEgresadosAPI.Database.Entities;

public partial class VPai
{
    public int Id { get; set; }

    public string Pais { get; set; } = null!;

    public string Iso { get; set; } = null!;
}
