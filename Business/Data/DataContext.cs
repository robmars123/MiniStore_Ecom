namespace Business.Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DataContext : DbContext
    {

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Subcategory> Subcategories { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<ProductImage> ProductImages { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Product>()
            //    .Property(e => e.Price)
            //    .HasPrecision(18, 0);
            modelBuilder.Entity<Product>()
                .HasKey(k => k.Product_Id);

            //modelBuilder.Entity<Cart>()
            //    .Property(k => k.Product_Id);

            modelBuilder.Entity<Product>()
                .Property(b => b.Product_Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            //modelBuilder.Entity<ProductImage>()
            //.HasRequired(c => c.product)
            //.WithMany(i=>i.productImages)
            //.Map(m => m.MapKey("Product_Id"));

            //modelBuilder.Entity<Product>()
            //.HasRequired(c => c.cart)
            //.WithMany(i => i.products)
            //.Map(m => m.MapKey("Product_Id"));

        }
    }
}
