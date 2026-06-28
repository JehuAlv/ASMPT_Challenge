using SmtOrderManager.Api.Entities;
using SmtOrderManager.Api.Requests;

namespace SmtOrderManager.Api.Services;

public interface IBoardService
{
    Task<List<Board>> GetAllAsync();
    Task<Board?> GetByIdAsync(int id);
    Task<List<Board>> SearchAsync(string name);
    Task<Board> CreateAsync(CreateBoardRequest request);
    Task<Board?> UpdateAsync(int id, UpdateBoardRequest request);
    Task<bool> DeleteAsync(int id);
}
