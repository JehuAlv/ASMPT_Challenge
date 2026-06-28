using Microsoft.EntityFrameworkCore;
using SmtOrderManager.Api.Database;
using SmtOrderManager.Api.Entities;
using SmtOrderManager.Api.Requests;

namespace SmtOrderManager.Api.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;
    private readonly ILogger<OrderService> _logger;

    public OrderService(AppDbContext context, ILogger<OrderService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Order>> GetAllAsync()
    {
        return await _context.Orders
            .Include(o => o.OrderBoards)
                .ThenInclude(ob => ob.Board)
            .ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.OrderBoards)
                .ThenInclude(ob => ob.Board)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<List<Order>> SearchAsync(string name)
    {
        return await _context.Orders
            .Include(o => o.OrderBoards)
                .ThenInclude(ob => ob.Board)
            .Where(o => o.Name.Contains(name))
            .ToListAsync();
    }

    public async Task<Order> CreateAsync(CreateOrderRequest request)
    {
        var order = new Order
        {
            Name = request.Name,
            Description = request.Description,
            OrderDate = request.OrderDate,
            Status = "Created"
        };

        foreach (var boardId in request.BoardIds)
        {
            order.OrderBoards.Add(new OrderBoard { BoardId = boardId });
        }

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Order created: {Name} (ID: {Id})", order.Name, order.Id);

        return order;
    }

    public async Task<Order?> UpdateAsync(int id, UpdateOrderRequest request)
    {
        var order = await _context.Orders
            .Include(o => o.OrderBoards)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) return null;

        order.Name = request.Name;
        order.Description = request.Description;
        order.OrderDate = request.OrderDate;

        order.OrderBoards.Clear();
        foreach (var boardId in request.BoardIds)
        {
            order.OrderBoards.Add(new OrderBoard { BoardId = boardId });
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("Order updated: {Name} (ID: {Id})", order.Name, order.Id);

        return order;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null) return false;

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Order deleted: {Name} (ID: {Id})", order.Name, order.Id);

        return true;
    }

    public async Task<Order?> DownloadAsync(int id)
    {
        var order = await _context.Orders
            .Include(o => o.OrderBoards)
                .ThenInclude(ob => ob.Board)
                    .ThenInclude(b => b.BoardComponents)
                        .ThenInclude(bc => bc.Component)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) return null;

        order.Status = "Downloaded";
        await _context.SaveChangesAsync();

        _logger.LogInformation("Order downloaded to production: {Name} (ID: {Id})", order.Name, order.Id);

        return order;
    }
}
