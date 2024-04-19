using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HalloDocRepository.DataModels;
public class DashboardViewModel
{
    public IEnumerable<Request>? UserRequests { get; set; }
    public Func<int, string>? GetUserRequestType { get; set; }
}