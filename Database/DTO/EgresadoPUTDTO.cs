public class EgresadoPUTDTO
{
    public int Id { get; set; }
    public required string PrimerApellido { get; set; }
    public string? SegundoApellido { get; set; }
    public required string PrimerNombre { get; set; }
    public string? SegundoNombre { get; set; }
    public required string Genero { get; set; }
    public required DateTime FechaNac { get; set; }
    public string? Cedula { get; set; }
    public string? Pasaporte { get; set; }
    public string? Acerca { get; set; }
    public required string Provincia { get; set; }
}