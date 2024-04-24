namespace EmployeeManagementService.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<EmployeeViewModel> Employeelist { get; set; } = new List<EmployeeViewModel>();
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int CurrentPageSize { get; set; }
        public int PageRangeStart { get; set; }
        public int PageRangeEnd { get; set; }
        public int TotalPage { get; set; }

        public EmployeeViewModel? CreateView {get; set;}
        public IEnumerable<DepartmentViewModel> AllDepartmentList {get; set;} = new List<DepartmentViewModel>();
    }

}