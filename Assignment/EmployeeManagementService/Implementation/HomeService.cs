using EmployeeManagementRepository.CustomModel;
using EmployeeManagementRepository.DataModels;
using EmployeeManagementRepository.Interface;
using EmployeeManagementService.Interface;
using EmployeeManagementService.ViewModels;

namespace EmployeeManagementService.Implementation;

public enum Edu{
    Diploma,
    Bachelors,
    Masters,
    Phd
}
public class HomeService : IHomeService
{
    private readonly IHomeRepository _homeRepository;
    public HomeService(IHomeRepository homeRepository)
    {
        _homeRepository = homeRepository;
    }

    public HomeViewModel EmployeeList(int PageSize = 10, int PageNum = 1, string? SearchBy=null){
        int StartIndex = (PageNum - 1) * PageSize + 1;
        EmployeeTable EmployeeData = _homeRepository.EmployeeList(PageSize, PageNum, SearchBy);
        HomeViewModel EmployeeListWithPage = new(){
            Employeelist = EmployeeData.EmployeeList.Select(emp => new EmployeeViewModel(){
                Id = emp.Id,
                FirstName = emp.Firstname,
                LastName = emp.Lastname,
                Email = emp.Email,
                Age = emp.Age,
                Gender = emp.Gender,
                DepartmentDetails = new DepartmentViewModel(){
                    Id = emp?.Dept?.Id,
                    Name = emp?.Dept?.Name
                },
                Education = emp?.Education,
                EducationName = GetEducation(emp?.Education),
                Company = emp?.Company,
                Experience = emp?.Experience,
                Package = emp?.Package
            }),
            TotalCount = EmployeeData.TotalCount,
            CurrentPage = PageNum,
            CurrentPageSize = PageSize,
            PageRangeStart = EmployeeData.TotalCount == 0 ? 0 : StartIndex,
            PageRangeEnd = Math.Min(StartIndex + PageSize - 1, EmployeeData.TotalCount),
            TotalPage = (int)Math.Ceiling((double)EmployeeData.TotalCount / PageSize)
        };
        return EmployeeListWithPage;
    }

    private string GetEducation(int? EduId){
        switch(EduId){
            case 1 : return "Diploma";
            case 2 : return "Graduate";
            case 3 : return "Post Graduate";
            case 4 : return "Intermediate";
            case 5 : return "Matric";
            default : return "";
        }
    }

    public IEnumerable<DepartmentViewModel> GetAllDepartments(){
        IEnumerable<DepartmentViewModel> departmentList = _homeRepository.GetAllDepartments().Select(dept => new DepartmentViewModel(){
                Id = dept.Id,
                Name = dept.Name,
        });
        return departmentList;
    }

    public void Create(EmployeeViewModel EmployeeData){
        Employee newEmployee = new(){
            Id = EmployeeData.Id,
            Firstname = EmployeeData.FirstName,
            Lastname = EmployeeData.LastName,
            Email = EmployeeData.Email,
            Age = EmployeeData.Age,
            Gender = EmployeeData.Gender,
            Education = EmployeeData.Education,
            Company = EmployeeData.Company,
            Experience = EmployeeData.Experience,
            Package = EmployeeData.Package,
            DeptId = EmployeeData.DepartmentId
        };
        _homeRepository.Create(newEmployee);
    }

    public void Delete(int Id){
        _homeRepository.Delete(Id);
    }


    public EmployeeViewModel GetSingleEmployee(int Id){
        Employee EmployeeDetails = _homeRepository.GetSingleEmployee(Id);
        EmployeeViewModel EmployeeData = new(){
            Id = EmployeeDetails.Id,
            FirstName = EmployeeDetails.Firstname,
            LastName = EmployeeDetails.Lastname,
            Email = EmployeeDetails.Email,
            Age = EmployeeDetails.Age,
            Gender = EmployeeDetails.Gender,
            DepartmentId = EmployeeDetails.DeptId,
            Education = EmployeeDetails.Education,
            Company = EmployeeDetails.Company,
            Experience = EmployeeDetails.Experience,
            Package = EmployeeDetails.Package
        };
        return EmployeeData;
    }

}
