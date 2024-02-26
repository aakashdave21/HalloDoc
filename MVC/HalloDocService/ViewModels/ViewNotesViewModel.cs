using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HalloDocService.ViewModels
{
    public class ViewNotesViewModel
    {
        public int Id { get; set; }
        public int NoteId { get; set; }
        public int ReqId { get; set; }
        public string? PhysicianNote { get; set; } = null!;
        public string? AdminNote { get; set; } = null!;
        public string? PatientNote { get; set; } = null!;
        public string? AdminCancelNote { get; set; } = null!;
        public string? PatientCancelNote { get; set; } = null!;
        public string? PhysicianCancelNote { get; set; } = null!;
        public string? TransferNote { get; set; } = null!;
        public string AdditionalNote {get; set;}
    }

}