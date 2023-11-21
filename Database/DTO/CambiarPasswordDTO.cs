public class CambiarPasswordDTO
{
    public required string Identificacion { get; set; }
    public required string PasswordVieja { get; set; }
    public required string PasswordNueva { get; set; }
}