using Microsoft.AspNetCore.Mvc;
using SmtOrderManager.Api.Entities;
using SmtOrderManager.Api.Requests;
using SmtOrderManager.Api.Services;

namespace SmtOrderManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComponentsController : ControllerBase
{
    private readonly IComponentService _componentService;

    public ComponentsController(IComponentService componentService)
    {
        _componentService = componentService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Component>>> GetAll()
    {
        return await _componentService.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Component>> GetById(int id)
    {
        var component = await _componentService.GetByIdAsync(id);
        if (component == null) return NotFound();
        return component;
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<Component>>> Search([FromQuery] string name)
    {
        return await _componentService.SearchAsync(name);
    }

    [HttpPost]
    public async Task<ActionResult<Component>> Create(CreateComponentRequest request)
    {
        var component = await _componentService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = component.Id }, component);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Component>> Update(int id, UpdateComponentRequest request)
    {
        var component = await _componentService.UpdateAsync(id, request);
        if (component == null) return NotFound();
        return component;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _componentService.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}
