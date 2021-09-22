using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace WSProcesSa.Models
{
    public partial class ModelContext : DbContext
    {
        public ModelContext()
        {
        }

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
        }

        public ModelContext(string connectionString) 
        {
            this.ConnectionString = connectionString;
        }

        public virtual DbSet<EstadoTarea> EstadoTareas { get; set; }
        public virtual DbSet<FlujoTarea> FlujoTareas { get; set; }
        public virtual DbSet<JustificacionTarea> JustificacionTareas { get; set; }
        public virtual DbSet<PrioridadTarea> PrioridadTareas { get; set; }
        public virtual DbSet<Rol> Rols { get; set; }
        public virtual DbSet<Tarea> Tareas { get; set; }
        public virtual DbSet<TareaSubordinadum> TareaSubordinada { get; set; }
        public virtual DbSet<UnidadInterna> UnidadInternas { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        private string ConnectionString { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseOracle("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=xe)));User ID=PORTAFOLIO;Password=ASDASD123;Persist Security Info=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("PORTAFOLIO")
                .HasAnnotation("Relational:Collation", "USING_NLS_COMP");

            modelBuilder.Entity<EstadoTarea>(entity =>
            {
                entity.HasKey(e => e.IdEstadoTarea)
                    .HasName("SYS_C008994");

                entity.ToTable("ESTADO_TAREA");

                entity.Property(e => e.IdEstadoTarea)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_ESTADO_TAREA");

                entity.Property(e => e.DescripcionEstado)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("DESCRIPCION_ESTADO");
            });

            modelBuilder.Entity<FlujoTarea>(entity =>
            {
                entity.HasKey(e => e.IdFlujoTarea)
                    .HasName("SYS_C009022");

                entity.ToTable("FLUJO_TAREA");

                entity.Property(e => e.IdFlujoTarea)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_FLUJO_TAREA");

                entity.Property(e => e.DescripcionFlujoTarea)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("DESCRIPCION_FLUJO_TAREA");

                entity.Property(e => e.FkIdTarea)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("FK_ID_TAREA");

                entity.Property(e => e.NombreFlujoTarea)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("NOMBRE_FLUJO_TAREA");

                entity.HasOne(d => d.FkIdTareaNavigation)
                    .WithMany(p => p.FlujoTareas)
                    .HasForeignKey(d => d.FkIdTarea)
                    .HasConstraintName("FK_ID_TAREA_FLUJO_TAREA");
            });

            modelBuilder.Entity<JustificacionTarea>(entity =>
            {
                entity.HasKey(e => e.IdJustificacion)
                    .HasName("SYS_C008999");

                entity.ToTable("JUSTIFICACION_TAREA");

                entity.Property(e => e.IdJustificacion)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_JUSTIFICACION");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("DESCRIPCION");
            });

            modelBuilder.Entity<PrioridadTarea>(entity =>
            {
                entity.HasKey(e => e.IdPrioridad)
                    .HasName("SYS_C008997");

                entity.ToTable("PRIORIDAD_TAREA");

                entity.Property(e => e.IdPrioridad)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_PRIORIDAD");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("DESCRIPCION");
            });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasKey(e => e.IdRol)
                    .HasName("SYS_C007372");

                entity.ToTable("ROL");

                entity.Property(e => e.IdRol)
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_ROL");

                entity.Property(e => e.DescripcionRol)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("DESCRIPCION_ROL");

                entity.Property(e => e.NombreRol)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("NOMBRE_ROL");
            });

            modelBuilder.Entity<Tarea>(entity =>
            {
                entity.HasKey(e => e.IdTarea)
                    .HasName("SYS_C009007");

                entity.ToTable("TAREA");

                entity.Property(e => e.IdTarea)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_TAREA");

                entity.Property(e => e.AsignacionTarea)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ASIGNACION_TAREA");

                entity.Property(e => e.DescripcionTarea)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("DESCRIPCION_TAREA");

                entity.Property(e => e.FechaPlazo)
                    .HasColumnType("DATE")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("FECHA_PLAZO");

                entity.Property(e => e.FkEstadoTarea)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("FK_ESTADO_TAREA");

                entity.Property(e => e.FkIdJustificacion)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("FK_ID_JUSTIFICACION");

                entity.Property(e => e.FkPrioridadTarea)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("FK_PRIORIDAD_TAREA");

                entity.Property(e => e.FkRutUsuario)
                    .IsRequired()
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("FK_RUT_USUARIO");

                entity.Property(e => e.NombreTarea)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("NOMBRE_TAREA");

                entity.Property(e => e.ReporteProblema)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("REPORTE_PROBLEMA");

                entity.HasOne(d => d.FkEstadoTareaNavigation)
                    .WithMany(p => p.Tareas)
                    .HasForeignKey(d => d.FkEstadoTarea)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ESTADO_TAREA");

                entity.HasOne(d => d.FkIdJustificacionNavigation)
                    .WithMany(p => p.Tareas)
                    .HasForeignKey(d => d.FkIdJustificacion)
                    .HasConstraintName("FK_ID_JUSTIFICACION");

                entity.HasOne(d => d.FkPrioridadTareaNavigation)
                    .WithMany(p => p.Tareas)
                    .HasForeignKey(d => d.FkPrioridadTarea)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PRIORIDAD_TAREA");

                entity.HasOne(d => d.FkRutUsuarioNavigation)
                    .WithMany(p => p.Tareas)
                    .HasForeignKey(d => d.FkRutUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RUT_USUARIO");
            });

            modelBuilder.Entity<TareaSubordinadum>(entity =>
            {
                entity.HasKey(e => e.IdTareaSubordinada)
                    .HasName("SYS_C009015");

                entity.ToTable("TAREA_SUBORDINADA");

                entity.Property(e => e.IdTareaSubordinada)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_TAREA_SUBORDINADA");

                entity.Property(e => e.DescripcionSubordinada)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("DESCRIPCION_SUBORDINADA");

                entity.Property(e => e.FkIdTarea)
                    .HasColumnType("NUMBER")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("FK_ID_TAREA");

                entity.Property(e => e.NombreSubordinada)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("NOMBRE_SUBORDINADA");

                entity.HasOne(d => d.FkIdTareaNavigation)
                    .WithMany(p => p.TareaSubordinada)
                    .HasForeignKey(d => d.FkIdTarea)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ID_TAREA");
            });

            modelBuilder.Entity<UnidadInterna>(entity =>
            {
                entity.HasKey(e => e.IdUnidadInterna)
                    .HasName("SYS_C007375");

                entity.ToTable("UNIDAD_INTERNA");

                entity.Property(e => e.IdUnidadInterna)
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_UNIDAD_INTERNA");

                entity.Property(e => e.DescripcionUnidad)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("DESCRIPCION_UNIDAD");

                entity.Property(e => e.NombreUnidad)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("NOMBRE_UNIDAD");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.RutUsuario)
                    .HasName("SYS_C007386");

                entity.ToTable("USUARIO");

                entity.Property(e => e.RutUsuario)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("RUT_USUARIO");

                entity.Property(e => e.ApellidoUsuario)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("APELLIDO_USUARIO");

                entity.Property(e => e.CorreoElectronico)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("CORREO_ELECTRONICO");

                entity.Property(e => e.IdRolUsuario)
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_ROL_USUARIO");

                entity.Property(e => e.IdUnidadInternaUsuario)
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID_UNIDAD_INTERNA_USUARIO");

                entity.Property(e => e.NombreUsuario)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("NOMBRE_USUARIO");

                entity.Property(e => e.NumTelefono)
                    .HasColumnType("NUMBER(38)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("NUM_TELEFONO");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("PASSWORD");

                entity.Property(e => e.SegundoApellido)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("SEGUNDO_APELLIDO");

                entity.Property(e => e.SegundoNombre)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("SEGUNDO_NOMBRE");

                entity.HasOne(d => d.IdRolUsuarioNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdRolUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ROL_USUARIO");

                entity.HasOne(d => d.IdUnidadInternaUsuarioNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdUnidadInternaUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ID_UNIDAD_INTERNA");
            });

            modelBuilder.HasSequence("ESTADO_TAREA_SEQ");

            modelBuilder.HasSequence("FLUJO_TAREA_SEQ");

            modelBuilder.HasSequence("FLUJO_TAREA_SEQ1");

            modelBuilder.HasSequence("JUSTIFICACION_TAREA_SEQ");

            modelBuilder.HasSequence("PRIORIDAD_TAREA_SEQ");

            modelBuilder.HasSequence("ROL_SEQ");

            modelBuilder.HasSequence("ROL_SEQ1");

            modelBuilder.HasSequence("TAREA_SEQ");

            modelBuilder.HasSequence("TAREA_SUBORDINADA_SEQ");

            modelBuilder.HasSequence("UNIDAD_INTERNA_SEQ");

            modelBuilder.HasSequence("USUARIO_SEQ");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
