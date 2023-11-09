using System;
using System.Collections.Generic;

namespace PortalEgresadosAPI;

public partial class VEgresado
{
    public int ParticipanteId { get; set; }

    public string TipoParticipante { get; set; } = null!;

    public int EgresadoId { get; set; }

    public string PrimerNombre { get; set; } = null!;

    public string? SegundoNombre { get; set; }

    public string PrimerApellido { get; set; } = null!;

    public string? SegundoApellido { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public string GenticilioNac { get; set; } = null!;

    public string Genero { get; set; } = null!;

    public DateTime FechaNac { get; set; }

    public int? Edad { get; set; }

    public string? Acerca { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string? FotoPerfilUrl { get; set; }

    public int DireccionId { get; set; }

    public string? DireccionPrincipal { get; set; }

    public string Localidad { get; set; } = null!;

    public string CodigoPostal { get; set; } = null!;

    public string Ciudad { get; set; } = null!;

    public string Pais { get; set; } = null!;
}
