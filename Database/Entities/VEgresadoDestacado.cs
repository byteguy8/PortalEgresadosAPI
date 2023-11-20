using System;
using System.Collections.Generic;

namespace PortalEgresadosAPI.Database.Entities;

public partial class VEgresadoDestacado
{
    public int EgresadoId { get; set; }

    public int EgresadoDestacadoId { get; set; }

    public int? Orden { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public DateTime FechaDesde { get; set; }

    public DateTime FechaHasta { get; set; }

    public string? Observacion { get; set; }
}
