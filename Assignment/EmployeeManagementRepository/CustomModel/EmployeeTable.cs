using EmployeeManagementRepository.DataModels;

namespace EmployeeManagementRepository.CustomModel;
public class EmployeeTable
{
    public IEnumerable<Employee>? EmployeeList { get; set; }
    public int TotalCount { get; set; }
}