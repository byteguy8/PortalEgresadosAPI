using System;
using System.Collections.Generic;

namespace PortalEgresadosAPI.Database.Entities;

public partial class VExperienciaLaboral
{
    public int EgresadoId { get; set; }

    public string Organizacion { get; set; } = null!;

    public string Posicion { get; set; } = null!;

    public DateTime FechaEntrada { get; set; }

    public DateTime? FechaSalida { get; set; }

    public string? Acerca { get; set; }
}
