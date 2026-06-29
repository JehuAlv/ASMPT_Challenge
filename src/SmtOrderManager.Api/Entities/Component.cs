namespace SmtOrderManager.Api.Entities;

public class Component
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }

    public List<Board> Boards { get; set; } = new();
}
