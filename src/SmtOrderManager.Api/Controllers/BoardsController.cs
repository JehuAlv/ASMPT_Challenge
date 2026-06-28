using Microsoft.AspNetCore.Mvc;
using SmtOrderManager.Api.Entities;
using SmtOrderManager.Api.Requests;
using SmtOrderManager.Api.Services;

namespace SmtOrderManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BoardsController : ControllerBase
{
    private readonly IBoardService _boardService;

    public BoardsController(IBoardService boardService)
    {
        _boardService = boardService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Board>>> GetAll()
    {
        return await _boardService.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Board>> GetById(int id)
    {
        var board = await _boardService.GetByIdAsync(id);
        if (board == null) return NotFound();
        return board;
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<Board>>> Search([FromQuery] string name)
    {
        return await _boardService.SearchAsync(name);
    }

    [HttpPost]
    public async Task<ActionResult<Board>> Create(CreateBoardRequest request)
    {
        var board = await _boardService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = board.Id }, board);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Board>> Update(int id, UpdateBoardRequest request)
    {
        var board = await _boardService.UpdateAsync(id, request);
        if (board == null) return NotFound();
        return board;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _boardService.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}
