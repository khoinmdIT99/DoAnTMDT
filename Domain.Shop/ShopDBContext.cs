using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Domain.Shop.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Domain.Shop;


namespace Shop.Application
{
    public class ShopDBContext: DbContext
    {
        public ShopDBContext(DbContextOptions<ShopDBContext> options) : base(options) { }
        public ShopDBContext() { }
        public DbSet<BlogCategory> BlogCategories { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogImage> BlogImages { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartProduct> CartProducts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerFeedback> CustomerFeedbacks { get; set; }
        public DbSet<CustomerFeedbackImage> CustomerFeedbackImages { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<ShopAddress> ShopAddress { get; set; }
        public DbSet<ShopSetting> ShopSetting { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ShippingAddress> ShippingAddress { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<District> Districts { get; set; }

        public static void ConfigureServices(IServiceCollection services)
		{
            services.AddScoped<IUnitOfWork<ShopDBContext>, ShopUnitOfWork>();
            services.AddScoped<IMaterialRepository, MaterialRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IMailerRepository, MailerRepository>();
            services.AddScoped<IProductTypeRepository, ProductTypeRepository>();
            services.AddScoped<IBlogCategoryRepository, BlogCategoryRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddScoped<IShopSettingRepository, ShopSettingRepository>();
            services.AddScoped<IShopAddressRepository, ShopAddressRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductImageRepository, ProductImageRepository>();
            services.AddScoped<IProductTagRepository, ProductTagRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IProvinceRepository, ProvinceRepository>();
            services.AddScoped<IDictrictRepository, DictrictRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
            services.AddScoped<IProductReViewRepository, ProductReViewRepository>();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();
        }
    }

}
