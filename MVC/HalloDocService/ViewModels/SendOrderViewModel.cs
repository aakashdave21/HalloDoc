
namespace HalloDocService.ViewModels
{
    public class SendOrderViewModel
    {
        public int ReqId { get; set; }
        public int ProfessionId {get; set;}
        public int BusinessId {get; set;}
        public string? BusinessContact { get; set; }
        public string? BusinessEmail { get; set; }
        public string? FaxNumber { get; set; }
        public string? Prescription { get; set; }
        public int NoOfRefill { get; set; }
        
        public IEnumerable<ProfessionList>? ProfessionLists {get; set;}
    }

    public class ProfessionList
    {
        public int ProfessionId {get; set;}
        public string? ProfessionName {get; set;}
    }

    public class BusinessList
    {
        public int BusinessId { get; set; }
        public string? BusinessName { get; set; }
    }

}