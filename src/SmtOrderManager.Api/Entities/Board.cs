namespace SmtOrderManager.Api.Entities;

public class Board
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Length { get; set; }
    public double Width { get; set; }

    public List<OrderBoard> OrderBoards { get; set; } = new();
    public List<BoardComponent> BoardComponents { get; set; } = new();
}
