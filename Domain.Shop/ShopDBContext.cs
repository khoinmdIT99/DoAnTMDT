using Domain.Shop.Entities;
using Domain.Shop.IRepositories;
using Domain.Shop.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Domain.Shop;
using Domain.Shop.Entities.SystemManage;


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
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<ShippingAddress> ShippingAddress { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<ImportBill> ImportBills { get; set; }
        public DbSet<ImportBillDetail> ImportBillDetails { get; set; }
        public DbSet<ForgetPassword> ForgetPasswords { get; set; }
        public DbSet<SystemInformation> SystemInformations { get; set; }
        public DbSet<ThongBao> ThongBaos { get; set; }
        public DbSet<TranhChap> TranhChaps { get; set; }
        public DbSet<TinNhan> TinNhans{ get; set; }
        public DbSet<PhanQuyen> PhanQuyens { get; set; }
        public DbSet<Quyen> Quyens { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<XacMinh> XacMinhs { get; set; }
        public DbSet<DanhGia> DanhGias { get; set; }
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
            services.AddScoped<ISliderRepository, SliderRepository>();
            services.AddScoped<ICustomerRepository,CustomerRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<ISystemInformationRepository, SystemInformationRepository>();
            services.AddScoped<IForgetPasswordRepository, ForgetPasswordRepository>();
            services.AddScoped<IImportRepository, ImportRepository>();
            services.AddScoped<IImportDetailRepository, ImportDetailRepository>();
            services.AddScoped<IThongBaoRepository, ThongBaoRepository>();
            services.AddScoped<ITinNhanRepository, TinNhanRepository>();
            services.AddScoped<ITranhChapRepository, TranhChapRepository>();
            services.AddScoped<IUtils, Utils>();
            services.AddScoped<IVnPayLibrary, VnPayLibrary>();
            services.AddScoped<IQuyenRepository, QuyenRepository>();
            services.AddScoped<IPhanQuyenRepository, PhanQuyenRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IXacMinhRepository, XacMinhRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IDanhGiaRepository, DanhGiaRepository>();
            services.AddScoped<IDiemTichLuyRepository, DiemTichLuyRepository>();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Seed();
            modelBuilder.Entity<Message>().ToTable("Messages");

            modelBuilder.Entity<Message>().Property(s => s.Content).IsRequired().HasMaxLength(500);

            modelBuilder.Entity<Message>()
                .HasOne(s => s.ToRoom)
                .WithMany(m => m.Messages)
                .HasForeignKey(s => s.ToRoomId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Room>().ToTable("Rooms");

            modelBuilder.Entity<Room>().Property(s => s.Name).IsRequired().HasMaxLength(100);

            modelBuilder.Entity<Room>()
                .HasOne(s => s.Admin)
                .WithMany(u => u.Rooms)
                .IsRequired();
            modelBuilder.Entity<PhanQuyen>()
                .HasKey(c => new { c.MaQuyen, c.MaTaiKhoan });
            modelBuilder.Entity<ProductType>()
                .HasMany(c => c.Products)
                .WithOne(c => c.ProductType).IsRequired()
                .HasForeignKey(c => c.ProductTypeId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Material>()
                .HasMany(c => c.Products)
                .WithOne(c => c.Material).IsRequired()
                .HasForeignKey(c => c.MaterialId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(c => c.Category).IsRequired()
                .HasForeignKey(c => c.CategoryId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Product>()
                .HasMany(e => e.ProductImages).WithOne(e => e.Product).HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Product>().HasMany(x => x.ProductTags).WithOne(x => x.Product).HasForeignKey(x => x.ProductId);
            modelBuilder.Entity<ImportBill>()
                .HasMany<ImportBillDetail>(ib => ib.DetailImports)
                .WithOne(ibd => ibd.ImportBill)
                .HasForeignKey(ibd => ibd.IdImport)
                .OnDelete(DeleteBehavior.Cascade);
            // Cart
            modelBuilder.Entity<Cart>()
                .HasMany(p => p.Products)
                .WithOne(p => p.Cart)
                .OnDelete(DeleteBehavior.Cascade);
            // CartProduct
            modelBuilder.Entity<CartProduct>()
                .HasKey(cp => new { cp.CartId, cp.ProductId });
            modelBuilder.Entity<CartProduct>()
                .HasOne(cp => cp.Cart)
                .WithMany(c => c.Products)
                .HasForeignKey(cp => cp.CartId)
                .IsRequired();
            modelBuilder.Entity<CartProduct>()
                .HasOne(cp => cp.Product)
                .WithMany(p => p.Carts)
                .HasForeignKey(cp => cp.ProductId)
                .IsRequired();
            // Category
            modelBuilder.Entity<Category>()
                .Property(a => a.CategoryName)
                .IsRequired();
            // ProductType
            modelBuilder.Entity<ProductType>()
                .Property(a => a.TypeName)
                .IsRequired();
            modelBuilder.Entity<CartProduct>(entity =>
            {
                entity.Property(e => e.NgayGiao).HasColumnType("datetime");

                entity.Property(e => e.TinhTrangChiTiet).HasMaxLength(20);
            });
            modelBuilder.Entity<DanhGia>(entity =>
            {
                entity.HasOne(d => d.IdTaiKhoanDanhGiaNavigation)
                    .WithMany(p => p.DanhGiaIdTaiKhoanDanhGiaNavigation)
                    .HasForeignKey(d => d.IdTaiKhoanDanhGia)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
            modelBuilder.Entity<TranhChap>(entity =>
            {
                entity.HasOne(d => d.DonDatHang)
                    .WithMany(p => p.ListTranhChaps)
                    .HasForeignKey(d => d.MaDDH)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasIndex(e => e.Id)
                    .HasName("UQ__DonHang__129584AC539A1D8D")
                    .IsUnique();

                entity.Property(e => e.CreateAt).HasColumnType("datetime");

                entity.Property(e => e.Status).HasMaxLength(100);

                entity.Property(e => e.TinhTrangDanhGiaCustomer).HasMaxLength(20);
            });
        }
    }

}
