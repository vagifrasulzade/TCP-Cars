using Microsoft.EntityFrameworkCore;

public class CarContext : DbContext
{
    public DbSet<Car> Cars { get; set; }

    public CarContext()
    {
        //Database.EnsureDeleted();

        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=AllCars
;Integrated Security=True;Trust Server Certificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>(c =>
        {
            c.HasKey(c => c.Id);
            c.Property(c => c.Id).ValueGeneratedOnAdd().UseIdentityColumn(1, 1);
            c.Property(c => c.Brand).HasMaxLength(50).IsRequired();
            c.Property(c => c.Model).HasMaxLength(50).IsRequired();
            c.Property(c => c.Price).IsRequired();
            c.Property(c => c.Year).IsRequired();
            c.Property(c => c.Colors).IsRequired();

            c.Property(c => c.Colors)
           .HasConversion<string>() 
           .IsRequired();

            c.Property(c => c.Currency) 
           .HasConversion<string>()
           .IsRequired();
        });
    }
}
