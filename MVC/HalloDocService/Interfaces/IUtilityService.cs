namespace HalloDocService.Interfaces;
public interface IUtilityService
{
    public void EmailSend(string rcvrMail,string? MessageBody=null,string? Subject=null,string[]? fileAttachments=null,int? role=null,int? req=null,int? phy=null,int? admin=null);

}
