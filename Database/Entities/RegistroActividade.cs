using System;
using System.Collections.Generic;

namespace PortalEgresadosAPI;

public partial class RegistroActividade
{
    public int Id { get; set; }

    public string? Usuario { get; set; }

    public string? Accion { get; set; }

    public DateTime? FechaHora { get; set; }

    public string? TablaAfectada { get; set; }

    public string? ValoresAnteriores { get; set; }

    public string? ValoresNuevos { get; set; }
}
