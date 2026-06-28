using Microsoft.AspNetCore.Mvc;
using SmtOrderManager.Api.Entities;
using SmtOrderManager.Api.Requests;
using SmtOrderManager.Api.Services;

namespace SmtOrderManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Order>>> GetAll()
    {
        return await _orderService.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetById(int id)
    {
        var order = await _orderService.GetByIdAsync(id);
        if (order == null) return NotFound();
        return order;
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<Order>>> Search([FromQuery] string name)
    {
        return await _orderService.SearchAsync(name);
    }

    [HttpPost]
    public async Task<ActionResult<Order>> Create(CreateOrderRequest request)
    {
        var order = await _orderService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Order>> Update(int id, UpdateOrderRequest request)
    {
        var order = await _orderService.UpdateAsync(id, request);
        if (order == null) return NotFound();
        return order;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _orderService.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpPost("{id}/download")]
    public async Task<ActionResult<Order>> Download(int id)
    {
        var order = await _orderService.DownloadAsync(id);
        if (order == null) return NotFound();
        return order;
    }
}
