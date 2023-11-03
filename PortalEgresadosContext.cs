using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GraduatesPortalAPI;

public partial class PortalEgresadosContext : DbContext
{
    public PortalEgresadosContext()
    {
    }

    public PortalEgresadosContext(DbContextOptions<PortalEgresadosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccionUsuario> AccionUsuarios { get; set; }

    public virtual DbSet<Ciudad> Ciudads { get; set; }

    public virtual DbSet<Contacto> Contactos { get; set; }

    public virtual DbSet<Direccion> Direccions { get; set; }

    public virtual DbSet<Documento> Documentos { get; set; }

    public virtual DbSet<Educacion> Educacions { get; set; }

    public virtual DbSet<Egresado> Egresados { get; set; }

    public virtual DbSet<EgresadoDestacado> EgresadoDestacados { get; set; }

    public virtual DbSet<EgresadoHabilidad> EgresadoHabilidads { get; set; }

    public virtual DbSet<EgresadoIdioma> EgresadoIdiomas { get; set; }

    public virtual DbSet<ExperienciaLaboral> ExperienciaLaborals { get; set; }

    public virtual DbSet<Formacion> Formacions { get; set; }

    public virtual DbSet<Habilidad> Habilidads { get; set; }

    public virtual DbSet<Idioma> Idiomas { get; set; }

    public virtual DbSet<LocalidadPostal> LocalidadPostals { get; set; }

    public virtual DbSet<LogUsuario> LogUsuarios { get; set; }

    public virtual DbSet<Nivel> Nivels { get; set; }

    public virtual DbSet<Pais> Pais { get; set; }

    public virtual DbSet<Participante> Participantes { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<TipoContacto> TipoContactos { get; set; }

    public virtual DbSet<TipoDocumento> TipoDocumentos { get; set; }

    public virtual DbSet<TipoParticipante> TipoParticipantes { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<VCiudad> VCiudads { get; set; }

    public virtual DbSet<VNacionalidad> VNacionalidads { get; set; }

    public virtual DbSet<VPai> VPais { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=34.23.161.151;Initial Catalog=PortalEgresados;User ID=backend;Password=@uasd809@; Encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccionUsuario>(entity =>
        {
            entity.HasKey(e => e.AccionUsuarioId).HasName("PK__AccionUs__2FAB39B426B78549");

            entity.ToTable("AccionUsuario");

            entity.HasIndex(e => e.Nombre, "UQ__AccionUs__75E3EFCF4D0A4419").IsUnique();

            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Ciudad>((Action<Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Ciudad>>)(entity =>
        {
            entity.HasKey(e => e.CiudadId).HasName("PK__Ciudad__E826E770C7A13E35");

            entity.ToTable("Ciudad");

            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property<string>(e => e.Nombre)
                .HasMaxLength(60)
                .IsUnicode(false);

            RelationalForeignKeyBuilderExtensions.HasConstraintName<Pais, Ciudad>(entity.HasOne<Pais>(d => d.Pais).WithMany(p => p.Ciudads)
                .HasForeignKey((System.Linq.Expressions.Expression<Func<Ciudad, object?>>)(d => d.PaisId))
                .OnDelete(DeleteBehavior.ClientSetNull)
, "FK__Ciudad__PaisId__2E1BDC42");
        }));

        modelBuilder.Entity<Contacto>(entity =>
        {
            entity.HasKey(e => e.ContactoId).HasName("PK__Contacto__8E0F85E8D6153BA3");

            entity.ToTable("Contacto");

            entity.HasIndex(e => e.Nombre, "UQ__Contacto__75E3EFCFD048720A").IsUnique();

            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Mostrar).HasDefaultValueSql("((1))");
            entity.Property(e => e.Nombre)
                .HasMaxLength(60)
                .IsUnicode(false);

            entity.HasOne(d => d.Participante).WithMany(p => p.Contactos)
                .HasForeignKey(d => d.ParticipanteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Contacto__Partic__3E1D39E1");

            entity.HasOne(d => d.TipoContacto).WithMany(p => p.Contactos)
                .HasForeignKey(d => d.TipoContactoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Contacto__TipoCo__3F115E1A");
        });

        modelBuilder.Entity<Direccion>(entity =>
        {
            entity.HasKey(e => e.DireccionId).HasName("PK__Direccio__68906D649640904C");

            entity.ToTable("Direccion");

            entity.Property(e => e.DireccionPrincipal)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MostrarDireccionPrincipal).HasDefaultValueSql("((0))");

            entity.HasOne(d => d.LocalidadPostal).WithMany(p => p.Direccions)
                .HasForeignKey(d => d.LocalidadPostalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Direccion__Local__3A81B327");
        });

        modelBuilder.Entity<Documento>(entity =>
        {
            entity.HasKey(e => e.DocumentoId).HasName("PK__Document__5DDBFC765663B16E");

            entity.ToTable("Documento");

            entity.HasIndex(e => new { e.TipoDocumentoId, e.ParticipanteId }, "UQ__Document__4D4400430749AABC").IsUnique();

            entity.HasIndex(e => e.DocumentoNo, "UQ__Document__5DDB97EC55F3EB85").IsUnique();

            entity.Property(e => e.DocumentoNo)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Mostrar).HasDefaultValueSql("((0))");

            entity.HasOne(d => d.Participante).WithMany(p => p.Documentos)
                .HasForeignKey(d => d.ParticipanteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Documento__Parti__00200768");

            entity.HasOne(d => d.TipoDocumento).WithMany(p => p.Documentos)
                .HasForeignKey(d => d.TipoDocumentoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Documento__TipoD__7F2BE32F");
        });

        modelBuilder.Entity<Educacion>(entity =>
        {
            entity.HasKey(e => e.EducacionId).HasName("PK__Educacio__6301DF6EA0A6073E");

            entity.ToTable("Educacion");

            entity.HasIndex(e => new { e.EgresadoId, e.FormacionId }, "UQ__Educacio__4F4183023B9F4D71").IsUnique();

            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaEntrada).HasColumnType("date");
            entity.Property(e => e.FechaGraduacion).HasColumnType("date");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaSalida).HasColumnType("date");
            entity.Property(e => e.Mostrar).HasDefaultValueSql("((1))");
            entity.Property(e => e.Organizacion)
                .HasMaxLength(120)
                .IsUnicode(false);

            entity.HasOne(d => d.Egresado).WithMany(p => p.Educacions)
                .HasForeignKey(d => d.EgresadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Educacion__Egres__2DE6D218");

            entity.HasOne(d => d.Formacion).WithMany(p => p.Educacions)
                .HasForeignKey(d => d.FormacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Educacion__Forma__2EDAF651");
        });

        modelBuilder.Entity<Egresado>(entity =>
        {
            entity.HasKey(e => e.EgresadoId).HasName("PK__Egresado__CE4D7586662C1BEA");

            entity.ToTable("Egresado");

            entity.HasIndex(e => e.ParticipanteId, "UQ__Egresado__E6DEAC5E45755AD4").IsUnique();

            entity.Property(e => e.Acerca).HasColumnType("text");
            entity.Property(e => e.Edad).HasComputedColumnSql("(datediff(year,[FechaNac],getdate()))", false);
            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaNac).HasColumnType("date");
            entity.Property(e => e.Genero)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.MatriculaEgresado)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.MatriculaGrado)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.PrimerApellido)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PrimerNombre)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.SegundoApellido)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SegundoNombre)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.NacionalidadNavigation).WithMany(p => p.Egresados)
                .HasForeignKey(d => d.Nacionalidad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Egresado__Nacion__6EF57B66");

            entity.HasOne(d => d.Participante).WithOne(p => p.Egresado)
                .HasForeignKey<Egresado>(d => d.ParticipanteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Egresado__Partic__6E01572D");
        });

        modelBuilder.Entity<EgresadoDestacado>(entity =>
        {
            entity.HasKey(e => e.EgresadoDestacadoId).HasName("PK__Egresado__C3C0C83C735BF56A");

            entity.ToTable("EgresadoDestacado");

            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaDesde).HasColumnType("date");
            entity.Property(e => e.FechaHasta).HasColumnType("date");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Observacion).HasColumnType("text");

            entity.HasOne(d => d.Egresado).WithMany(p => p.EgresadoDestacados)
                .HasForeignKey(d => d.EgresadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EgresadoD__Egres__55009F39");
        });

        modelBuilder.Entity<EgresadoHabilidad>(entity =>
        {
            entity.HasKey(e => e.EgresadoHabilidadId).HasName("PK__Egresado__710742485EFDDBE9");

            entity.ToTable("EgresadoHabilidad");

            entity.HasIndex(e => new { e.EgresadoId, e.HabilidadId }, "UQ__Egresado__F9796A65DA3CA6D4").IsUnique();

            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Mostrar).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Egresado).WithMany(p => p.EgresadoHabilidads)
                .HasForeignKey(d => d.EgresadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EgresadoH__Egres__4C6B5938");

            entity.HasOne(d => d.Habilidad).WithMany(p => p.EgresadoHabilidads)
                .HasForeignKey(d => d.HabilidadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EgresadoH__Habil__4D5F7D71");
        });

        modelBuilder.Entity<EgresadoIdioma>(entity =>
        {
            entity.HasKey(e => e.EgresadoIdiomaId).HasName("PK__Egresado__95C75C42CF2DABFD");

            entity.ToTable("EgresadoIdioma");

            entity.HasIndex(e => new { e.EgresadoId, e.IdiomaId }, "UQ__Egresado__5E007617A12AD9EB").IsUnique();

            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Mostrar).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.Egresado).WithMany(p => p.EgresadoIdiomas)
                .HasForeignKey(d => d.EgresadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EgresadoI__Egres__0E6E26BF");

            entity.HasOne(d => d.Idioma).WithMany(p => p.EgresadoIdiomas)
                .HasForeignKey(d => d.IdiomaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EgresadoI__Idiom__0F624AF8");
        });

        modelBuilder.Entity<ExperienciaLaboral>(entity =>
        {
            entity.HasKey(e => e.ExperienciaLaboralId).HasName("PK__Experien__90603FE66B62E681");

            entity.ToTable("ExperienciaLaboral");

            entity.Property(e => e.Acerca).HasColumnType("text");
            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaEntrada).HasColumnType("date");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaSalida).HasColumnType("date");
            entity.Property(e => e.Mostrar).HasDefaultValueSql("((1))");
            entity.Property(e => e.Organizacion)
                .HasMaxLength(120)
                .IsUnicode(false);
            entity.Property(e => e.Posicion)
                .HasMaxLength(60)
                .IsUnicode(false);

            entity.HasOne(d => d.Egresado).WithMany(p => p.ExperienciaLaborals)
                .HasForeignKey(d => d.EgresadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Experienc__Egres__17F790F9");
        });

        modelBuilder.Entity<Formacion>(entity =>
        {
            entity.HasKey(e => e.FormacionId).HasName("PK__Formacio__10CF68591C530331");

            entity.ToTable("Formacion");

            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(80)
                .IsUnicode(false);

            entity.HasOne(d => d.Nivel).WithMany(p => p.Formacions)
                .HasForeignKey(d => d.NivelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Formacion__Nivel__25518C17");
        });

        modelBuilder.Entity<Habilidad>(entity =>
        {
            entity.HasKey(e => e.HabilidadId).HasName("PK__Habilida__7341FE2218C2D77A");

            entity.ToTable("Habilidad");

            entity.HasIndex(e => e.Nombre, "UQ__Habilida__75E3EFCFD130E9D4").IsUnique();

            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Idioma>(entity =>
        {
            entity.HasKey(e => e.IdiomaId).HasName("PK__Idioma__04D039080E60DD17");

            entity.ToTable("Idioma");

            entity.HasIndex(e => e.Nombre, "UQ__Idioma__75E3EFCFDEE4E56C").IsUnique();

            entity.HasIndex(e => e.Iso, "UQ__Idioma__C4979A236FDF1F59").IsUnique();

            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Iso)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISO");
            entity.Property(e => e.Nombre)
                .HasMaxLength(40)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LocalidadPostal>(entity =>
        {
            entity.HasKey(e => e.LocalidadPostalId).HasName("PK__Localida__A44BA8454F17F79B");

            entity.ToTable("LocalidadPostal");

            entity.Property(e => e.CodigoPostal)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Ciudad).WithMany(p => p.LocalidadPostals)
                .HasForeignKey(d => d.CiudadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Localidad__Ciuda__33D4B598");
        });

        modelBuilder.Entity<LogUsuario>(entity =>
        {
            entity.HasKey(e => e.LogUsuarioId).HasName("PK__LogUsuar__7DCF8168F868C1BD");

            entity.ToTable("LogUsuario");

            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.AccionUsuario).WithMany(p => p.LogUsuarios)
                .HasForeignKey(d => d.AccionUsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LogUsuari__Accio__5441852A");

            entity.HasOne(d => d.Usuario).WithMany(p => p.LogUsuarios)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LogUsuari__Usuar__534D60F1");
        });

        modelBuilder.Entity<Nivel>(entity =>
        {
            entity.HasKey(e => e.NivelId).HasName("PK__Nivel__316FA2776F6C92BB");

            entity.ToTable("Nivel");

            entity.HasIndex(e => e.Nombre, "UQ__Nivel__75E3EFCFAC1D66A8").IsUnique();

            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Prioridad).HasDefaultValueSql("((0))");
        });

        modelBuilder.Entity<Pais>(entity =>
        {
            entity.HasKey(e => e.PaisId).HasName("PK__Pais__B501E185E346BDE6");

            entity.HasIndex(e => e.Nombre, "UQ__Pais__75E3EFCFBDE93F59").IsUnique();

            entity.HasIndex(e => e.Iso, "UQ__Pais__C4979A235CC15C4E").IsUnique();

            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.GenticilioNac)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.Iso)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISO");
            entity.Property(e => e.Nombre)
                .HasMaxLength(80)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Participante>(entity =>
        {
            entity.HasKey(e => e.ParticipanteId).HasName("PK__Particip__E6DEAC5FE65C366E");

            entity.ToTable("Participante");

            entity.HasIndex(e => e.UsuarioId, "UQ__Particip__2B3DE7B97CFF33D0").IsUnique();

            entity.Property(e => e.EsEgresado).HasDefaultValueSql("((0))");
            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FotoPerfilUrl)
                .HasColumnType("text")
                .HasColumnName("FotoPerfilURL");

            entity.HasOne(d => d.Direccion).WithMany(p => p.Participantes)
                .HasForeignKey(d => d.DireccionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Participa__Direc__6477ECF3");

            entity.HasOne(d => d.TipoParticipante).WithMany(p => p.Participantes)
                .HasForeignKey(d => d.TipoParticipanteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Participa__TipoP__628FA481");

            entity.HasOne(d => d.Usuario).WithOne(p => p.Participante)
                .HasForeignKey<Participante>(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Participa__Usuar__6383C8BA");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.RolId).HasName("PK__Rol__F92302F18999D364");

            entity.ToTable("Rol");

            entity.HasIndex(e => e.Nombre, "UQ__Rol__75E3EFCF13BA7689").IsUnique();

            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TipoContacto>(entity =>
        {
            entity.HasKey(e => e.TipoContactoId).HasName("PK__TipoCont__2A6E82DC6295C609");

            entity.ToTable("TipoContacto");

            entity.HasIndex(e => e.Nombre, "UQ__TipoCont__75E3EFCF9D372AA8").IsUnique();

            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TipoDocumento>(entity =>
        {
            entity.HasKey(e => e.TipoDocumentoId).HasName("PK__TipoDocu__A329EA87103D4DD5");

            entity.ToTable("TipoDocumento");

            entity.HasIndex(e => e.Nombre, "UQ__TipoDocu__75E3EFCF878D21F6").IsUnique();

            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TipoParticipante>(entity =>
        {
            entity.HasKey(e => e.TipoParticipanteId).HasName("PK__TipoPart__5CEAA5C337CCA2DB");

            entity.ToTable("TipoParticipante");

            entity.HasIndex(e => e.Nombre, "UQ__TipoPart__75E3EFCFBD3DDCA9").IsUnique();

            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuario__2B3DE798E449FD41");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.UserName, "UQ__Usuario__C9F284565FF430B4").IsUnique();

            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Password).HasColumnType("text");
            entity.Property(e => e.Salt).HasColumnType("text");
            entity.Property(e => e.UserName)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuario__RolId__47DBAE45");
        });

        modelBuilder.Entity<VCiudad>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_Ciudad");

            entity.Property(e => e.NombreCiudad)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.Pais)
                .HasMaxLength(80)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VNacionalidad>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_Nacionalidad");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Iso)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISO");
            entity.Property(e => e.Nacionalidad)
                .HasMaxLength(60)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VPai>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_Pais");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Iso)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISO");
            entity.Property(e => e.Pais)
                .HasMaxLength(80)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
