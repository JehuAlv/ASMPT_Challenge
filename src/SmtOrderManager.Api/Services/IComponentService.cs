using SmtOrderManager.Api.Entities;
using SmtOrderManager.Api.Requests;

namespace SmtOrderManager.Api.Services;

public interface IComponentService
{
    Task<List<Component>> GetAllAsync();
    Task<Component?> GetByIdAsync(int id);
    Task<List<Component>> SearchAsync(string name);
    Task<Component> CreateAsync(CreateComponentRequest request);
    Task<Component?> UpdateAsync(int id, UpdateComponentRequest request);
    Task<bool> DeleteAsync(int id);
}
