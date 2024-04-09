using HalloDocService.Admin.Interfaces;
using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
using HalloDocRepository.Admin.Interfaces;
using System.Globalization;
using HalloDocRepository.Interfaces;
using AdminTable = HalloDocRepository.DataModels.Admin;
using System.Reflection;

namespace HalloDocService.Admin.Implementation;
public class RecordsService : IRecordsService
{
    private readonly IRecordsRepo _recordsRepo;
    public RecordsService(IRecordsRepo recordsRepo)
    {
        _recordsRepo = recordsRepo;
    }
    public RecordsViewModel GetPatientHistory(PatientHistoryView Parameters, int PageNum = 1, int PageSize = 5)
    {
        Requestclient reqData = new()
        {
            Firstname = Parameters.Firstname,
            Lastname = Parameters.Lastname,
            Email = Parameters.Email,
            Phonenumber = Parameters.PhoneNumber
        };
        var (PatientList, totalCount) = _recordsRepo.GetPatientHistory(reqData, PageNum, PageSize);
        int startIndex = (PageNum - 1) * PageSize + 1;
        RecordsViewModel allRecords = new()
        {
            PatientHistoryList = PatientList.Select(pecnt => new PatientHistoryView()
            {
                Id = pecnt.Id,
                UserId = pecnt.Request.Userid,
                RequestId = pecnt.Requestid,
                Firstname = pecnt.Firstname,
                Lastname = pecnt.Lastname,
                Email = pecnt.Email,
                PhoneNumber = pecnt.Phonenumber,
                Address = string.IsNullOrEmpty(pecnt.Street) && string.IsNullOrEmpty(pecnt.City) && string.IsNullOrEmpty(pecnt?.Region?.Name) && string.IsNullOrEmpty(pecnt?.Zipcode)
                ? "-"
                : $"{pecnt.Street ?? ""}, {pecnt.City ?? ""}, {pecnt?.Region?.Name ?? ""}, {pecnt?.Zipcode ?? ""}",
            }),
            TotalCount = totalCount,
            CurrentPage = PageNum,
            CurrentPageSize = PageSize,
            PageRangeStart = totalCount == 0 ? 0 : startIndex,
            PageRangeEnd = Math.Min(startIndex + PageSize - 1, totalCount),
            TotalPage = (int)Math.Ceiling((double)totalCount / PageSize)
        };
        return allRecords;
    }

    public RecordsViewModel GetPatientRequest(int UserId,int RequestId){
        IEnumerable<Request> requestList = _recordsRepo.GetPatientRequest(UserId,RequestId);
        foreach (var item in requestList)
        {
            Console.WriteLine(item.Id);
            Console.WriteLine(item.Firstname);
            Console.WriteLine("-------------------");
        }
        RecordsViewModel patientRequest = new(){
            PatientRequestList = requestList.Select(req => new PatientRequestView(){
                Id = req.Id,
                PatientName = req?.Requestclients?.FirstOrDefault()?.Firstname + " " + req?.Requestclients?.FirstOrDefault()?.Lastname,
                CreatedDate =  req?.Createdat?.ToString("MMM dd, yyyy"),
                ConfirmationNumber = req?.Confirmationnumber,
                ProviderName = req?.Physician?.Firstname,
                Status = GetUserRequestType(req?.Status),
            })
        };
        return patientRequest;
    }
    public static string GetUserRequestType(int? status){
        return status switch
        {
            1 => "Unassigned",
            2 => "Accepted",
            3 => "Cancelled",
            4 => "MDOnSite",
            5 => "MDEnRoute",
            6 => "Conclude",
            7 => "CancelledByPatient",
            8 => "Closed",
            9 => "Unpaid",
            10 => "Clear",
            11 => "Blocked",
            // 12 => "Clear",
            // 13 => "CancelledByProvider",
            // 14 => "CCUploadedByClient",
            // 15 => "CCApprovedByAdmin",
            _ => "Unknown",
        };
    }
}
