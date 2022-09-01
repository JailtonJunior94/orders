using Orders.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Orders.Core.Requests;

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
    public async Task<IActionResult> Post([FromBody] Order order)
    {
        await _service.CreateOrderAsync(order);
        return NoContent();
    }
}