using HalloDocService.Admin.Interfaces;
using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
using HalloDocRepository.Admin.Interfaces;
using static HalloDocRepository.Admin.Implementation.RecordsRepo;
using HalloDocRepository.Admin.Implementation;

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
        RecordsViewModel patientRequest = new(){
            PatientRequestList = requestList.Select(req => new PatientRequestView(){
                Id = req.Id,
                PatientName = req?.Requestclients?.FirstOrDefault()?.Firstname + " " + req?.Requestclients?.FirstOrDefault()?.Lastname,
                CreatedDate =  req?.Createdat?.ToString("MMM dd, yyyy"),
                ConfirmationNumber = req?.Confirmationnumber ?? "-",
                ProviderName = req?.Physician?.Firstname ?? "-",
                Status = GetUserRequestType(req?.Status) ?? "-",
                ConcludedDate = req?.Requeststatuslogs?.FirstOrDefault(r=>r.Requestid == req.Id && r.Status == 6)?.Createddate.ToString("MMM dd, yyyy") ?? "-"
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


    public RecordsViewModel GetAllRecords(RecordsView Parameters,int PageNum = 1,int PageSize = 5){
        RecordsRepoView repoView = new(){
            PatientName = Parameters.PatientName,
            PhysicianName = Parameters.PhysicianName,
            Email = Parameters.Email,
            PhoneNumber = Parameters.PhoneNumber,
            InputFromDate = Parameters.InputFromDate,
            InputToDate = Parameters.InputToDate,
            InputRequestStatus = Parameters.InputRequestStatus,
            InputRequestType = Parameters.InputRequestType
        };
        var (RecordList, totalCount) = _recordsRepo.GetAllRecords(repoView, PageNum, PageSize);
        
        int startIndex = (PageNum - 1) * PageSize + 1;
        RecordsViewModel recordData = new(){
            RecordsRequestList = RecordList.Select(rec => new RecordsView(){
                RequestId = rec.Id,
                PatientName = $"{rec?.Requestclients?.FirstOrDefault()?.Firstname} {rec?.Requestclients?.FirstOrDefault()?.Lastname}" ?? "-",
                Requestor = rec?.Firstname +" " +rec?.Lastname ?? "-",
                DateOfService = rec?.Accepteddate != null ? rec.Accepteddate?.ToString("MMM dd ,yyyy") : "-",
                Email = rec?.Requestclients?.FirstOrDefault()?.Email ?? "-",
                PhoneNumber = rec?.Requestclients?.FirstOrDefault()?.Phonenumber ?? "-",
                Address = string.IsNullOrEmpty(rec?.Requestclients?.FirstOrDefault()?.Street) && string.IsNullOrEmpty(rec?.Requestclients?.FirstOrDefault()?.City) && string.IsNullOrEmpty(rec?.Requestclients?.FirstOrDefault()?.State)
                ? "-"
                : $"{rec?.Requestclients?.FirstOrDefault()?.Street ?? ""}, {rec?.Requestclients?.FirstOrDefault()?.City ?? ""}, {rec?.Requestclients?.FirstOrDefault()?.State ?? ""}",
                Zip = string.IsNullOrEmpty(rec?.Requestclients?.FirstOrDefault()?.Zipcode) ? "-" : rec?.Requestclients?.FirstOrDefault()?.Zipcode,
                RequestStatus = GetUserRequestType(rec?.Status),
                PhysicianId = rec?.Physicianid,
                PhysicianName = rec?.Physician?.Firstname ?? "-",
                PhysicianNote = rec?.Requestnote?.Physiciannotes ?? "-",
                CancelledByProviderNote = rec?.Requeststatuslogs?.FirstOrDefault(r => r.Requestid == r.Id && r.Status == 3 && r.Physicianid != null)?.Notes ?? "-",
                AdminNote = rec?.Requestnote?.Adminnotes ?? "-",
                PatientNote = rec?.Symptoms ?? "-",
            }),
            TotalCount = totalCount,
            CurrentPage = PageNum,
            CurrentPageSize = PageSize,
            PageRangeStart = totalCount == 0 ? 0 : startIndex,
            PageRangeEnd = Math.Min(startIndex + PageSize - 1, totalCount),
            TotalPage = (int)Math.Ceiling((double)totalCount / PageSize)
        };

        return recordData;
    }

    public void DeleteRecord(int ReqId){
        _recordsRepo.DeleteRecord(ReqId);
    }

    // public RecordsViewModel EmailLogs(int PageNum = 1,int PageSize = 5){
    //     var (RecordList, totalCount) = _recordsRepo.EmailLogs(PageNum, PageSize);
        // int startIndex = (PageNum - 1) * PageSize + 1;
        // RecordsViewModel recordData = new(){
        //     EmailLogsList = RecordList.Select(rec => new EmailLogView(){
        //         Id = rec.Id,
        //         Email = rec.Email,
        //         Subject = rec.Subject,s
        //         Message = rec.Message,
        //         CreatedDate = rec.Createddate.ToString("MMM dd, yyyy")
        //     }),
        //     TotalCount = totalCount,
        //     CurrentPage = PageNum,
        //     CurrentPageSize = PageSize,
        //     PageRangeStart = totalCount == 0 ? 0 : startIndex,
        //     PageRangeEnd = Math.Min(startIndex + PageSize - 1, totalCount),
        //     TotalPage = (int)Math.Ceiling((double)totalCount / PageSize)
        // };
    // }

}
