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
        public string AdditionalNote {get; set;}
    }

}