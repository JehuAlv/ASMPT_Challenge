using Microsoft.EntityFrameworkCore;
using SmtOrderManager.Api.Database;
using SmtOrderManager.Api.Entities;
using SmtOrderManager.Api.Requests;

namespace SmtOrderManager.Api.Services;

public class BoardService : IBoardService
{
    private readonly AppDbContext _context;
    private readonly ILogger<BoardService> _logger;

    public BoardService(AppDbContext context, ILogger<BoardService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Board>> GetAllAsync()
    {
        return await _context.Boards
            .Include(b => b.BoardComponents)
                .ThenInclude(bc => bc.Component)
            .ToListAsync();
    }

    public async Task<Board?> GetByIdAsync(int id)
    {
        return await _context.Boards
            .Include(b => b.BoardComponents)
                .ThenInclude(bc => bc.Component)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<List<Board>> SearchAsync(string name)
    {
        return await _context.Boards
            .Include(b => b.BoardComponents)
                .ThenInclude(bc => bc.Component)
            .Where(b => b.Name.Contains(name))
            .ToListAsync();
    }

    public async Task<Board> CreateAsync(CreateBoardRequest request)
    {
        var board = new Board
        {
            Name = request.Name,
            Description = request.Description,
            Length = request.Length,
            Width = request.Width
        };

        foreach (var componentId in request.ComponentIds)
        {
            board.BoardComponents.Add(new BoardComponent { ComponentId = componentId });
        }

        _context.Boards.Add(board);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Board created: {Name} (ID: {Id})", board.Name, board.Id);

        return board;
    }

    public async Task<Board?> UpdateAsync(int id, UpdateBoardRequest request)
    {
        var board = await _context.Boards
            .Include(b => b.BoardComponents)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (board == null) return null;

        board.Name = request.Name;
        board.Description = request.Description;
        board.Length = request.Length;
        board.Width = request.Width;

        board.BoardComponents.Clear();
        foreach (var componentId in request.ComponentIds)
        {
            board.BoardComponents.Add(new BoardComponent { ComponentId = componentId });
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("Board updated: {Name} (ID: {Id})", board.Name, board.Id);

        return board;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var board = await _context.Boards.FindAsync(id);
        if (board == null) return false;

        _context.Boards.Remove(board);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Board deleted: {Name} (ID: {Id})", board.Name, board.Id);

        return true;
    }
}
