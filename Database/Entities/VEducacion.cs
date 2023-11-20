using System;
using System.Collections.Generic;

namespace PortalEgresadosAPI;

public partial class VEducacion
{
    public int EgresadoId { get; set; }

    public int EducacionId { get; set; }

    public string Organizacion { get; set; } = null!;

    public DateTime FechaEntrada { get; set; }

    public DateTime FechaSalida { get; set; }

    public DateTime? FechaGraduacion { get; set; }

    public string NombreFormacion { get; set; } = null!;

    public string NivelAcademico { get; set; } = null!;
}
