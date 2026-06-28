namespace SmtOrderManager.Api.Entities;

public class BoardComponent
{
    public int BoardId { get; set; }
    public Board Board { get; set; } = null!;

    public int ComponentId { get; set; }
    public Component Component { get; set; } = null!;
}
