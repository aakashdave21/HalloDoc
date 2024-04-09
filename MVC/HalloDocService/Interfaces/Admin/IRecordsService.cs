using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
using Microsoft.VisualBasic;

namespace HalloDocService.Admin.Interfaces;
public interface IRecordsService{

    RecordsViewModel GetPatientHistory(PatientHistoryView Parameters,int PageNum = 1,int PageSize = 5);
    RecordsViewModel GetPatientRequest(int UserId,int RequestId);
}