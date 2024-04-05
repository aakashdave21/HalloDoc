using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HalloDocService.ViewModels
{
    public class ProviderLocationViewModel
    {
        public IEnumerable<ProviderLocation> ProviderLocationList = new List<ProviderLocation>();
    }

    public class ProviderLocation{
        public int Id { get; set; }
        public string? PhyicianName { get; set; }
        public string? Address { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
    }
}