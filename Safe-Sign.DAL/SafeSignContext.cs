using Microsoft.EntityFrameworkCore;

using Safe_Sign.DAL.Models;

namespace Safe_Sign.DAL
{
    public class SafeSignContext : DbContext
    {
        public DbSet<Document>? Document { get; set; }

        public DbSet<DocumentFile>? DocumentFile { get; set; }

        public DbSet<Signature>? Signature { get; set; }

        public DbSet<LegalPerson>? LegalPerson { get; set; }

        public DbSet<Marker>? Marker { get; set; }

        public DbSet<Person>? Person { get; set; }

        public DbSet<Profile>? Profile { get; set; }

        public DbSet<Theme>? Theme { get; set; }

        public DbSet<User>? User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = Environment.GetEnvironmentVariable("SAFE_SIGN_CONNECTIONSTRING");
                var serverVersion = ServerVersion.AutoDetect(connectionString);
                optionsBuilder.UseMySql(connectionString, serverVersion);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<LegalPerson>()
                .HasIndex(u => u.CompanyName)
                .IsUnique();

            builder.Entity<LegalPerson>()
                .HasIndex(u => u.CNPJ)
                .IsUnique();

            builder.Entity<Person>()
                .HasIndex(p => p.CPF)
                .IsUnique();

            builder.Entity<User>()
                .Property(u => u.UserType)
                .HasConversion<string>();

            builder.Entity<Profile>()
                .Property(p => p.Type)
                .HasConversion<string>();

            builder.Entity<Document>()
                .Property(d => d.Status)
                .HasConversion<string>();
        }
    }
}
