using System;
using System.Collections.Generic;

namespace GraduatesPortalAPI;

public partial class Educacion
{
    public int EducacionId { get; set; }

    public int EgresadoId { get; set; }

    public int FormacionId { get; set; }

    public string Organizacion { get; set; } = null!;

    public DateTime FechaEntrada { get; set; }

    public DateTime FechaSalida { get; set; }

    public DateTime? FechaGraduacion { get; set; }

    public bool? Mostrar { get; set; }

    public bool? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual Egresado Egresado { get; set; } = null!;

    public virtual Formacion Formacion { get; set; } = null!;
}
