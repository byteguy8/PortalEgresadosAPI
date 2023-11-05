public class EgresadoPOSTDTO
{
    public required string Rol { get; set; }
    public required string TipoParticipante { get; set; }
    public required string PrimerApellido { get; set; }
    public string? SegundoApellido { get; set; }
    public required string PrimerNombre { get; set; }
    public string? SegundoNombre { get; set; }
    public required string Genero { get; set; }
    public required DateTime FechaNacimiento { get; set; }
    public string? Cedula { get; set; }
    public string? Pasaporte { get; set; }
    public required string Nacionalidad { get; set; }
    public required string Provincia { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string MatriculaEgresado { get; set; }
    public required string MatriculaGrado { get; set; }
}