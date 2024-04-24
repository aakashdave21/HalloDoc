using System.Net.Http.Headers;
using EmployeeManagementRepository.CustomModel;
using EmployeeManagementRepository.DataModels;
using EmployeeManagementRepository.Interface;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementService.Implementation;
public class HomeRepository : IHomeRepository
{
        private readonly EmployeeMsContext _dbContext;

        public HomeRepository(EmployeeMsContext dbContext)
        {
            _dbContext = dbContext;
        }

        public EmployeeTable EmployeeList(int PageSize = 10, int PageNum = 1, string? SearchBy=null){
            IQueryable<Employee> query = _dbContext.Employees.Include(emp=>emp.Dept);
            if (!string.IsNullOrWhiteSpace(SearchBy))
            {
                query = query.Where(emp => emp.Firstname != null && emp.Firstname.ToLower().Contains(SearchBy));
            }
            EmployeeTable employeeTable = new(){
                TotalCount = query.Count(),
                EmployeeList = query.Skip((PageNum - 1) * PageSize).Take(PageSize)
            };
            return employeeTable;
        }

        public IEnumerable<Department> GetAllDepartments(){
            return _dbContext.Departments;
        }

        public void Create(Employee EmployeeData){
            if(EmployeeData.Id != 0){
                Employee? EmployeeDetails = _dbContext.Employees.FirstOrDefault(emp => emp.Id == EmployeeData.Id);
                if(EmployeeDetails!=null){
                    EmployeeDetails.Firstname = EmployeeData.Firstname;
                    EmployeeDetails.Lastname = EmployeeData.Lastname;
                    EmployeeDetails.Email = EmployeeData.Email;
                    EmployeeDetails.Age = EmployeeData.Age;
                    EmployeeDetails.Gender = EmployeeData.Gender;
                    EmployeeDetails.DeptId = EmployeeData.DeptId;
                    EmployeeDetails.Education = EmployeeData.Education;
                    EmployeeDetails.Package = EmployeeData.Package;
                    EmployeeDetails.Education = EmployeeData.Education;
                    EmployeeDetails.Company = EmployeeData.Company;
                    _dbContext.SaveChanges();
                    return;
                }
            }
            _dbContext.Employees.Add(EmployeeData);
            _dbContext.SaveChanges();
        }

        public void Delete(int Id){
            Employee? EmployeeData = _dbContext.Employees.FirstOrDefault(emp => emp.Id == Id);
            if(EmployeeData!=null){
                _dbContext.Employees.Remove(EmployeeData);
                _dbContext.SaveChanges();
                return;
            }
            throw new Exception();
        }

        public Employee GetSingleEmployee(int Id){
            Employee? EmployeeData = _dbContext.Employees.FirstOrDefault(emp => emp.Id == Id);
            if(EmployeeData!=null){
                return EmployeeData;
            }
            throw new Exception();
        }

}

