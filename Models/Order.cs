using Concrete.Api.Models;

namespace Backend.Models;

public class Order
{
    public int Id { get; set; }
    public string ProductName { get; set; } = "";
    public int Quantity { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}

