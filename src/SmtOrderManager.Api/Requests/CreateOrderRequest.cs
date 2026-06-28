namespace SmtOrderManager.Api.Requests;

public class CreateOrderRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public List<int> BoardIds { get; set; } = new();
}
