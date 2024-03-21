
using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;

namespace HalloDocService.Interfaces;
public interface IUtilityService
{
    public Task EmailSend(string callbackUrl,string rcvrMail,string? MessageBody=null,string? Subject=null);

}
