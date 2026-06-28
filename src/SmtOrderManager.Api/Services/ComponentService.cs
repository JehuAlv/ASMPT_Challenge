using Microsoft.EntityFrameworkCore;
using SmtOrderManager.Api.Database;
using SmtOrderManager.Api.Entities;
using SmtOrderManager.Api.Requests;

namespace SmtOrderManager.Api.Services;

public class ComponentService : IComponentService
{
    private readonly AppDbContext _context;
    private readonly ILogger<ComponentService> _logger;

    public ComponentService(AppDbContext context, ILogger<ComponentService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Component>> GetAllAsync()
    {
        return await _context.Components.ToListAsync();
    }

    public async Task<Component?> GetByIdAsync(int id)
    {
        return await _context.Components.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<Component>> SearchAsync(string name)
    {
        return await _context.Components
            .Where(c => c.Name.Contains(name))
            .ToListAsync();
    }

    public async Task<Component> CreateAsync(CreateComponentRequest request)
    {
        var component = new Component
        {
            Name = request.Name,
            Description = request.Description,
            Quantity = request.Quantity
        };

        _context.Components.Add(component);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Component created: {Name} (ID: {Id})", component.Name, component.Id);

        return component;
    }

    public async Task<Component?> UpdateAsync(int id, UpdateComponentRequest request)
    {
        var component = await _context.Components.FindAsync(id);
        if (component == null) return null;

        component.Name = request.Name;
        component.Description = request.Description;
        component.Quantity = request.Quantity;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Component updated: {Name} (ID: {Id})", component.Name, component.Id);

        return component;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var component = await _context.Components.FindAsync(id);
        if (component == null) return false;

        _context.Components.Remove(component);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Component deleted: {Name} (ID: {Id})", component.Name, component.Id);

        return true;
    }
}
