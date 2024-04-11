using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
using Microsoft.VisualBasic;

namespace HalloDocService.Admin.Interfaces;
public interface IRecordsService{

    RecordsViewModel GetPatientHistory(PatientHistoryView Parameters,int PageNum = 1,int PageSize = 5);
    RecordsViewModel GetPatientRequest(int UserId,int RequestId);
    RecordsViewModel GetAllRecords(RecordsView Parameters,int PageNum = 1,int PageSize = 5);
    void DeleteRecord(int ReqId);

    RecordsViewModel EmailLogs(EmailLogsView Parameters,int PageNum = 1,int PageSize = 5);
    RecordsViewModel SMSLogs(EmailLogsView Parameters, int PageNum = 1, int PageSize = 5);
    RecordsViewModel BlockHistory(EmailLogsView Parameters, int PageNum = 1, int PageSize = 5);

    void UnblockRequest(int Id);
        
}