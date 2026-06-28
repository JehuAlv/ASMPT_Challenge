namespace SmtOrderManager.Api.Requests;

public class CreateComponentRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
}
