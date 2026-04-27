using system.collections.Concurrent;    

public class InventoryService : IInventoryService
{ 
    private int _availableInventory = 0;

    private readonly ConcurrentDictionary<string, Reservation> _reservations = new();
    private readonly Queue<string> _waitlist = new();

    private readonly object _lock = new();

    public void Initialize(int count)
    {         
        lock (_lock)
        {
            _availableInventory = count;
            _reservations.Clear();
            _waitlist.Clear();
        }
    }

    public object Reserve(string userId)
    {
        lock (_lock)
        {
            if (_availableInventory > 0)
            {
                _availableInventory--;
                var reservation = new Reservation
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(5) // Reservation expires in 5 minutes
                };
                _reservations[reservation.Id] = reservation;
                return new
                {
                    status = 201,
                    reservationId = reservation.Id,
                    expires_at = reservation.ExpiresAt
                };
            }
            else
            {
                _waitlist.Enqueue(userId);

                return new
                {
                    status = 202,
                    message = "Added to waitlist"
                    position = _waitlist.Count
                };
            }
        }
    }

    public object GetSatus()
    {
        lock (_lock)
        {
            return new
            {
                availableInventory = _availableInventory,
                activeReservations = _reservations.Count,
                waitlistCount = _waitlist.Count
            };
        }
    }

    public void cleanupExpired()
    {
        lock (_lock)    
        {
            var expired = _reservations.Where(r => r.Value.ExpiresAt <= DateTime.UtcNow).Select(r => r.Key).ToList();

            foreach (var key in expired)
            {
                _reservations.TryRemove(key, out _);
                _availableInventory++;
                
                if (_waitlist.Count > 0)
                {
                    var nextUserId = _waitlist.Dequeue();

                    _availableInventory--;

                    var newReservation = new Reservation
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = nextUserId,
                        ExpiresAt = DateTime.UtcNow.AddMinutes(5)
                    };
                    _reservations[newReservation.Id] = newReservation;
                }
            }
        }