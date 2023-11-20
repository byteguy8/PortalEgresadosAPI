using System;
using System.Collections.Generic;

namespace PortalEgresadosAPI;

public partial class VEducacionConDetalle
{
    public int EducacionId { get; set; }

    public int EgresadoId { get; set; }

    public int IdInEducacion { get; set; }

    public string Organizacion { get; set; } = null!;

    public DateTime FechaEntrada { get; set; }

    public DateTime FechaSalida { get; set; }

    public DateTime? FechaGraduacion { get; set; }

    public bool? EducacionActiva { get; set; }

    public bool? EstadoEducacion { get; set; }

    public int IdInFormacion { get; set; }

    public string NombreFormacion { get; set; } = null!;

    public bool? EstadoFormacion { get; set; }

    public int NivelId { get; set; }

    public string NombreNivel { get; set; } = null!;

    public int? PrioridadNivel { get; set; }

    public bool? EstadoNivel { get; set; }
}
