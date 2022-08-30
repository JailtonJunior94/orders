using Orders.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Orders.API.Controllers;

[Route("v1/orders")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _service;

    public OrderController(IOrderService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Post()
    {
        await _service.CreateOrderAsync();
        return NoContent();
    }
}