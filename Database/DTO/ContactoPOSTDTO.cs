namespace PortalEgresadosAPI.Database.DTO
{
    public class ContactoPOSTDTO
    {

        public int ParticipanteId { get; set; }

        public int TipoContactoId { get; set; }

        public string Nombre { get; set; } = null!;

        public bool? Mostrar { get; set; }

        public bool? Estado { get; set; }

        public Contacto ToContacto()
        {
            return new Contacto
            {
                ContactoId = 0,
                ParticipanteId = this.ParticipanteId,
                Nombre = this.Nombre,
                Estado = this.Estado,
                Mostrar = this.Mostrar
            };
        }
    }
}
