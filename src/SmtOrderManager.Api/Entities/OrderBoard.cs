namespace SmtOrderManager.Api.Entities;

public class OrderBoard
{
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public int BoardId { get; set; }
    public Board Board { get; set; } = null!;
}
