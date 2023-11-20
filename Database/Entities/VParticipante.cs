using System;
using System.Collections.Generic;

namespace PortalEgresadosAPI;

public partial class VParticipante
{
    public int ParticipanteId { get; set; }

    public string TipoParticipante { get; set; } = null!;

    public string NombreUsuario { get; set; } = null!;

    public string? FotoPerfilUrl { get; set; }

    public string EsEgresado { get; set; } = null!;

    public int DireccionId { get; set; }

    public string? DireccionPrincipal { get; set; }

    public string Localidad { get; set; } = null!;

    public string CodigoPostal { get; set; } = null!;

    public string Ciudad { get; set; } = null!;

    public string Pais { get; set; } = null!;
}
