
using EmployeeManagementRepository.DataModels;
using EmployeeManagementRepository.Interface;
using EmployeeManagementService.ViewModels;

namespace EmployeeManagementService.Interface;
public interface IHomeService{
    HomeViewModel EmployeeList(int PageSize = 10, int PageNum = 1, string? SearchBy=null);
    IEnumerable<DepartmentViewModel> GetAllDepartments();

    void Create(EmployeeViewModel EmployeeData);
    void Delete(int Id);
    EmployeeViewModel GetSingleEmployee(int Id);

}