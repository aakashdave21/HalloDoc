namespace HalloDocRepository.CustomModels;

public class DashboardRequestQuery
{
    public string? SearchBy { get; set; } = null;
    public int RequestTypeId { get; set; } = 0;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 5;
    public int Region { get; set; } = 0;
    public string? Status { get; set; } = "new";
}
