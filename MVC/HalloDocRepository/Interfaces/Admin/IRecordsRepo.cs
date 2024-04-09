using HalloDocRepository.Admin.Implementation;
using HalloDocRepository.DataModels;

namespace HalloDocRepository.Admin.Interfaces;
public interface IRecordsRepo
{
    (IEnumerable<Requestclient>,int) GetPatientHistory(Requestclient Parameters,int PageNum = 1,int PageSize = 5);

    IEnumerable<Request> GetPatientRequest(int UserId,int RequestId);
}