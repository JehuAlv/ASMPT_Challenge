namespace SmtOrderManager.Api.Requests;

public class UpdateBoardRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Length { get; set; }
    public double Width { get; set; }
    public List<int> ComponentIds { get; set; } = new();
}
