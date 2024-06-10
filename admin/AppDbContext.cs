using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using webapi.model.po;
using webapi.util;

namespace webapi;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<ResourcePo> Resource { set; get; } = null!;
    public DbSet<UserPo> User { set; get; } = null!;
    public DbSet<SceneryPo> scenery { set; get; } = null!;
    public DbSet<CommentPo> comment { set; get; } = null!;
    public DbSet<PropertiesPo> properties { set; get; } = null!;
    public DbSet<OrderPo> order { set; get; } = null!;
    public DbSet<StrategyPo> strategy { set; get; } = null!;
    public DbSet<LikePo> like { set; get; } = null!;

    private static readonly IHashPassword HashPassword = new Sha512HashPassword();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserPo>().HasData(new UserPo()
        {
            Id = 1,
            userNo = "U0001",
            name = "admin",
            aliasName = "admin",
            sex = "1",
            birthDate = "2000-01-01",
            phone = "14878789090",
            email = "admin@gmail.com",
            address = "北京朝阳区热心市民",
            balance = 0,
            isAdmin = true,
            passwd = HashPassword.Generate(HashPassword.GenerateSalt(), "admin")
        });

        modelBuilder.Entity<PropertiesPo>().HasData(new PropertiesPo()
        {
            Id = 1,
            name = "hotCity",
            value = "北京;上海;广州;深圳"
        });

        modelBuilder.Entity<UserPo>().Property(e => e.CreateTime).ValueGeneratedOnAdd()
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        modelBuilder.Entity<UserPo>().Property(e => e.ModifyTime).ValueGeneratedOnAddOrUpdate()
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

        modelBuilder.Entity<OrderPo>().Property(e => e.CreateTime).ValueGeneratedOnAdd()
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        modelBuilder.Entity<OrderPo>().Property(e => e.ModifyTime).ValueGeneratedOnAddOrUpdate()
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");


        modelBuilder.Entity<ResourcePo>().Property(e => e.CreateTime).ValueGeneratedOnAdd()
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        modelBuilder.Entity<ResourcePo>().Property(e => e.ModifyTime).ValueGeneratedOnAddOrUpdate()
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

        modelBuilder.Entity<CommentPo>().Property(e => e.CreateTime).ValueGeneratedOnAdd()
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        modelBuilder.Entity<CommentPo>().Property(e => e.ModifyTime).ValueGeneratedOnAddOrUpdate()
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        
        modelBuilder.Entity<StrategyPo>().Property(e => e.CreateTime).ValueGeneratedOnAdd()
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        modelBuilder.Entity<StrategyPo>().Property(e => e.ModifyTime).ValueGeneratedOnAddOrUpdate()
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

        modelBuilder.Entity<PropertiesPo>().Property(e => e.CreateTime).ValueGeneratedOnAdd()
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        modelBuilder.Entity<PropertiesPo>().Property(e => e.ModifyTime).ValueGeneratedOnAddOrUpdate()
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

        modelBuilder.Entity<LikePo>().Property(e => e.CreateTime).ValueGeneratedOnAdd()
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        modelBuilder.Entity<LikePo>().Property(e => e.ModifyTime).ValueGeneratedOnAddOrUpdate()
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        
        var productConverter = new ValueConverter<List<ProductPo>, string>(v => JsonConvert.SerializeObject(v),
            v => JsonConvert.DeserializeObject<List<ProductPo>>(v)!);
        var imageConvert = new ValueConverter<List<ImagePo>, string>(v => JsonConvert.SerializeObject(v),
            v => JsonConvert.DeserializeObject<List<ImagePo>>(v)!);

        modelBuilder.Entity<SceneryPo>().Property(e => e.product).HasConversion(productConverter!);
        modelBuilder.Entity<SceneryPo>().Property(e => e.imageList).HasConversion(imageConvert!);
        modelBuilder.Entity<SceneryPo>().Property(e => e.CreateTime).ValueGeneratedOnAdd()
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
        modelBuilder.Entity<SceneryPo>().Property(e => e.ModifyTime).ValueGeneratedOnAddOrUpdate()
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
    }
}