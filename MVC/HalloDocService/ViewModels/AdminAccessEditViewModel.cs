using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HalloDocService.ViewModels
{
    public class AdminAccessEditViewModel
    { 
        public int Id { get; set; }
        public string? Role {get; set;}
        public short? AccountType { get; set; }
        public IEnumerable<MenuList> AccessMenuList { get; set; } = new List<MenuList>();

        public List<int> SelectedIds = new();
    }
}