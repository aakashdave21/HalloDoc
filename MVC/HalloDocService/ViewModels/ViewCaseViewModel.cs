using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HalloDocService.ViewModels
{
    public class ViewCaseViewModel
    {
        public int Id { get; set; }
        public string ConfirmationNumber { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Region { get; set; }
        public string PropertyName { get; set; }
        public string Room { get; set; }
        public string Symptoms { get; set; }
        public string RequestType { get; set; }
    }

}