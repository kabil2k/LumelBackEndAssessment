var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IInventoryService, InventoryService>();
builder.Services.AddHostedService<ReservationExpiryService>();

builder.Services.AddControllers(); 

var app = builder.Build();

app.MapControllers();

app.Run();