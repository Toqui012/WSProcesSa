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

        public virtual DbSet<Rol> Rols { get; set; }
        public virtual DbSet<UnidadInterna> UnidadInternas { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }

        private string ConnectionString { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseOracle("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=xe)));User ID=portafolio;Password=portafolio;Persist Security Info=True");

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("PORTAFOLIO")
                .HasAnnotation("Relational:Collation", "USING_NLS_COMP");

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasKey(e => e.IdRol)
                    .HasName("SYS_C007622");

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

            modelBuilder.Entity<UnidadInterna>(entity =>
            {
                entity.HasKey(e => e.IdUnidadInterna)
                    .HasName("SYS_C007625");

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
                    .HasName("SYS_C007636");

                entity.ToTable("USUARIO");

                entity.Property(e => e.RutUsuario)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("RUT_USUARIO");

                entity.Property(e => e.ApellidoUsuario)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("APELLIDO_USUARIO");

                entity.Property(e => e.CorreoElectronico)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CORREO_ELECTRONICO");

                entity.Property(e => e.IdRolUsuario)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("ID_ROL_USUARIO");

                entity.Property(e => e.IdUnidadInternaUsuario)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("ID_UNIDAD_INTERNA_USUARIO");

                entity.Property(e => e.NombreUsuario)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE_USUARIO");

                entity.Property(e => e.NumTelefono)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("NUM_TELEFONO");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("PASSWORD");

                entity.Property(e => e.SegundoApellido)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SEGUNDO_APELLIDO");

                entity.Property(e => e.SegundoNombre)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
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

            modelBuilder.HasSequence("ROL_SEQ");

            modelBuilder.HasSequence("UNIDAD_INTERNA_SEQ");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
