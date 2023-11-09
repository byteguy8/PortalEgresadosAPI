using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PortalEgresadosAPI;

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

    public virtual DbSet<LogTransaccional> LogTransaccionals { get; set; }

    public virtual DbSet<LogUsuario> LogUsuarios { get; set; }

    public virtual DbSet<Nivel> Nivels { get; set; }

    public virtual DbSet<Pai> Pais { get; set; }

    public virtual DbSet<Participante> Participantes { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<TipoContacto> TipoContactos { get; set; }

    public virtual DbSet<TipoDocumento> TipoDocumentos { get; set; }

    public virtual DbSet<TipoParticipante> TipoParticipantes { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<VCiudad> VCiudads { get; set; }

    public virtual DbSet<VContactoConTipo> VContactoConTipos { get; set; }

    public virtual DbSet<VDireccion> VDireccions { get; set; }

    public virtual DbSet<VEducacionConDetalle> VEducacionConDetalles { get; set; }

    public virtual DbSet<VEgresado> VEgresados { get; set; }

    public virtual DbSet<VEgresadoIdioma> VEgresadoIdiomas { get; set; }

    public virtual DbSet<VExperienciaLaboral> VExperienciaLaborals { get; set; }

    public virtual DbSet<VLocalidadPostal> VLocalidadPostals { get; set; }

    public virtual DbSet<VNacionalidad> VNacionalidads { get; set; }

    public virtual DbSet<VPai> VPais { get; set; }

    public virtual DbSet<VParticipante> VParticipantes { get; set; }

    public virtual DbSet<VParticipanteDocumento> VParticipanteDocumentos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=34.23.161.151;Initial Catalog=PortalEgresados;User ID=backend;Password=@uasd809@;Encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("db_datareader");

        modelBuilder.Entity<AccionUsuario>(entity =>
        {
            entity.HasKey(e => e.AccionUsuarioId).HasName("PK_AccionUsuario_AccionUsuarioId");

            entity.ToTable("AccionUsuario", "dbo", tb => tb.HasTrigger("TR_RegistrarLogTransaccionalAccionUsuario"));

            entity.HasIndex(e => e.Nombre, "UQ_AccionUsuario_Nombre").IsUnique();

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

        modelBuilder.Entity<Ciudad>(entity =>
        {
            entity.HasKey(e => e.CiudadId).HasName("PK_Ciudad_CiudadId");

            entity.ToTable("Ciudad", "dbo", tb => tb.HasTrigger("TR_RegistrarLogTransaccionalCiudad"));

            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(60)
                .IsUnicode(false);

            entity.HasOne(d => d.Pais).WithMany(p => p.Ciudads)
                .HasForeignKey(d => d.PaisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ciudad_PaisId");
        });

        modelBuilder.Entity<Contacto>(entity =>
        {
            entity.HasKey(e => e.ContactoId).HasName("PK_Contacto_ContactoId");

            entity.ToTable("Contacto", "dbo", tb => tb.HasTrigger("TR_RegistrarLogTransaccionalContacto"));

            entity.HasIndex(e => e.Nombre, "UQ_Contacto_Nombre").IsUnique();

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
                .HasConstraintName("FK_Contacto_ParticipanteId");

            entity.HasOne(d => d.TipoContacto).WithMany(p => p.Contactos)
                .HasForeignKey(d => d.TipoContactoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Contacto_TipoContactoId");
        });

        modelBuilder.Entity<Direccion>(entity =>
        {
            entity.HasKey(e => e.DireccionId).HasName("PK_Direccion_DireccionId");

            entity.ToTable("Direccion", "dbo", tb => tb.HasTrigger("TR_RegistrarLogTransaccionalDireccion"));

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
                .HasConstraintName("FK_Direccion_LocalidadPostalId");
        });

        modelBuilder.Entity<Documento>(entity =>
        {
            entity.HasKey(e => e.DocumentoId).HasName("PK_Documento_DocumentoId");

            entity.ToTable("Documento", "dbo", tb => tb.HasTrigger("TR_RegistrarLogTransaccionalDocumento"));

            entity.HasIndex(e => e.DocumentoNo, "UQ_Documento_DocumentoNo").IsUnique();

            entity.HasIndex(e => new { e.TipoDocumentoId, e.ParticipanteId }, "UQ_Documento_TipoDocumentoId_ParticipanteId").IsUnique();

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
                .HasConstraintName("FK_Documento_ParticipanteId");

            entity.HasOne(d => d.TipoDocumento).WithMany(p => p.Documentos)
                .HasForeignKey(d => d.TipoDocumentoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Documento_TipoDocumentoId");
        });

        modelBuilder.Entity<Educacion>(entity =>
        {
            entity.HasKey(e => e.EducacionId).HasName("PK_Educacion_EducacionId");

            entity.ToTable("Educacion", "dbo", tb => tb.HasTrigger("TR_RegistrarLogTransaccionalEducacion"));

            entity.HasIndex(e => new { e.EgresadoId, e.FormacionId }, "UQ_Educacion_EgresadoId_FormacionId").IsUnique();

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
                .HasConstraintName("FK_Educacion_EgresadoId");

            entity.HasOne(d => d.Formacion).WithMany(p => p.Educacions)
                .HasForeignKey(d => d.FormacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Educacion_FormacionId");
        });

        modelBuilder.Entity<Egresado>(entity =>
        {
            entity.HasKey(e => e.EgresadoId).HasName("PK_Egresado_EgresadoId");

            entity.ToTable("Egresado", "dbo", tb => tb.HasTrigger("TR_RegistrarLogTransaccionalEgresado"));

            entity.HasIndex(e => e.ParticipanteId, "UQ_Egresado_ParticipanteId").IsUnique();

            entity.Property(e => e.Acerca).IsUnicode(false);
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
                .HasConstraintName("FK_Egresado_Nacionalidad");

            entity.HasOne(d => d.Participante).WithOne(p => p.Egresado)
                .HasForeignKey<Egresado>(d => d.ParticipanteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Egresado_ParticipanteId");
        });

        modelBuilder.Entity<EgresadoDestacado>(entity =>
        {
            entity.HasKey(e => e.EgresadoDestacadoId).HasName("PK_EgresadoDestacado_EgresadoDestacadoId");

            entity.ToTable("EgresadoDestacado", "dbo", tb => tb.HasTrigger("TR_RegistrarLogTransaccionalEgresadoDestacado"));

            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaDesde).HasColumnType("date");
            entity.Property(e => e.FechaHasta).HasColumnType("date");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Observacion).IsUnicode(false);

            entity.HasOne(d => d.Egresado).WithMany(p => p.EgresadoDestacados)
                .HasForeignKey(d => d.EgresadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EgresadoDestacado_EgresadoId");
        });

        modelBuilder.Entity<EgresadoHabilidad>(entity =>
        {
            entity.HasKey(e => e.EgresadoHabilidadId).HasName("PK_EgresadoHabilidad_EgresadoHabilidadId");

            entity.ToTable("EgresadoHabilidad", "dbo", tb => tb.HasTrigger("TR_RegistrarLogTransaccionalEgresadoHabilidad"));

            entity.HasIndex(e => new { e.EgresadoId, e.HabilidadId }, "UQ_EgresadoHabilidad_EgresadoId_HabilidadId").IsUnique();

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
                .HasConstraintName("FK_EgresadoHabilidad_EgresadoId");

            entity.HasOne(d => d.Habilidad).WithMany(p => p.EgresadoHabilidads)
                .HasForeignKey(d => d.HabilidadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EgresadoHabilidad_HabilidadId");
        });

        modelBuilder.Entity<EgresadoIdioma>(entity =>
        {
            entity.HasKey(e => e.EgresadoIdiomaId).HasName("PK_EgresadoIdioma_EgresadoIdiomaId");

            entity.ToTable("EgresadoIdioma", "dbo", tb => tb.HasTrigger("TR_RegistrarLogTransaccionalEgresadoIdioma"));

            entity.HasIndex(e => new { e.EgresadoId, e.IdiomaId }, "UQ_EgresadoIdioma_EgresadoId_IdiomaId").IsUnique();

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
                .HasConstraintName("FK_EgresadoIdioma_EgresadoId");

            entity.HasOne(d => d.Idioma).WithMany(p => p.EgresadoIdiomas)
                .HasForeignKey(d => d.IdiomaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EgresadoIdioma_IdiomaId");
        });

        modelBuilder.Entity<ExperienciaLaboral>(entity =>
        {
            entity.HasKey(e => e.ExperienciaLaboralId).HasName("PK_ExperienciaLaboral_ExperienciaLaboralId");

            entity.ToTable("ExperienciaLaboral", "dbo", tb => tb.HasTrigger("TR_RegistrarLogTransaccionalExperienciaLaboral"));

            entity.Property(e => e.Acerca).IsUnicode(false);
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
                .HasConstraintName("FK_ExperienciaLaboral_EgresadoId");
        });

        modelBuilder.Entity<Formacion>(entity =>
        {
            entity.HasKey(e => e.FormacionId).HasName("PK_Formacion_FormacionId");

            entity.ToTable("Formacion", "dbo", tb => tb.HasTrigger("TR_RegistrarLogTransaccionalFormacion"));

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
                .HasConstraintName("FK_Formacion_NivelId");
        });

        modelBuilder.Entity<Habilidad>(entity =>
        {
            entity.HasKey(e => e.HabilidadId).HasName("PK_Habilidad_HabilidadId");

            entity.ToTable("Habilidad", "dbo", tb => tb.HasTrigger("TR_RegistrarLogTransaccionalHabilidad"));

            entity.HasIndex(e => e.Nombre, "UQ_Habilidad_Nombre").IsUnique();

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
            entity.HasKey(e => e.IdiomaId).HasName("PK_Idioma_IdiomaId");

            entity.ToTable("Idioma", "dbo", tb => tb.HasTrigger("TR_RegistrarLogTransaccionalIdioma"));

            entity.HasIndex(e => e.Iso, "UQ_Idioma_ISO").IsUnique();

            entity.HasIndex(e => e.Nombre, "UQ_Idioma_Nombre").IsUnique();

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
            entity.HasKey(e => e.LocalidadPostalId).HasName("PK_LocalidadPostal_LocalidadPostalId");

            entity.ToTable("LocalidadPostal", "dbo", tb => tb.HasTrigger("TR_RegistrarLogTransaccionalLocalidadPostal"));

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
                .HasConstraintName("FK_LocalidadPostal_CiudadId");
        });

        modelBuilder.Entity<LogTransaccional>(entity =>
        {
            entity.HasKey(e => e.LogTransaccionalId).HasName("PK_LogTransaccional_LogTransaccionalID");

            entity.ToTable("LogTransaccional", "dbo");

            entity.Property(e => e.LogTransaccionalId).HasColumnName("LogTransaccionalID");
            entity.Property(e => e.Accion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaHora)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TablaAfectada)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioDb)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValueSql("(suser_sname())")
                .HasColumnName("UsuarioDB");
        });

        modelBuilder.Entity<LogUsuario>(entity =>
        {
            entity.HasKey(e => e.LogUsuarioId).HasName("PK_LogUsuario_LogUsuarioId");

            entity.ToTable("LogUsuario", "dbo", tb => tb.HasTrigger("TR_RegistrarLogTransaccionalLogUsuario"));

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
                .HasConstraintName("FK_LogUsuario_AccionUsuarioId");

            entity.HasOne(d => d.Usuario).WithMany(p => p.LogUsuarios)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LogUsuario_UsuarioId");
        });

        modelBuilder.Entity<Nivel>(entity =>
        {
            entity.HasKey(e => e.NivelId).HasName("PK_Nivel_NivelId");

            entity.ToTable("Nivel", "dbo", tb => tb.HasTrigger("TR_RegistrarLogTransaccionalNivel"));

            entity.HasIndex(e => e.Nombre, "UQ_Nivel_Nombre").IsUnique();

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

        modelBuilder.Entity<Pai>(entity =>
        {
            entity.HasKey(e => e.PaisId).HasName("PK_Pais_PaisId");

            entity.ToTable("Pais", "dbo", tb => tb.HasTrigger("TR_RegistrarLogTransaccionalPais"));

            entity.HasIndex(e => e.Iso, "UQ_Pais_ISO").IsUnique();

            entity.HasIndex(e => e.Nombre, "UQ_Pais_Nombre").IsUnique();

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
            entity.HasKey(e => e.ParticipanteId).HasName("PK_Participante_ParticipanteId");

            entity.ToTable("Participante", "dbo", tb => tb.HasTrigger("TR_RegistrarLogTransaccionalParticipante"));

            entity.HasIndex(e => e.UsuarioId, "UQ_Participante_UsuarioId").IsUnique();

            entity.Property(e => e.EsEgresado).HasDefaultValueSql("((0))");
            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FotoPerfilUrl)
                .IsUnicode(false)
                .HasColumnName("FotoPerfilURL");

            entity.HasOne(d => d.Direccion).WithMany(p => p.Participantes)
                .HasForeignKey(d => d.DireccionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Participante_DireccionId");

            entity.HasOne(d => d.TipoParticipante).WithMany(p => p.Participantes)
                .HasForeignKey(d => d.TipoParticipanteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Participante_TipoParticipanteId");

            entity.HasOne(d => d.Usuario).WithOne(p => p.Participante)
                .HasForeignKey<Participante>(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Participante_UsuarioId");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.RolId).HasName("PK_Rol_RolId");

            entity.ToTable("Rol", "dbo", tb => tb.HasTrigger("TR_RegistrarLogTransaccionalRol"));

            entity.HasIndex(e => e.Nombre, "UQ_Rol_Nombre").IsUnique();

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
            entity.HasKey(e => e.TipoContactoId).HasName("PK_TipoContacto_TipoContactoId");

            entity.ToTable("TipoContacto", "dbo", tb => tb.HasTrigger("TR_RegistrarLogTransaccionalTipoContacto"));

            entity.HasIndex(e => e.Nombre, "UQ_TipoContacto_Nombre").IsUnique();

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
            entity.HasKey(e => e.TipoDocumentoId).HasName("PK_TipoDocumento_TipoDocumentoId");

            entity.ToTable("TipoDocumento", "dbo", tb => tb.HasTrigger("TR_RegistrarLogTransaccionalTipoDocumento"));

            entity.HasIndex(e => e.Nombre, "UQ_TipoDocumento_Nombre").IsUnique();

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
            entity.HasKey(e => e.TipoParticipanteId).HasName("PK_TipoParticipante_TipoParticipanteId");

            entity.ToTable("TipoParticipante", "dbo", tb => tb.HasTrigger("TR_RegistrarLogTransaccionalTipoParticipante"));

            entity.HasIndex(e => e.Nombre, "UQ_TipoParticipante_Nombre").IsUnique();

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
            entity.HasKey(e => e.UsuarioId).HasName("PK_Usuario_UsuarioID");

            entity.ToTable("Usuario", "dbo", tb => tb.HasTrigger("TR_RegistrarLogTransaccionalUsuario"));

            entity.HasIndex(e => e.UserName, "UQ_Usuario_UserName").IsUnique();

            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
            entity.Property(e => e.Estado).HasDefaultValueSql("((1))");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Password).IsUnicode(false);
            entity.Property(e => e.Salt).IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_RolId");
        });

        modelBuilder.Entity<VCiudad>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_Ciudad", "dbo");

            entity.Property(e => e.NombreCiudad)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.Pais)
                .HasMaxLength(80)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VContactoConTipo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_ContactoConTipo", "dbo");

            entity.Property(e => e.IdInContacto).HasColumnName("idInContacto");
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(81)
                .IsUnicode(false);
            entity.Property(e => e.NombreContacto)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.NombreTipoContacto)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VDireccion>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_Direccion", "dbo");

            entity.Property(e => e.Ciudad)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.CodigoPostal)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Localidad)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Pais)
                .HasMaxLength(80)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VEducacionConDetalle>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_EducacionConDetalle", "dbo");

            entity.Property(e => e.FechaEntrada).HasColumnType("date");
            entity.Property(e => e.FechaGraduacion).HasColumnType("date");
            entity.Property(e => e.FechaSalida).HasColumnType("date");
            entity.Property(e => e.IdInEducacion).HasColumnName("idInEducacion");
            entity.Property(e => e.IdInFormacion).HasColumnName("idInFormacion");
            entity.Property(e => e.NombreFormacion)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.NombreNivel)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Organizacion)
                .HasMaxLength(120)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VEgresado>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_Egresado", "dbo");

            entity.Property(e => e.Acerca).IsUnicode(false);
            entity.Property(e => e.Ciudad)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.CodigoPostal)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DireccionPrincipal)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FechaNac).HasColumnType("date");
            entity.Property(e => e.FotoPerfilUrl)
                .IsUnicode(false)
                .HasColumnName("FotoPerfilURL");
            entity.Property(e => e.Genero)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.GenticilioNac)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.Localidad)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(165)
                .IsUnicode(false);
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Pais)
                .HasMaxLength(80)
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
            entity.Property(e => e.TipoParticipante)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VEgresadoIdioma>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_EgresadoIdioma", "dbo");

            entity.Property(e => e.Idioma)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Iso)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ISO");
        });

        modelBuilder.Entity<VExperienciaLaboral>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_ExperienciaLaboral", "dbo");

            entity.Property(e => e.Acerca).IsUnicode(false);
            entity.Property(e => e.FechaEntrada).HasColumnType("date");
            entity.Property(e => e.FechaSalida).HasColumnType("date");
            entity.Property(e => e.Organizacion)
                .HasMaxLength(120)
                .IsUnicode(false);
            entity.Property(e => e.Posicion)
                .HasMaxLength(60)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VLocalidadPostal>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_LocalidadPostal", "dbo");

            entity.Property(e => e.Ciudad)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.CodigoPostal)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Localidad)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Pais)
                .HasMaxLength(80)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VNacionalidad>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_Nacionalidad", "dbo");

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
                .ToView("v_Pais", "dbo");

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

        modelBuilder.Entity<VParticipante>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_Participante", "dbo");

            entity.Property(e => e.Ciudad)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.CodigoPostal)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DireccionPrincipal)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.EsEgresado)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.FotoPerfilUrl)
                .IsUnicode(false)
                .HasColumnName("FotoPerfilURL");
            entity.Property(e => e.Localidad)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Pais)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.TipoParticipante)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VParticipanteDocumento>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_ParticipanteDocumento", "dbo");

            entity.Property(e => e.DocumentoNo)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
