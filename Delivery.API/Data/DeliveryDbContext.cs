using Delivery.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Delivery.API.Data
{
    public class DeliveryDbContext : DbContext
    {
        public DeliveryDbContext(DbContextOptions<DeliveryDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Courier> Couriers => Set<Courier>();
        public DbSet<Administrator> Administrators => Set<Administrator>();

        public DbSet<Restaurant> Restaurants => Set<Restaurant>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<Promotion> Promotions => Set<Promotion>();
        public DbSet<Notification> Notifications => Set<Notification>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureUsers(modelBuilder);
            ConfigureCatalog(modelBuilder);
            ConfigureCartAndOrders(modelBuilder);
            ConfigurePaymentsAndExtras(modelBuilder);
            ConfigureDateTimeUtc(modelBuilder);

            DeliverySeed.Seed(modelBuilder);
        }

        // Npgsql exige Kind=Utc para "timestamp with time zone". Esto normaliza
        // cualquier DateTime que llegue con Kind=Unspecified (ej. desde JSON sin offset).
        private static void ConfigureDateTimeUtc(ModelBuilder modelBuilder)
        {
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v.Kind == DateTimeKind.Utc ? v : DateTime.SpecifyKind(v, DateTimeKind.Utc),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
                v => v.HasValue ? (v.Value.Kind == DateTimeKind.Utc ? v.Value : DateTime.SpecifyKind(v.Value, DateTimeKind.Utc)) : v,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                        property.SetValueConverter(dateTimeConverter);
                    else if (property.ClrType == typeof(DateTime?))
                        property.SetValueConverter(nullableDateTimeConverter);
                }
            }
        }

        private static void ConfigureUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
                entity.Property(u => u.LastName).HasMaxLength(100).IsRequired();
                entity.Property(u => u.Email).HasMaxLength(150).IsRequired();

                entity.HasDiscriminator<string>("UserType")
                    .HasValue<Client>("Client")
                    .HasValue<Courier>("Courier")
                    .HasValue<Administrator>("Administrator");
            });

            modelBuilder.Entity<Administrator>().Property(a => a.Salary).HasColumnType("decimal(18,2)");
        }

        private static void ConfigureCatalog(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Price).HasColumnType("decimal(18,2)");

                entity.HasOne(p => p.Restaurant)
                    .WithMany(r => r.Products)
                    .HasForeignKey(p => p.RestaurantId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(p => p.Category)
                    .WithMany(c => c.Products)
                    .HasForeignKey(p => p.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureCartAndOrders(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.Property(c => c.Subtotal).HasColumnType("decimal(18,2)");

                entity.HasOne(c => c.Client)
                    .WithOne(cl => cl.Cart)
                    .HasForeignKey<Cart>(c => c.ClientId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.Property(ci => ci.UnitPrice).HasColumnType("decimal(18,2)");

                entity.HasOne(ci => ci.Cart)
                    .WithMany(c => c.Items)
                    .HasForeignKey(ci => ci.CartId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ci => ci.Product)
                    .WithMany()
                    .HasForeignKey(ci => ci.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(o => o.Total).HasColumnType("decimal(18,2)");

                entity.HasOne(o => o.Client)
                    .WithMany(cl => cl.Orders)
                    .HasForeignKey(o => o.ClientId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(o => o.Courier)
                    .WithMany(co => co.Orders)
                    .HasForeignKey(o => o.CourierId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(o => o.Restaurant)
                    .WithMany(r => r.Orders)
                    .HasForeignKey(o => o.RestaurantId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.Property(od => od.UnitPrice).HasColumnType("decimal(18,2)");
                entity.Property(od => od.Subtotal).HasColumnType("decimal(18,2)");

                entity.HasOne(od => od.Order)
                    .WithMany(o => o.OrderDetails)
                    .HasForeignKey(od => od.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(od => od.Product)
                    .WithMany()
                    .HasForeignKey(od => od.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigurePaymentsAndExtras(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(p => p.Amount).HasColumnType("decimal(18,2)");

                entity.HasOne(p => p.Order)
                    .WithOne(o => o.Payment)
                    .HasForeignKey<Payment>(p => p.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Promotion>(entity =>
            {
                entity.Property(p => p.Discount).HasColumnType("decimal(18,2)");
                entity.HasIndex(p => p.Code).IsUnique();
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasOne(n => n.User)
                    .WithMany(u => u.Notifications)
                    .HasForeignKey(n => n.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
