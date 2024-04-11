using HalloDocRepository.Admin.Implementation;
using HalloDocRepository.DataModels;
using static HalloDocRepository.Admin.Implementation.RecordsRepo;

namespace HalloDocRepository.Admin.Interfaces;
public interface IRecordsRepo
{
    (IEnumerable<Requestclient>, int) GetPatientHistory(Requestclient Parameters, int PageNum = 1, int PageSize = 5);

    IEnumerable<Request> GetPatientRequest(int UserId, int RequestId);

    (IEnumerable<Request>, int) GetAllRecords(RecordsRepoView Parameters, int PageNum = 1, int PageSize = 5);

    void DeleteRecord(int ReqId);

    (IEnumerable<Emaillog>, int) EmailLogs(EmailRepoView Parameters, int PageNum = 1, int PageSize = 5);

    (IEnumerable<Smslog>, int) SMSLogs(EmailRepoView Parameters, int PageNum = 1, int PageSize = 5);
    (IEnumerable<Blockrequest>, int) BlockHistory(EmailRepoView Parameters, int PageNum = 1, int PageSize = 5);

    void UnblockRequest(int Id);
}

