namespace Service.Repository;
using Microsoft.EntityFrameworkCore;
using Service.Model;

public class CounterContext(DbContextOptions<CounterContext> options) : DbContext(options)
{
    public required DbSet<Counter> Counters { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 添加种子数据
        modelBuilder.Entity<Counter>().HasData(
            new Counter { Id = 1, Value = 0 }
        );
    }

}