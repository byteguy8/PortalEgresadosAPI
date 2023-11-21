using PortalEgresadosAPI;

public class ExperienciaLaboralPOSTDTO
{
    public int EgresadoId { get; set; }

    public string Organizacion { get; set; } = null!;

    public string Posicion { get; set; } = null!;

    public DateTime FechaEntrada { get; set; }

    public DateTime? FechaSalida { get; set; }

    public string? Acerca { get; set; }

    public bool? Mostrar { get; set; }

    public bool? Estado { get; set; }

    public ExperienciaLaboral Convert()
    {
        ExperienciaLaboral experienciaLaboral = new ExperienciaLaboral
        {
            EgresadoId = EgresadoId,
            Organizacion = Organizacion,
            Posicion = Posicion,
            FechaSalida = FechaSalida,
            Acerca = Acerca,
            FechaEntrada = FechaEntrada,
            Mostrar = Mostrar,
            Estado = Estado,
            FechaModificacion = DateTime.Now,
            FechaCreacion = DateTime.Now,
            ExperienciaLaboralId = 0
        };


        return experienciaLaboral;
    }
}