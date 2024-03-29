using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HalloDocService.ViewModels
{
    public class AdminAccessViewModel
    { 
        public IEnumerable<AccessList> AccessRoleList { get; set; } = new List<AccessList>();
        public IEnumerable<MenuList> AccessMenuList { get; set; } = new List<MenuList>();

        public List<int> SelectedIds = new();
    }
    
    public class AccessList{
        public int Id { get; set; }
        public string? RoleName { get; set; }
        public string? AccountType { get; set; }
    }

    public class MenuList{
        public int Id { get; set; }
        public string? Name { get; set; }
        public short? AccountType { get; set; }
    }
}