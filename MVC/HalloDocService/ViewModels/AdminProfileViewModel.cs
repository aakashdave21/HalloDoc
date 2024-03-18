using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HalloDocService.ViewModels
{
    public class AdminProfileViewModel
    {
        // Account Information
        public int Id { get; set; }
        public int AdminId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Status { get; set; }
        public int? Role { get; set; }

        // Administrator Information
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Compare("Email", ErrorMessage = "Emails do not match")]
        public string ConfirmEmail { get; set; }
        public string Mobile { get; set; }
        public bool[] CheckBoxes { get; set; }

        // Mailing and Billing Information
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }

        public IEnumerable<RoleList> roleSelect {get; set;}
        public IEnumerable<RegionList> regionSelect {get; set;}

        public List<int> UnCheckedRegions { get; set; }
    }

    public class RoleList{
        public int Id {get; set;}
        public string? Name {get; set;}
    }
    public class RegionList{
        public int Id {get; set;}
        public string? Name {get; set;}
    }
}