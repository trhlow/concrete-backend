namespace Backend.Models; // hoặc Concrete.Api.Models (PHẢI KHỚP)

public class CreateOrderRequest
{
    public string ProductName { get; set; } = "";
    public int Quantity { get; set; }
}
