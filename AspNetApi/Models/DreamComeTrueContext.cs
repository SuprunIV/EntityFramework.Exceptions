using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
namespace DreamComeTrueApi.Models;

public partial class DreamComeTrueContext : DbContext
{
    public DreamComeTrueContext()
    {
    }

    public DreamComeTrueContext(DbContextOptions<DreamComeTrueContext> options)
        : base(options)
    {
      
    }

    public virtual DbSet<Dream> Dreams { get; set; }

    public virtual DbSet<Goal> Goals { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
       
    }
    //optionsBuilder.;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Dream>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("dream_pk");

            entity.ToTable("Dream", "MyDreams");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasColumnType("character varying");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("character varying");
        });

        modelBuilder.Entity<Goal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("goal_pk");

            entity.ToTable("Goal", "MyDreams");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("character varying");

            entity.HasOne(d => d.Dream).WithMany(p => p.Goals)
                .HasForeignKey(d => d.DreamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("goal_dream_id_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
