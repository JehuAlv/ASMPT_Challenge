using SmtOrderManager.Api.Entities;
using SmtOrderManager.Api.Requests;

namespace SmtOrderManager.Api.Services;

public interface IOrderService
{
    Task<List<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int id);
    Task<List<Order>> SearchAsync(string name);
    Task<Order> CreateAsync(CreateOrderRequest request);
    Task<Order?> UpdateAsync(int id, UpdateOrderRequest request);
    Task<bool> DeleteAsync(int id);
    Task<Order?> DownloadAsync(int id);
}
