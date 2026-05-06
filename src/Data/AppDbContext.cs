using Microsoft.EntityFrameworkCore;
using MyProject.Models;

namespace MyProject.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<SessionVersion> SessionVersions => Set<SessionVersion>();
    public DbSet<CartSession> CartSessions => Set<CartSession>();
    public DbSet<CartSessionItem> CartSessionItems => Set<CartSessionItem>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<OrderStatusHistory> OrderStatusHistories => Set<OrderStatusHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SessionVersion>(entity =>
        {
            entity.ToTable("SessionVersions");
            entity.HasKey(x => x.Key);
            entity.Property(x => x.Key).HasMaxLength(50);
            entity.Property(x => x.Value).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");

            entity.HasIndex(x => x.Email).IsUnique();

            entity.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.FullName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.PasswordHash)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.Price)
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.Discount)
                .HasColumnType("decimal(5,2)");

            entity.Property(x => x.ImageUrl)
                .HasMaxLength(500);

            entity.Property(x => x.GalleryImages)
                .HasMaxLength(1000);

            entity.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            entity.HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Orders");

            entity.Property(x => x.TotalAmount)
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            entity.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.ToTable("OrderItems");

            entity.Property(x => x.UnitPrice)
                .HasColumnType("decimal(18,2)");

            entity.HasOne(x => x.Order)
                .WithMany(x => x.OrderItems)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CartSession>(entity =>
        {
            entity.ToTable("CartSessions");
            entity.HasIndex(x => x.SessionId);
            entity.HasIndex(x => x.UserId);
            entity.HasMany(x => x.Items)
                .WithOne(x => x.CartSession)
                .HasForeignKey(x => x.CartSessionId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<CartSessionItem>(entity =>
        {
            entity.ToTable("CartSessionItems");
            entity.HasIndex(x => new { x.CartSessionId, x.ProductId }).IsUnique();
            entity.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("Payments");
            entity.Property(x => x.Amount).HasColumnType("decimal(18,2)");
            entity.HasOne(x => x.Order)
                .WithMany(x => x.Payments)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderStatusHistory>(entity =>
        {
            entity.ToTable("OrderStatusHistories");
            entity.HasOne(x => x.Order)
                .WithMany(x => x.StatusHistory)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                Id = 1,
                Name = "Men's Watches",
                Description = "Luxury and premium watches for men",
                CreatedAt = new DateTime(2026, 1, 1)
            },
            new Category
            {
                Id = 2,
                Name = "Women's Watches",
                Description = "Elegant and stylish watches for women",
                CreatedAt = new DateTime(2026, 1, 1)
            },
            new Category
            {
                Id = 3,
                Name = "Couple Watches",
                Description = "Matching watch pairs for couples",
                CreatedAt = new DateTime(2026, 1, 1)
            }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "Citizen Eco-Drive",
                Price = 4500000,
                Discount = 10,
                StockQuantity = 20,
                CategoryId = 1,
                Description = "Citizen Eco-Drive men's watch, light-powered battery",
                ImageUrl = "https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80,https://images.unsplash.com/photo-1522312346375-d1a52e2b99b3?w=600&q=80,https://images.unsplash.com/photo-1614164185128-e4ec99c436d7?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1)
            },
            new Product
            {
                Id = 2,
                Name = "Seiko 5",
                Price = 3800000,
                Discount = 5,
                StockQuantity = 15,
                CategoryId = 1,
                Description = "Seiko 5 men's watch, automatic movement",
                ImageUrl = "https://images.unsplash.com/photo-1524592094714-0f0654e20314?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1524592094714-0f0654e20314?w=600&q=80,https://images.unsplash.com/photo-1539874754764-5a96559165b0?w=600&q=80,https://images.unsplash.com/photo-1434056886845-dac89ffe9b56?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 2, 1)
            },
            new Product
            {
                Id = 3,
                Name = "Casio Sheen",
                Price = 2900000,
                Discount = 0,
                StockQuantity = 25,
                CategoryId = 2,
                Description = "Casio Sheen women's watch, sapphire crystal",
                ImageUrl = "https://images.unsplash.com/photo-1539874754764-5a96559165b0?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1539874754764-5a96559165b0?w=600&q=80,https://images.unsplash.com/photo-1524592094714-0f0654e20314?w=600&q=80,https://images.unsplash.com/photo-1587836374828-4dbafa94cf0e?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 2, 15)
            },
            new Product
            {
                Id = 4,
                Name = "Olym Pianus Couple Set",
                Price = 8500000,
                Discount = 15,
                StockQuantity = 10,
                CategoryId = 3,
                Description = "Olym Pianus couple watch set, premium leather strap",
                ImageUrl = "https://images.unsplash.com/photo-1548171915-e79a380a2a4b?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1548171915-e79a380a2a4b?w=600&q=80,https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80,https://images.unsplash.com/photo-1524592094714-0f0654e20314?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 3, 1)
            },
            new Product
            {
                Id = 5,
                Name = "Tissot PRX",
                Price = 12500000,
                Discount = 0,
                StockQuantity = 12,
                CategoryId = 1,
                Description = "Tissot PRX men's watch, automatic, elegant design",
                ImageUrl = "https://images.unsplash.com/photo-1587836374828-4dbafa94cf0e?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1587836374828-4dbafa94cf0e?w=600&q=80,https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80,https://images.unsplash.com/photo-1508685096489-7aacd43bd3b1?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 3, 10)
            },
            new Product
            {
                Id = 6,
                Name = "Orient Bambino",
                Price = 5500000,
                Discount = 8,
                StockQuantity = 18,
                CategoryId = 1,
                Description = "Orient Bambino men's watch, sapphire crystal, leather strap",
                ImageUrl = "https://images.unsplash.com/photo-1508685096489-7aacd43bd3b1?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1508685096489-7aacd43bd3b1?w=600&q=80,https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80,https://images.unsplash.com/photo-1434056886845-dac89ffe9b56?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 3, 20)
            },
            new Product
            {
                Id = 7,
                Name = "Daniel Wellington",
                Price = 6200000,
                Discount = 20,
                StockQuantity = 30,
                CategoryId = 2,
                Description = "Daniel Wellington women's watch, minimalist design, NATO strap",
                ImageUrl = "https://images.unsplash.com/photo-1434056886845-dac89ffe9b56?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1434056886845-dac89ffe9b56?w=600&q=80,https://images.unsplash.com/photo-1539874754764-5a96559165b0?w=600&q=80,https://images.unsplash.com/photo-1524592094714-0f0654e20314?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 4, 1)
            },
            new Product
            {
                Id = 8,
                Name = "Longines Master Collection",
                Price = 22000000,
                Discount = 0,
                StockQuantity = 5,
                CategoryId = 1,
                Description = "Longines Master Collection, automatic chronograph",
                ImageUrl = "https://images.unsplash.com/photo-1587836374828-4dbafa94cf0e?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1587836374828-4dbafa94cf0e?w=600&q=80,https://images.unsplash.com/photo-1579586337278-3befd40fd17a?w=600&q=80,https://images.unsplash.com/photo-1508685096489-7aacd43bd3b1?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 4, 10)
            },
            new Product
            {
                Id = 9,
                Name = "Fossil Jacqueline",
                Price = 3200000,
                Discount = 25,
                StockQuantity = 35,
                CategoryId = 2,
                Description = "Fossil Jacqueline women's watch, elegant round dial",
                ImageUrl = "https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80,https://images.unsplash.com/photo-1539874754764-5a96559165b0?w=600&q=80,https://images.unsplash.com/photo-1434056886845-dac89ffe9b56?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 4, 15)
            },
            new Product
            {
                Id = 10,
                Name = "Seiko Couple Set",
                Price = 9800000,
                Discount = 10,
                StockQuantity = 8,
                CategoryId = 3,
                Description = "Seiko couple watch set, perfectly matching design",
                ImageUrl = "https://images.unsplash.com/photo-1548171915-e79a380a2a4b?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1548171915-e79a380a2a4b?w=600&q=80,https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80,https://images.unsplash.com/photo-1524592094714-0f0654e20314?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 4, 20)
            },
            new Product
            {
                Id = 11,
                Name = "Hamilton Khaki Field",
                Price = 16000000,
                Discount = 5,
                StockQuantity = 7,
                CategoryId = 1,
                Description = "Hamilton Khaki Field watch, automatic, 100m water resistant",
                ImageUrl = "https://images.unsplash.com/photo-1524592094714-0f0654e20314?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1524592094714-0f0654e20314?w=600&q=80,https://images.unsplash.com/photo-1587836374828-4dbafa94cf0e?w=600&q=80,https://images.unsplash.com/photo-1508685096489-7aacd43bd3b1?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 4, 25)
            },
            new Product
            {
                Id = 12,
                Name = "Skagen Signatur",
                Price = 2800000,
                Discount = 0,
                StockQuantity = 22,
                CategoryId = 2,
                Description = "Skagen Signatur women's watch, Danish minimalism",
                ImageUrl = "https://images.unsplash.com/photo-1539874754764-5a96559165b0?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1539874754764-5a96559165b0?w=600&q=80,https://images.unsplash.com/photo-1434056886845-dac89ffe9b56?w=600&q=80,https://images.unsplash.com/photo-1524592094714-0f0654e20314?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 5, 1)
            },
            new Product
            {
                Id = 13,
                Name = "Omega Seamaster",
                Price = 45000000,
                Discount = 0,
                StockQuantity = 3,
                CategoryId = 1,
                Description = "Omega Seamaster, iconic dive watch, sapphire crystal",
                ImageUrl = "https://images.unsplash.com/photo-1579586337278-3befd40fd17a?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1579586337278-3befd40fd17a?w=600&q=80,https://images.unsplash.com/photo-1587836374828-4dbafa94cf0e?w=600&q=80,https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 5, 2)
            },
            new Product
            {
                Id = 14,
                Name = "Citizen Couple Set",
                Price = 7200000,
                Discount = 15,
                StockQuantity = 14,
                CategoryId = 3,
                Description = "Citizen Eco-Drive couple watch set, light-powered",
                ImageUrl = "https://images.unsplash.com/photo-1548171915-e79a380a2a4b?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1548171915-e79a380a2a4b?w=600&q=80,https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80,https://images.unsplash.com/photo-1524592094714-0f0654e20314?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 5, 2)
            },
            new Product
            {
                Id = 15,
                Name = "Casio Edifice",
                Price = 4800000,
                Discount = 0,
                StockQuantity = 20,
                CategoryId = 1,
                Description = "Casio Edifice men's watch, Tough Solar, sapphire crystal",
                ImageUrl = "https://images.unsplash.com/photo-1508685096489-7aacd43bd3b1?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1508685096489-7aacd43bd3b1?w=600&q=80,https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80,https://images.unsplash.com/photo-1587836374828-4dbafa94cf0e?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 5, 2)
            },
            new Product
            {
                Id = 16,
                Name = "Cluse Minuit",
                Price = 2500000,
                Discount = 30,
                StockQuantity = 40,
                CategoryId = 2,
                Description = "Cluse Minuit women's watch, French elegance, interchangeable strap",
                ImageUrl = "https://images.unsplash.com/photo-1434056886845-dac89ffe9b56?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1434056886845-dac89ffe9b56?w=600&q=80,https://images.unsplash.com/photo-1539874754764-5a96559165b0?w=600&q=80,https://images.unsplash.com/photo-1524592094714-0f0654e20314?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 5, 2)
            },
            new Product
            {
                Id = 17,
                Name = "Tag Heuer Monaco",
                Price = 52000000,
                Discount = 0,
                StockQuantity = 4,
                CategoryId = 1,
                Description = "Tag Heuer Monaco, iconic square case, automatic chronograph",
                ImageUrl = "https://images.unsplash.com/photo-1619134778706-7015533a6150?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1619134778706-7015533a6150?w=600&q=80,https://images.unsplash.com/photo-1587836374828-4dbafa94cf0e?w=600&q=80,https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 5, 3)
            },
            new Product
            {
                Id = 18,
                Name = "Bulova Precisionist",
                Price = 7800000,
                Discount = 10,
                StockQuantity = 16,
                CategoryId = 1,
                Description = "Bulova Precisionist, ultra-precise quartz movement, sapphire crystal",
                ImageUrl = "https://images.unsplash.com/photo-1622434641406-a158123450f9?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1622434641406-a158123450f9?w=600&q=80,https://images.unsplash.com/photo-1524592094714-0f0654e20314?w=600&q=80,https://images.unsplash.com/photo-1508685096489-7aacd43bd3b1?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 5, 3)
            },
            new Product
            {
                Id = 19,
                Name = "Frederique Constant Classics",
                Price = 18500000,
                Discount = 5,
                StockQuantity = 6,
                CategoryId = 1,
                Description = "Frederique Constant Classics, elegant dress watch, automatic movement",
                ImageUrl = "https://images.unsplash.com/photo-1539874754764-5a96559165b0?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1539874754764-5a96559165b0?w=600&q=80,https://images.unsplash.com/photo-1587836374828-4dbafa94cf0e?w=600&q=80,https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 5, 3)
            },
            new Product
            {
                Id = 20,
                Name = "Baume et Mercier Classima",
                Price = 12800000,
                Discount = 0,
                StockQuantity = 9,
                CategoryId = 2,
                Description = "Baume et Mercier Classima, ladies elegant dress watch, quartz",
                ImageUrl = "https://images.unsplash.com/photo-1614164185128-e4ec99c436d7?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1614164185128-e4ec99c436d7?w=600&q=80,https://images.unsplash.com/photo-1539874754764-5a96559165b0?w=600&q=80,https://images.unsplash.com/photo-1524592094714-0f0654e20314?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 5, 3)
            },
            new Product
            {
                Id = 21,
                Name = "Tissot Chemises",
                Price = 5600000,
                Discount = 15,
                StockQuantity = 21,
                CategoryId = 2,
                Description = "Tissot Chemises, classic ladies watch, sapphire crystal",
                ImageUrl = "https://images.unsplash.com/photo-1522312346375-d1a52e2b99b3?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1522312346375-d1a52e2b99b3?w=600&q=80,https://images.unsplash.com/photo-1434056886845-dac89ffe9b56?w=600&q=80,https://images.unsplash.com/photo-1539874754764-5a96559165b0?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 5, 3)
            },
            new Product
            {
                Id = 22,
                Name = "Longines DolceVita",
                Price = 17000000,
                Discount = 0,
                StockQuantity = 7,
                CategoryId = 2,
                Description = "Longines DolceVita, ladies luxury watch, rectangular case, automatic",
                ImageUrl = "https://images.unsplash.com/photo-1619134778706-7015533a6150?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1619134778706-7015533a6150?w=600&q=80,https://images.unsplash.com/photo-1587836374828-4dbafa94cf0e?w=600&q=80,https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 5, 4)
            },
            new Product
            {
                Id = 23,
                Name = "Cartier Santos de Cartier",
                Price = 62000000,
                Discount = 0,
                StockQuantity = 2,
                CategoryId = 1,
                Description = "Cartier Santos de Cartier, legendary aviator watch, steel bracelet",
                ImageUrl = "https://images.unsplash.com/photo-1579586337278-3befd40fd17a?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1579586337278-3befd40fd17a?w=600&q=80,https://images.unsplash.com/photo-1587836374828-4dbafa94cf0e?w=600&q=80,https://images.unsplash.com/photo-1508685096489-7aacd43bd3b1?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 5, 4)
            },
            new Product
            {
                Id = 24,
                Name = "Seiko Prospex Turtle",
                Price = 8200000,
                Discount = 0,
                StockQuantity = 25,
                CategoryId = 1,
                Description = "Seiko Prospex Turtle, legendary dive watch, automatic, 200m water resistant",
                ImageUrl = "https://images.unsplash.com/photo-1508685096489-7aacd43bd3b1?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1508685096489-7aacd43bd3b1?w=600&q=80,https://images.unsplash.com/photo-1524592094714-0f0654e20314?w=600&q=80,https://images.unsplash.com/photo-1434056886845-dac89ffe9b56?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 5, 4)
            },
            new Product
            {
                Id = 25,
                Name = "Omega De Ville Prestige",
                Price = 38000000,
                Discount = 0,
                StockQuantity = 5,
                CategoryId = 2,
                Description = "Omega De Ville Prestige, ladies luxury watch, Co-Axial movement",
                ImageUrl = "https://images.unsplash.com/photo-1434056886845-dac89ffe9b56?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1434056886845-dac89ffe9b56?w=600&q=80,https://images.unsplash.com/photo-1539874754764-5a96559165b0?w=600&q=80,https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 5, 4)
            },
            new Product
            {
                Id = 26,
                Name = "Certina DS Action",
                Price = 9500000,
                Discount = 8,
                StockQuantity = 18,
                CategoryId = 1,
                Description = "Certina DS Action, sporty dive watch, ceramic bezel, automatic",
                ImageUrl = "https://images.unsplash.com/photo-1524592094714-0f0654e20314?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1524592094714-0f0654e20314?w=600&q=80,https://images.unsplash.com/photo-1587836374828-4dbafa94cf0e?w=600&q=80,https://images.unsplash.com/photo-1508685096489-7aacd43bd3b1?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 5, 5)
            },
            new Product
            {
                Id = 27,
                Name = "Hamilton Jazzmaster",
                Price = 14000000,
                Discount = 0,
                StockQuantity = 8,
                CategoryId = 1,
                Description = "Hamilton Jazzmaster, American spirit, automatic, exhibition caseback",
                ImageUrl = "https://images.unsplash.com/photo-1587836374828-4dbafa94cf0e?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1587836374828-4dbafa94cf0e?w=600&q=80,https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80,https://images.unsplash.com/photo-1579586337278-3befd40fd17a?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 5, 5)
            },
            new Product
            {
                Id = 28,
                Name = "Mido Baroncelli",
                Price = 11200000,
                Discount = 12,
                StockQuantity = 11,
                CategoryId = 1,
                Description = "Mido Baroncelli, elegant dress watch, automatic, slim profile",
                ImageUrl = "https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80,https://images.unsplash.com/photo-1508685096489-7aacd43bd3b1?w=600&q=80,https://images.unsplash.com/photo-1434056886845-dac89ffe9b56?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 5, 5)
            },
            new Product
            {
                Id = 29,
                Name = "Rado Captain Cook",
                Price = 24500000,
                Discount = 0,
                StockQuantity = 6,
                CategoryId = 1,
                Description = "Rado Captain Cook, iconic dive watch, ceramic case, automatic",
                ImageUrl = "https://images.unsplash.com/photo-1619134778706-7015533a6150?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1619134778706-7015533a6150?w=600&q=80,https://images.unsplash.com/photo-1579586337278-3befd40fd17a?w=600&q=80,https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 5, 5)
            },
            new Product
            {
                Id = 30,
                Name = "Oris Big Crown",
                Price = 29000000,
                Discount = 0,
                StockQuantity = 3,
                CategoryId = 1,
                Description = "Oris Big Crown Pointer Date, pilot watch, bronze case, automatic",
                ImageUrl = "https://images.unsplash.com/photo-1579586337278-3befd40fd17a?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1579586337278-3befd40fd17a?w=600&q=80,https://images.unsplash.com/photo-1587836374828-4dbafa94cf0e?w=600&q=80,https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 5, 6)
            },
            new Product
            {
                Id = 31,
                Name = "Gucci G-Timeless",
                Price = 18000000,
                Discount = 20,
                StockQuantity = 14,
                CategoryId = 2,
                Description = "Gucci G-Timeless, ladies luxury watch, interlocking G motif",
                ImageUrl = "https://images.unsplash.com/photo-1614164185128-e4ec99c436d7?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1614164185128-e4ec99c436d7?w=600&q=80,https://images.unsplash.com/photo-1434056886845-dac89ffe9b56?w=600&q=80,https://images.unsplash.com/photo-1522312346375-d1a52e2b99b3?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 5, 6)
            },
            new Product
            {
                Id = 32,
                Name = "Chopard Happy Sport",
                Price = 42000000,
                Discount = 0,
                StockQuantity = 4,
                CategoryId = 2,
                Description = "Chopard Happy Sport, ladies luxury watch, moving diamonds, steel bracelet",
                ImageUrl = "https://images.unsplash.com/photo-1522312346375-d1a52e2b99b3?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1522312346375-d1a52e2b99b3?w=600&q=80,https://images.unsplash.com/photo-1539874754764-5a96559165b0?w=600&q=80,https://images.unsplash.com/photo-1434056886845-dac89ffe9b56?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 5, 6)
            },
            new Product
            {
                Id = 33,
                Name = "Tissot Couple Gentleman",
                Price = 9200000,
                Discount = 10,
                StockQuantity = 15,
                CategoryId = 3,
                Description = "Tissot Gentleman couple set, matching elegant design, automatic",
                ImageUrl = "https://images.unsplash.com/photo-1548171915-e79a380a2a4b?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1548171915-e79a380a2a4b?w=600&q=80,https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80,https://images.unsplash.com/photo-1524592094714-0f0654e20314?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 5, 6)
            },
            new Product
            {
                Id = 34,
                Name = "Seiko Presage Couple",
                Price = 13400000,
                Discount = 5,
                StockQuantity = 12,
                CategoryId = 3,
                Description = "Seiko Presage couple set, Japanese craftsmanship, automatic, enamel dial",
                ImageUrl = "https://images.unsplash.com/photo-1548171915-e79a380a2a4b?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1548171915-e79a380a2a4b?w=600&q=80,https://images.unsplash.com/photo-1587836374828-4dbafa94cf0e?w=600&q=80,https://images.unsplash.com/photo-1539874754764-5a96559165b0?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 5, 7)
            },
            new Product
            {
                Id = 35,
                Name = "Citizen Eco-Drive Promaster",
                Price = 6800000,
                Discount = 0,
                StockQuantity = 19,
                CategoryId = 1,
                Description = "Citizen Eco-Drive Promaster, dive watch, light-powered, 200m water resistant",
                ImageUrl = "https://images.unsplash.com/photo-1524592094714-0f0654e20314?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1524592094714-0f0654e20314?w=600&q=80,https://images.unsplash.com/photo-1508685096489-7aacd43bd3b1?w=600&q=80,https://images.unsplash.com/photo-1434056886845-dac89ffe9b56?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 5, 7)
            },
            new Product
            {
                Id = 36,
                Name = "Longines Conquest Classic",
                Price = 19500000,
                Discount = 0,
                StockQuantity = 8,
                CategoryId = 1,
                Description = "Longines Conquest Classic, sporty elegance, automatic, 30 bar water resistant",
                ImageUrl = "https://images.unsplash.com/photo-1587836374828-4dbafa94cf0e?w=600&q=80",
                GalleryImages = "https://images.unsplash.com/photo-1587836374828-4dbafa94cf0e?w=600&q=80,https://images.unsplash.com/photo-1523170335258-f5ed11844a49?w=600&q=80,https://images.unsplash.com/photo-1579586337278-3befd40fd17a?w=600&q=80",
                IsActive = true,
                CreatedAt = new DateTime(2026, 5, 7)
            }
        );

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                FullName = "Admin User",
                Email = "admin@chronostore.com",
                PasswordHash = "$2a$11$OSNEsLOiiRlpR16p5REbQ.IJWmO/wEXf2nlnsCzv67erHGG0JTJcO",
                Phone = "0901234567",
                Address = "123 Admin Street, Ho Chi Minh City",
                Role = "Admin",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1)
            },
            new User
            {
                Id = 2,
                FullName = "Staff User",
                Email = "staff@chronostore.com",
                PasswordHash = "$2a$11$Lbx80/c219pWcGcSC7kOoukGOB5RCDmD5CiD0mEyQSxfeZhSVaJDq",
                Phone = "0902345678",
                Address = "456 Staff Street, Ha Noi",
                Role = "Staff",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1)
            },
            new User
            {
                Id = 3,
                FullName = "Customer User",
                Email = "customer@chronostore.com",
                PasswordHash = "$2a$11$7tLj7ZFD6MNpmBJcl/3LwOWDuNQ3STxtYRs/GejIF2jyeXFWxdYdm",
                Phone = "0903456789",
                Address = "789 Customer Street, Da Nang",
                Role = "Customer",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1)
            }
        );
    }
}
