using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using Microsoft.AspNetCore.Http;

namespace HalloDocService.ViewModels
{
    public class PartnerViewModel
    {
        public IEnumerable<ProfessionList> AllProfessionsList { get; set; } = new List<ProfessionList>();
        public IEnumerable<VendorDetail> VendorsDetails {get; set;} = new List<VendorDetail>();
    }

    public class VendorDetail
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Business Name is required")]
        public string? BusinessName { get; set; }

        [Required(ErrorMessage = "Profession is required")]
        public int? ProfessionId { get; set; }
        public string? ProfessionName { get; set; }

        public string? FaxNumber { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Business Contact is required")]
        public string? BusinessContact { get; set; }

        [Required(ErrorMessage = "Street is required")]
        public string? Street { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string? City { get; set; }

        [Required(ErrorMessage = "State is required")]
        public int? State { get; set; }

        [Required(ErrorMessage = "Zip Code is required")]
        [MinLength(5, ErrorMessage = "Zip Code must be at least 5 characters")]
        public string? ZipCode { get; set; }

        public IEnumerable<RegionList> AllRegionsList { get; set; } = new List<RegionList>();
        public IEnumerable<ProfessionList> AllProfessionsList { get; set; } = new List<ProfessionList>();



    }

}