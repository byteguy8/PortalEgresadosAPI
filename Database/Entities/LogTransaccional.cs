using System;
using System.Collections.Generic;

namespace PortalEgresadosAPI.Database.Entities;

public partial class LogTransaccional
{
    public int LogTransaccionalId { get; set; }

    public string? UsuarioDb { get; set; }

    public string Accion { get; set; } = null!;

    public DateTime? FechaHora { get; set; }

    public string TablaAfectada { get; set; } = null!;

    public string? ValorAnterior { get; set; }

    public string? ValorNuevo { get; set; }
}
