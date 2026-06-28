namespace SmtOrderManager.Api.Entities;

public class Order
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = "Created";

    public List<OrderBoard> OrderBoards { get; set; } = new();
}
