using Microsoft.EntityFrameworkCore;

namespace HT.Models
{
    public partial class HarmonyToysDatabaseContext : DbContext
    {
        public HarmonyToysDatabaseContext(DbContextOptions<HarmonyToysDatabaseContext> options) : base(options)
        {
        }

        public virtual DbSet<TblLoginDetails> TblLoginDetails { get; set; }
        public virtual DbSet<TblUserDetails> TblUserDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblLoginDetails>(entity =>
            {
                entity.HasKey(e => e.LoginId)
                    .HasName("PK_tblLoginDetails");

                entity.ToTable("tblLoginDetails");

                entity.Property(e => e.LoginId).HasColumnName("LoginID");

                entity.Property(e => e.IsAdmin)
                    .IsRequired()
                    .HasColumnType("char(1)")
                    .HasDefaultValueSql("'N'");

                entity.Property(e => e.IsApproved)
                    .IsRequired()
                    .HasColumnName("IsApproved ")
                    .HasColumnType("char(1)")
                    .HasDefaultValueSql("'Y'");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnType("varchar(150)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.UserId).HasDefaultValueSql("0");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnType("varchar(150)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblLoginDetails)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_tblLoginDetails_UserId");
            });

            modelBuilder.Entity<TblUserDetails>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK_tblVisitorRegistration");

                entity.ToTable("tblUserDetails");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnType("varchar(150)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.EmailId)
                    .IsRequired()
                    .HasColumnName("EmailID")
                    .HasColumnType("varchar(150)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Fname)
                    .IsRequired()
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Lname)
                    .IsRequired()
                    .HasColumnName("LName")
                    .HasColumnType("varchar(50)")
                    .HasDefaultValueSql("''");

                entity.Property(e => e.Phone)
                    .HasColumnType("numeric")
                    .HasDefaultValueSql("0");
            });
        }
    }
}