using Microsoft.extensions.Hosting;

public class ReservationExpiryService : BackgroundService
{
    private readonly IInventoryService _inventoryService;
    public ReservationExpiryService(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _inventoryService.ExpireReservations();
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken); // Check every 30 seconds
        }
    }
}