using EmployeeManagementRepository.CustomModel;
using EmployeeManagementRepository.DataModels;

namespace EmployeeManagementRepository.Interface;
public interface IHomeRepository{
    EmployeeTable EmployeeList(int PageSize = 10, int PageNum = 1, string? searchBy=null);
    IEnumerable<Department> GetAllDepartments();
    void Create(Employee EmployeeData);

    void Delete(int Id);

    Employee GetSingleEmployee(int Id);
}