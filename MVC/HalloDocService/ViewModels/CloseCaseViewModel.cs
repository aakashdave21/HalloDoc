using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HalloDocService.ViewModels
{
    public class CloseCaseViewModel
    {
        public int ReqId { get; set; }
        public int PatientId { get; set; }
        public string? PatientName { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public IEnumerable<ViewDocuments>? documentList {get; set;}
    }

}