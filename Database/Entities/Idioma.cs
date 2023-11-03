using System;
using System.Collections.Generic;

namespace GraduatesPortalAPI;

public partial class Idioma
{
    public int IdiomaId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Iso { get; set; } = null!;

    public bool? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual ICollection<EgresadoIdioma> EgresadoIdiomas { get; set; } = new List<EgresadoIdioma>();
}
