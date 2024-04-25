namespace HalloDocRepository.Enums;

public enum RequestStatusEnum{
    Unassigned=1,
    Accepted,
    Cancelled,
    MdRequest,
    MDONSite,
    Conclude,
    CancelledByPatient,	
    Closed,
    Unpaid,
    Clear,
    Blocked
}

public enum AccountTypeEnum
{
    Admin = 1,
    Provider
}