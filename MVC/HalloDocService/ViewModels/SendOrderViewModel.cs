
using System.ComponentModel.DataAnnotations;

namespace HalloDocService.ViewModels
{
    public class SendOrderViewModel
    {
        
        public int ReqId { get; set; }

        [Required(ErrorMessage = "Profession is Required")]
        public int ProfessionId {get; set;}

        [Required(ErrorMessage = "Business is Required")]
        public int BusinessId {get; set;}

        [Required(ErrorMessage = "Business Contact is Required")]
        public string? BusinessContact { get; set; }

        [Required(ErrorMessage = "Business Mail is Required")]
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