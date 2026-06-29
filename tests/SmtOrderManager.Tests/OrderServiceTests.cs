using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SmtOrderManager.Api.Database;
using SmtOrderManager.Api.Entities;
using SmtOrderManager.Api.Requests;
using SmtOrderManager.Api.Services;

namespace SmtOrderManager.Tests;

public class OrderServiceTests
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);

        context.Boards.Add(new Board
        {
            Id = 1,
            Name = "Test Board",
            Description = "Board for testing",
            Length = 100,
            Width = 50
        });
        context.SaveChanges();

        return context;
    }

    private OrderService CreateService(AppDbContext context)
    {
        var logger = new Mock<ILogger<OrderService>>();
        return new OrderService(context, logger.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateOrder()
    {
        var context = CreateContext();
        var service = CreateService(context);

        var result = await service.CreateAsync(new CreateOrderRequest
        {
            Name = "ORD-001",
            Description = "Test order",
            OrderDate = new DateTime(2026, 6, 28),
            BoardIds = new List<int> { 1 }
        });

        Assert.Equal("ORD-001", result.Name);
        Assert.Equal("Created", result.Status);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllOrders()
    {
        var context = CreateContext();
        var service = CreateService(context);

        await service.CreateAsync(new CreateOrderRequest { Name = "Order A", Description = "First", OrderDate = DateTime.Now });
        await service.CreateAsync(new CreateOrderRequest { Name = "Order B", Description = "Second", OrderDate = DateTime.Now });

        var result = await service.GetAllAsync();

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        var context = CreateContext();
        var service = CreateService(context);

        var result = await service.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveOrder()
    {
        var context = CreateContext();
        var service = CreateService(context);

        var order = await service.CreateAsync(new CreateOrderRequest { Name = "To Delete", Description = "Remove me", OrderDate = DateTime.Now });

        var deleted = await service.DeleteAsync(order.Id);
        var found = await service.GetByIdAsync(order.Id);

        Assert.True(deleted);
        Assert.Null(found);
    }

    [Fact]
    public async Task DownloadAsync_ShouldChangeStatusToDownloaded()
    {
        var context = CreateContext();
        var service = CreateService(context);

        var order = await service.CreateAsync(new CreateOrderRequest
        {
            Name = "ORD-DL-001",
            Description = "Download test",
            OrderDate = DateTime.Now,
            BoardIds = new List<int> { 1 }
        });

        var result = await service.DownloadAsync(order.Id);

        Assert.NotNull(result);
        Assert.Equal("Downloaded", result.Status);
    }

    [Fact]
    public async Task SearchAsync_ShouldFilterByName()
    {
        var context = CreateContext();
        var service = CreateService(context);

        await service.CreateAsync(new CreateOrderRequest { Name = "Alpha Order", Description = "First", OrderDate = DateTime.Now });
        await service.CreateAsync(new CreateOrderRequest { Name = "Beta Order", Description = "Second", OrderDate = DateTime.Now });

        var result = await service.SearchAsync("Alpha");

        Assert.Single(result);
        Assert.Equal("Alpha Order", result[0].Name);
    }
}
