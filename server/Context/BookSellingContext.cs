using Microsoft.EntityFrameworkCore;
using server.Models.DataBase;

namespace server.Context;

public partial class BookSellingContext : DbContext
{
    public BookSellingContext()
    {
    }

    public BookSellingContext(DbContextOptions<BookSellingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Genre> Genries { get; set; } = null!;
    public virtual DbSet<Book> Books { get; set; } = null!;
    public virtual DbSet<BookGenre> BooksGenries { get; set; } = null!;
    public virtual DbSet<Seller> Sellers { get; set; } = null!;
    public virtual DbSet<Sale> Sales { get; set; } = null!;

    public virtual DbSet<SaleBook> SalesBooks { get; set; } = null!;

    public virtual DbSet<User> Users { get; set; } = null!;
    public virtual DbSet<Role> Roles { get; set; } = null!;
    public virtual DbSet<UserRole> UsersRoles { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_GENRIES");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .IsUnicode(false)
                .HasColumnName("name");
        });
        
        modelBuilder.Entity<BookGenre>(entity =>
        {
            entity.ToTable("Books_Genries");

            entity.HasKey(e => new { e.BookId, e.GenreId });

            entity
                .HasOne(e => e.Book)
                .WithMany(e => e.BooksGenres)
                .HasForeignKey(e => e.BookId)
                .HasConstraintName("book_id");
        });
        
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_BOOKS");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Author)
                .IsUnicode(false)
                .HasColumnName("author");
            entity.Property(e => e.Name)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.StoreCount).HasColumnName("store_count");
            entity.Property(e => e.Year)
                .HasColumnType("numeric(4, 0)")
                .HasColumnName("year");
            entity.Property(e => e.Image)
                .IsUnicode(false)
                .HasColumnName("image");
            entity.Property(e => e.Active)
                .HasColumnType("bit")
                .HasColumnName("active");
        });

        modelBuilder.Entity<Seller>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_SELLERS");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Fname)
                .IsUnicode(false)
                .HasColumnName("fname");
            entity.Property(e => e.Name)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Surname)
                .IsUnicode(false)
                .HasColumnName("surname");
            entity.Property(e => e.PhoneNumber)
                .HasColumnType("numeric(11, 0)")
                .HasColumnName("phone_number");
            entity.Property(e => e.Active)
                .HasColumnType("bit")
                .HasColumnName("active");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_SALES");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date)
                .HasColumnType("date")
                .HasColumnName("date");
            entity.Property(e => e.SellerId).HasColumnName("seller_id");

            entity.HasOne(d => d.Seller)
                .WithMany(p => p.Sales)
                .HasForeignKey(d => d.SellerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Sales_fk1");
        });

        modelBuilder.Entity<SaleBook>(entity =>
        {
            entity.ToTable("Sales_Books");

            entity.HasKey(e => new { e.SaleId, e.BookId });
            
            entity
                .HasOne(e => e.Sale)
                .WithMany(e => e.SalesBooks)
                .HasForeignKey(e => e.SaleId)
                .HasConstraintName("sb_sale_id");

            entity
                .Property(e => e.SoldCount)
                .HasColumnName("sold_count");
        });
        
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3213E83FA5C138C2");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("name");
        });
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3213E83F8874F9F2");

            entity.Property(e => e.Id).HasColumnName("id");
            
            entity.Property(e => e.PasswordHash)
                .IsUnicode(false)
                .HasColumnName("password_hash");
            
            entity.Property(e => e.PasswordKey)
                .IsUnicode(false)
                .HasColumnName("password_key");
            
            entity.Property(e => e.Username)
                .IsUnicode(false)
                .HasColumnName("username");
        });
        
        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("Users_Roles");

            entity.HasKey(e => new { e.UserId, e.RoleId });

            entity
                .HasOne(e => e.User)
                .WithMany(e => e.UsersRoles)
                .HasForeignKey(e => e.UserId)
                .HasConstraintName("user_id");
            
            entity
                .HasOne(e => e.Role)
                .WithMany(e => e.UsersRoles)
                .HasForeignKey(e => e.RoleId)
                .HasConstraintName("role_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
