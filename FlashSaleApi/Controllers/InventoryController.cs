[ApiController]
[Route("")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;
    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }
    [HttpPost("inventory/init")]
    public IActionResult InitializeInventory([FromBody] InitializeInventoryRequest request)
    {
        _inventoryService.Initialize(request.Count);
        return Ok();
    }

    [HttpPost("reserve")]
    public IActionResult Reserve([FromBody] ReserveRequest request)
    {
        var result = _inventoryService.Reserve(request.UserId);
        return StatusCode(result.status, result);
    }

    [HttpGet("status")]
    public IActionResult GetStatus()
    {
        var status = _inventoryService.GetStatus();
        return Ok(status);
    }
}
    