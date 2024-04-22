using HalloDocService.Admin.Interfaces;
using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
using HalloDocRepository.Admin.Interfaces;
using System.Globalization;
using HalloDocRepository.Interfaces;
using AdminTable = HalloDocRepository.DataModels.Admin;
using HalloDocService.Interfaces;


namespace HalloDocService.Admin.Implementation;
public class AdminDashboardService : IAdminDashboardService
{

    private readonly IAdminDashboardRepo _dashboardRepo;
    private readonly IDashboardRepo _patientDashboardRepo;
    private readonly IPatientRequestRepo _patientRequestRepo;
    private readonly IPatientRequestService _patientRequestService;

    private readonly IProviderRepo _providerRepo;

    public AdminDashboardService(IAdminDashboardRepo dashboardRepo, IDashboardRepo patientDashboardRepo, IPatientRequestRepo patientRequestRepo, IPatientRequestService patientRequestService, IProviderRepo providerRepo)
    {
        _dashboardRepo = dashboardRepo;
        _patientDashboardRepo = patientDashboardRepo;
        _patientRequestRepo = patientRequestRepo;
        _patientRequestService = patientRequestService;
        _providerRepo = providerRepo;

    }

    public IEnumerable<HeaderMenu>? GetRoleOfUser(int AspUserId)
    {
        return _dashboardRepo?.GetUserRoles(AspUserId)?.Select(x => new HeaderMenu()
        {
            Title = x.Menu.Title,
            Name = x.Menu.Name,
            Id = x.Id,
            SortOrder = x.Menu.Sortorder,
            AccountType = x.Menu.Accounttype
        }).OrderBy(x=>x.SortOrder).ToList();
    }
    // Patient Request Implementation
    public (List<RequestViewModel>, int totalCount) GetNewStatusRequest(string? searchBy, int reqTypeId, int pageNumber, int pageSize, int region)
    {

        var result = _dashboardRepo.GetNewRequest(searchBy, reqTypeId, pageNumber, pageSize, region);
        var (requests, totalCount) = result;

        // Convert each Request object to RequestViewModel
        var requestViewModels = requests.Select(r => new RequestViewModel
        {
            Id = r.Id,
            Firstname = r.Requestclients.FirstOrDefault()?.Firstname,
            Lastname = r.Requestclients.FirstOrDefault()?.Lastname,
            Email = r.Requestclients.FirstOrDefault()?.Email,
            Phonenumber = r.Requestclients.FirstOrDefault()?.Phonenumber,
            BirthDate = r.Requestclients.FirstOrDefault()?.Strmonth + ", " + r.Requestclients.FirstOrDefault()?.Intdate + " " + r.Requestclients.FirstOrDefault()?.Intyear,
            Requestor = r.Firstname + ", " + r.Lastname,
            RequestedDate = r.Createdat?.ToString("MMM,d yyyy HH\\h m\\m ss"),
            Address = r.PropertyName != null ? "Room No/Property : " + r.PropertyName : r.Requestclients.FirstOrDefault()?.Street + ", " + r.Requestclients.FirstOrDefault()?.City + ", " + r.Requestclients.FirstOrDefault()?.State + ", " + r.Requestclients.FirstOrDefault()?.Zipcode,
            Notes = r.Symptoms != null ? r.Symptoms : r.Requestclients.FirstOrDefault()?.Notes,
            RequestType = r.Requesttypeid,
            IsFinalized = r?.Encounterform?.Isfinalized ?? false
        });

        return (requestViewModels.ToList(), totalCount);
    }


    public (List<RequestViewModel>, int totalCount) GetPendingStatusRequest(string? searchBy, int reqTypeId, int pageNumber, int pageSize, int region)
    {
        var result = _dashboardRepo.GetPendingStatusRequest(searchBy, reqTypeId, pageNumber, pageSize, region);
        var (requests, totalCount) = result;

        // Convert each Request object to RequestViewModel
        var requestViewModels = requests.Select(r => new RequestViewModel
        {
            Id = r.Id,
            Firstname = r.Requestclients.FirstOrDefault()?.Firstname,
            Lastname = r.Requestclients.FirstOrDefault()?.Lastname,
            Email = r.Requestclients.FirstOrDefault()?.Email,
            Phonenumber = r.Requestclients.FirstOrDefault()?.Phonenumber,
            BirthDate = r.Requestclients.FirstOrDefault()?.Strmonth + ", " + r.Requestclients.FirstOrDefault()?.Intdate + " " + r.Requestclients.FirstOrDefault()?.Intyear,
            Requestor = r.Firstname + ", " + r.Lastname,
            RequestedDate = r.Createdat?.ToString("MMM,d yyyy HH\\h m\\m ss"),
            Address = r.PropertyName != null ? "Room No/Property : " + r.PropertyName : r.Requestclients.FirstOrDefault()?.Street + ", " + r.Requestclients.FirstOrDefault()?.City + ", " + r.Requestclients.FirstOrDefault()?.State + ", " + r.Requestclients.FirstOrDefault()?.Zipcode,
            Notes = r.Symptoms != null ? r.Symptoms : r.Requestclients.FirstOrDefault()?.Notes,
            RequestType = r.Requesttypeid,
            PhysicianId = r.Physicianid,
            PhysicianName = r.Physician != null ? r.Physician?.Firstname + ", " + r.Physician?.Lastname : "-",
            ServiceDate = r.Createdat?.ToString("MMM,d yyyy HH\\h m\\m ss"),
            IsFinalized = r?.Encounterform?.Isfinalized ?? false
        });

        return (requestViewModels.ToList(), totalCount);
    }
    public (List<RequestViewModel>, int totalCount) GetActiveStatusRequest(string? searchBy, int reqTypeId, int pageNumber, int pageSize, int region)
    {
        var result = _dashboardRepo.GetActiveStatusRequest(searchBy, reqTypeId, pageNumber, pageSize, region);
        var (requests, totalCount) = result;

        // Convert each Request object to RequestViewModel
        var requestViewModels = requests.Select(r => new RequestViewModel
        {
            Id = r.Id,
            Firstname = r.Requestclients.FirstOrDefault()?.Firstname,
            Lastname = r.Requestclients.FirstOrDefault()?.Lastname,
            Email = r.Requestclients.FirstOrDefault()?.Email,
            Phonenumber = r.Requestclients.FirstOrDefault()?.Phonenumber,
            BirthDate = r.Requestclients.FirstOrDefault()?.Strmonth + ", " + r.Requestclients.FirstOrDefault()?.Intdate + " " + r.Requestclients.FirstOrDefault()?.Intyear,
            Requestor = r.Firstname + ", " + r.Lastname,
            RequestedDate = r.Createdat?.ToString("MMM,d yyyy HH\\h m\\m ss"),
            Address = r.PropertyName != null ? "Room No/Property : " + r.PropertyName : r.Requestclients.FirstOrDefault()?.Street + ", " + r.Requestclients.FirstOrDefault()?.City + ", " + r.Requestclients.FirstOrDefault()?.State + ", " + r.Requestclients.FirstOrDefault()?.Zipcode,
            Notes = r.Symptoms != null ? r.Symptoms : r.Requestclients.FirstOrDefault()?.Notes,
            RequestType = r.Requesttypeid,
            PhysicianId = r.Physicianid,
            PhysicianName = r.Physician != null ? r.Physician?.Firstname + ", " + r.Physician?.Lastname : "-",
            ServiceDate = r.Createdat?.ToString("MMM,d yyyy HH\\h m\\m ss"),
            IsFinalized = r?.Encounterform?.Isfinalized ?? false
            // Add other properties as needed
        });

        return (requestViewModels.ToList(), totalCount);
    }
    public (List<RequestViewModel>, int totalCount) GetConcludeStatusRequest(string? searchBy, int reqTypeId, int pageNumber, int pageSize, int region)
    {
        var result = _dashboardRepo.GetConcludeStatusRequest(searchBy, reqTypeId, pageNumber, pageSize, region);
        var (requests, totalCount) = result;

        // Convert each Request object to RequestViewModel
        var requestViewModels = requests.Select(r => new RequestViewModel
        {
            Id = r.Id,
            Firstname = r.Requestclients.FirstOrDefault()?.Firstname,
            Lastname = r.Requestclients.FirstOrDefault()?.Lastname,
            Email = r.Requestclients.FirstOrDefault()?.Email,
            Phonenumber = r.Requestclients.FirstOrDefault()?.Phonenumber,
            BirthDate = r.Requestclients.FirstOrDefault()?.Strmonth + ", " + r.Requestclients.FirstOrDefault()?.Intdate + " " + r.Requestclients.FirstOrDefault()?.Intyear,
            Requestor = r.Firstname + ", " + r.Lastname,
            RequestedDate = r.Createdat?.ToString("MMM,d yyyy HH\\h m\\m ss"),
            Address = r.PropertyName != null ? "Room No/Property : " + r.PropertyName : r.Requestclients.FirstOrDefault()?.Street + ", " + r.Requestclients.FirstOrDefault()?.City + ", " + r.Requestclients.FirstOrDefault()?.State + ", " + r.Requestclients.FirstOrDefault()?.Zipcode,
            Notes = r.Symptoms != null ? r.Symptoms : r.Requestclients.FirstOrDefault()?.Notes,
            RequestType = r.Requesttypeid,
            PhysicianId = r.Physicianid,
            PhysicianName = r.Physician != null ? r.Physician?.Firstname + ", " + r.Physician?.Lastname : "-",
            ServiceDate = r.Createdat?.ToString("MMM,d yyyy HH\\h m\\m ss"),
            IsFinalized = r?.Encounterform?.Isfinalized ?? false
            // Add other properties as needed
        });

        return (requestViewModels.ToList(), totalCount);
    }
    public (List<RequestViewModel>, int totalCount) GetCloseStatusRequest(string? searchBy, int reqTypeId, int pageNumber, int pageSize, int region)
    {
        var result = _dashboardRepo.GetCloseStatusRequest(searchBy, reqTypeId, pageNumber, pageSize, region);
        var (requests, totalCount) = result;

        // Convert each Request object to RequestViewModel
        var requestViewModels = requests.Select(r => new RequestViewModel
        {
            Id = r.Id,
            Firstname = r.Requestclients.FirstOrDefault()?.Firstname,
            Lastname = r.Requestclients.FirstOrDefault()?.Lastname,
            Email = r.Requestclients.FirstOrDefault()?.Email,
            Phonenumber = r.Requestclients.FirstOrDefault()?.Phonenumber,
            BirthDate = r.Requestclients.FirstOrDefault()?.Strmonth + ", " + r.Requestclients.FirstOrDefault()?.Intdate + " " + r.Requestclients.FirstOrDefault()?.Intyear,
            Requestor = r.Firstname + ", " + r.Lastname,
            RequestedDate = r.Createdat?.ToString("MMM,d yyyy HH\\h m\\m ss"),
            Address = r.PropertyName != null ? "Room No/Property : " + r.PropertyName : r.Requestclients.FirstOrDefault()?.Street + ", " + r.Requestclients.FirstOrDefault()?.City + ", " + r.Requestclients.FirstOrDefault()?.State + ", " + r.Requestclients.FirstOrDefault()?.Zipcode,
            Notes = r.Symptoms != null ? r.Symptoms : r.Requestclients.FirstOrDefault()?.Notes,
            RequestType = r.Requesttypeid,
            PhysicianId = r.Physicianid,
            PhysicianName = r.Physician != null ? r.Physician?.Firstname + ", " + r.Physician?.Lastname : "-",
            ServiceDate = r.Createdat?.ToString("MMM,d yyyy HH\\h m\\m ss"),
            Region = r.Requestclients.FirstOrDefault()?.Region?.Name ?? "-",
            IsFinalized = r?.Encounterform?.Isfinalized ?? false
        });

        return (requestViewModels.ToList(), totalCount);
    }
    public (List<RequestViewModel>, int totalCount) GetUnpaidStatusRequest(string? searchBy, int reqTypeId, int pageNumber, int pageSize, int region)
    {

        var result = _dashboardRepo.GetUnpaidStatusRequest(searchBy, reqTypeId, pageNumber, pageSize, region);
        var (requests, totalCount) = result;

        // Convert each Request object to RequestViewModel
        var requestViewModels = requests.Select(r => new RequestViewModel
        {
            Id = r.Id,
            Firstname = r.Requestclients.FirstOrDefault()?.Firstname,
            Lastname = r.Requestclients.FirstOrDefault()?.Lastname,
            Email = r.Requestclients.FirstOrDefault()?.Email,
            Phonenumber = r.Requestclients.FirstOrDefault()?.Phonenumber,
            BirthDate = r.Requestclients.FirstOrDefault()?.Strmonth + ", " + r.Requestclients.FirstOrDefault()?.Intdate + " " + r.Requestclients.FirstOrDefault()?.Intyear,
            Requestor = r.Firstname + ", " + r.Lastname,
            RequestedDate = r.Createdat?.ToString("MMM,d yyyy HH\\h m\\m ss"),
            Address = r.PropertyName != null ? "Room No/Property : " + r.PropertyName : r.Requestclients.FirstOrDefault()?.Street + ", " + r.Requestclients.FirstOrDefault()?.City + ", " + r.Requestclients.FirstOrDefault()?.State + ", " + r.Requestclients.FirstOrDefault()?.Zipcode,
            Notes = r.Symptoms != null ? r.Symptoms : r.Requestclients.FirstOrDefault()?.Notes,
            RequestType = r.Requesttypeid,
            PhysicianId = r.Physicianid,
            PhysicianName = r.Physician != null ? r.Physician?.Firstname + ", " + r.Physician?.Lastname : "-",
            ServiceDate = r.Createdat?.ToString("MMM,d yyyy HH\\h m\\m ss"),
            Region = r.Requestclients.FirstOrDefault()?.Region?.Name ?? "-",
            IsFinalized = r?.Encounterform?.Isfinalized ?? false
        });

        return (requestViewModels.ToList(), totalCount);
    }
    public Dictionary<string, int> CountRequestByType()
    {
        return _dashboardRepo.CountRequestByType();

    }
    public ViewCaseViewModel GetViewCaseDetails(int id)
    {
        Request resData = _dashboardRepo.GetViewCaseDetails(id);
        DateTime date = DateTime.ParseExact(resData.Requestclients.FirstOrDefault()?.Strmonth, "MMMM", CultureInfo.InvariantCulture);
        int year = resData.Requestclients.FirstOrDefault().Intyear ?? 0000;
        int day = resData.Requestclients.FirstOrDefault().Intdate ?? 1;
        date = new DateTime(year, date.Month, day);
        ViewCaseViewModel viewCase = new()
        {
            Id = resData.Id,
            Firstname = resData.Requestclients?.FirstOrDefault()?.Firstname,
            Lastname = resData.Requestclients?.FirstOrDefault()?.Lastname,
            Email = resData.Requestclients?.FirstOrDefault()?.Email,
            ConfirmationNumber = resData.Confirmationnumber ?? "No Confirmation Number",
            Phone = resData.Requestclients?.FirstOrDefault()?.Phonenumber,
            PropertyName = resData.PropertyName,
            Room = resData.Roomnoofpatient,
            DateOfBirth = date.ToString("yyyy-MM-dd"),
            Region = resData.Requestclients.FirstOrDefault()?.Region?.Name ?? " ",
            Symptoms = resData.Symptoms ?? resData.Requestclients?.FirstOrDefault()?.Notes,
            RequestType = resData?.Requesttype?.Name
        };

        return viewCase;
    }

    public ViewNotesViewModel GetViewNotesDetails(int reqId, int reqType = 1)
    {
        var reqData = _dashboardRepo.GetViewNotesDetails(reqId);
        var patientNote = _dashboardRepo.GetPatientNoteDetails(reqId);
        IQueryable<Requeststatuslog> cancelAndTransferNote = _dashboardRepo.GetAllCancelNotes(reqId);
        var PatientCancelNoteFromQuery = cancelAndTransferNote.FirstOrDefault(req => req.Status == 7)?.Notes;
        var AdminCancelNoteFromQuery = cancelAndTransferNote.FirstOrDefault(req => req.Status == 3 && req.Adminid != null && req.Physicianid == null)?.Notes;
        var PhysicianCancelNoteFromQuery = cancelAndTransferNote.FirstOrDefault(req => req.Status == 3 && req.Adminid == null && req.Physicianid != null)?.Notes;
        var filteredNotes = cancelAndTransferNote.Where(req => req.Status == 2);
        var sentences = new List<string>();
        foreach (var item in filteredNotes)
        {
            string sentence;
            if (item.Transtoadmin == false)
            {
                sentence = "admin transferred to physician " + item.Transtophysicianid + "on " + item.Createddate.ToString("dd-MM-yyyy") + " at " + item.Createddate.ToString("hh:mm tt") + " -> " + item.Notes;
            }
            else
            {
                sentence = "physician sent Transfer Request to admin " + item.Adminid + " on " + item.Createddate.ToString("dd-MM-yyyy") + " at " + item.Createddate.ToString("hh:mm tt") + " -> " + item.Notes;
            }
            sentences.Add(sentence);
        }

        ViewNotesViewModel viewNotes = new();
        if (reqData != null || patientNote != null || sentences.Count > 0)
        {
            viewNotes = new ViewNotesViewModel
            {
                Id = reqData?.Id ?? 0,
                NoteId = reqData?.Id ?? 0,
                ReqId = reqId,
                AdminNote = reqData?.Adminnotes ?? "",
                PhysicianNote = reqData?.Physiciannotes ?? "",
                AdditionalNote = reqType == 1 ? reqData?.Adminnotes ?? "" : reqData?.Physiciannotes ?? "",
                PatientNote = patientNote?.Notes ?? "",
                PatientCancelNote = PatientCancelNoteFromQuery ?? "",
                AdminCancelNote = AdminCancelNoteFromQuery ?? "",
                PhysicianCancelNote = PhysicianCancelNoteFromQuery ?? "",
                TransferNote = sentences.Any() ? sentences : new List<string>()
            };
        }

        return viewNotes;
    }
    public void SaveAdditionalNotes(string AdditionalNote, int noteId, int reqId, int reqType)
    {
        _dashboardRepo.SaveAdditionalNotes(AdditionalNote, noteId, reqId, reqType);
    }

    public void CancleRequestCase(int reqId, string reason, string additionalNotes)
    {
        short newStatus = 3;
        int adminId = 1;
        Nullable<int> physicianId = null;
        short oldStatus = _dashboardRepo.GetStatusOfRequest(reqId);
        int? noteId = _dashboardRepo.GetNoteIdFromRequestId(reqId);

        _dashboardRepo.ChangeStatusOfRequest(reqId, newStatus);
        _dashboardRepo.AddStatusLog(reqId, newStatus, oldStatus, reason, adminId, physicianId, null);

        if (noteId != null) _dashboardRepo.SaveAdditionalNotes(additionalNotes, (int)noteId, reqId);
        else _dashboardRepo.SaveAdditionalNotes(additionalNotes, 0, reqId);

    }

    public List<Region> GetRegions()
    {
        return _dashboardRepo.GetRegions();
    }
    public async Task<IEnumerable<Casetag>> GetCaseTag()
    {
        return await _dashboardRepo.GetCaseTag();
    }

    public async Task<IEnumerable<Physician>> GetPhysicianByRegion(int regionId)
    {
        return await _dashboardRepo.GetPhysicianByRegion(regionId);
    }

    public void AssignRequestCase(int reqId, int transPhyId, int? adminId, string desc)
    {
        // short newStatus = 2;
        // short oldStatus = _dashboardRepo.GetStatusOfRequest(reqId);
        // _dashboardRepo.ChangeStatusOfRequest(reqId, newStatus);
        _dashboardRepo.AddStatusLog(reqId, 1, 1, desc, adminId, null, transPhyId);
        _dashboardRepo.AddPhysicianToRequest(reqId, transPhyId);
    }

    public async Task BlockRequestCase(int reqId, int? adminId, string reason)
    {
        Request requestData = _dashboardRepo.GetSingleRequestDetails(reqId);
        short status = 11; //Blocked
        _dashboardRepo.ChangeStatusOfRequest(reqId, status);
        _dashboardRepo.SetBlockFieldRequest(reqId);

        Blockrequest newBlockRequest = new()
        {
            Requestid = reqId,
            Phonenumber = requestData.Requestclients.FirstOrDefault()?.Phonenumber,
            Email = requestData.Requestclients.FirstOrDefault()?.Email,
            Isactive = requestData.Status == 4 || requestData.Status == 5,
            Reason = reason
        };
        _dashboardRepo.AddBlockRequest(newBlockRequest);
    }

    public Request GetSingleRequest(int reqId)
    {
        return _dashboardRepo.GetSingleRequest(reqId);
    }

    public void DeleteDocument(int docId)
    {
        _dashboardRepo.DeleteDocument(docId);
    }

    public IEnumerable<ProfessionList> GetAllProfessions()
    {
        IEnumerable<Healthprofessionaltype> healthProfessionals = _dashboardRepo.GetAllProfessions();
        IEnumerable<ProfessionList> Porfessions = healthProfessionals.Select(prof => new ProfessionList
        {
            ProfessionId = prof.Id,
            ProfessionName = prof.Professionname
        }).ToList();
        return Porfessions;
    }

    public IEnumerable<BusinessList> GetBusinessByProfession(int ProfessionId)
    {
        IEnumerable<BusinessList> businessLists = _dashboardRepo.GetBusinessByProfession(ProfessionId).Select(prof => new BusinessList
        {
            BusinessId = prof.Id,
            BusinessName = prof.Vendorname
        });
        return businessLists;
    }
    public SendOrderViewModel GetBusinessDetails(int businessId)
    {
        Healthprofessional vendor = _dashboardRepo.GetBusinessDetails(businessId);
        SendOrderViewModel SendOrders = new()
        {
            BusinessContact = vendor.Phonenumber,
            BusinessEmail = vendor.Email,
            FaxNumber = vendor.Faxnumber
        };
        return SendOrders;
    }

    public void AddOrderDetails(SendOrderViewModel sendOrders)
    {
        Orderdetail newOrder = new()
        {
            Vendorid = sendOrders.BusinessId,
            Requestid = sendOrders.ReqId,
            Faxnumber = sendOrders.FaxNumber,
            Email = sendOrders.BusinessEmail,
            Businesscontact = sendOrders.BusinessContact,
            Prescription = sendOrders.Prescription,
            Noofrefill = sendOrders.NoOfRefill,
            // Createdby = AdminId
        };
        _dashboardRepo.AddOrderDetails(newOrder);
    }

    public void SetClearCase(int RequestId)
    {
        _dashboardRepo.ChangeStatusOfRequest(RequestId, 10);
    }

    public void SetTransferCase(int reqId, int oldphyId, int physician, string description)
    {
        _dashboardRepo.AddPhysicianToRequest(reqId, physician);
        _dashboardRepo.AddStatusLog(reqId, 2, 2, description, null, null, physician);
    }

    public void StoreAcceptToken(int reqId, string token, DateTime expirationTime)
    {
        _dashboardRepo.StoreAcceptToken(reqId, token, expirationTime);
    }
    public void AgreementAccept(int reqId)
    {
        _dashboardRepo.AgreementAccept(reqId);
        short newStatus = 4;
        short oldStatus = _dashboardRepo.GetStatusOfRequest(reqId);
        _dashboardRepo.AddStatusLog(reqId, newStatus, oldStatus, null, null, null, null);
    }
    public void AgreementReject(int reqId, string reason)
    {
        _dashboardRepo.AgreementReject(reqId);
        short newStatus = 7;
        short oldStatus = _dashboardRepo.GetStatusOfRequest(reqId);
        _dashboardRepo.AddStatusLog(reqId, newStatus, oldStatus, reason, null, null, null);
    }

    public CloseCaseViewModel CloseCase(int RequestId)
    {
        var PatientData = _dashboardRepo.GetSingleRequest(RequestId);
        var DocumentRecords = _patientDashboardRepo.GetAllRequestedDocuments(RequestId);

        IEnumerable<ViewDocuments> viewModel = DocumentRecords.Select(d => new ViewDocuments
        {
            DocumentId = d.Id,
            FilePath = d.Filename,
            FileName = Path.GetFileName(d.Filename),
            UploaderName = d.Request.Createduser != null ? d.Request.Createduser.Email : d.Request.Firstname,
            UploadDate = d.Createddate.ToString("yyyy-MM-dd"),
            PatientName = d.Request.User != null ? (d.Request.User.Firstname + " " + d.Request.User.Lastname) :
                (d.Request.Requestclients.FirstOrDefault()?.Firstname.ToUpper() + " " + d.Request.Requestclients.FirstOrDefault()?.Lastname.ToUpper())
        }).ToList();

        int month = DateTime.ParseExact(PatientData.Requestclients.FirstOrDefault().Strmonth, "MMMM", CultureInfo.InvariantCulture).Month;
        DateTime dt = new DateTime((int)PatientData.Requestclients.FirstOrDefault().Intyear, month, (int)PatientData.Requestclients.FirstOrDefault().Intdate);

        CloseCaseViewModel CloseCaseView = new()
        {
            ReqId = PatientData.Id,
            PatientId = PatientData.Requestclients.FirstOrDefault().Id,
            Firstname = PatientData.Requestclients.FirstOrDefault().Firstname,
            Lastname = PatientData.Requestclients.FirstOrDefault().Lastname,
            Email = PatientData.Requestclients.FirstOrDefault().Email,
            Phone = PatientData.Requestclients.FirstOrDefault().Phonenumber,
            DateOfBirth = dt.ToString("yyyy-MM-dd"),
            documentList = viewModel
        };



        return CloseCaseView;
    }

    public void EditPatientInfo(string Email, string Phone, int patientId, int requestId)
    {
        _dashboardRepo.EditPatientInfo(Email, Phone, patientId, requestId);
    }

    public void CloseCaseSubmit(int reqId)
    {
        _dashboardRepo.ChangeStatusOfRequest(reqId, 9);
    }



    public EncounterFormViewModel GetEncounterDetails(int reqId)
    {
        Request encounterData = _dashboardRepo.GetEncounterDetails(reqId);
        DateTime date = DateTime.ParseExact(encounterData?.Requestclients?.FirstOrDefault()?.Strmonth, "MMMM", CultureInfo.InvariantCulture);
        int year = encounterData?.Requestclients?.FirstOrDefault()?.Intyear ?? 0000;
        int day = encounterData.Requestclients.FirstOrDefault().Intdate ?? 1;
        date = new DateTime(year, date.Month, day);
        DateTime createdDate = (DateTime)encounterData.Createdat;

        EncounterFormViewModel encounterFormViewModel = new()
        {
            ReqId = encounterData.Id,
            FirstName = encounterData.Requestclients.FirstOrDefault()?.Firstname,
            LastName = encounterData.Requestclients.FirstOrDefault()?.Lastname,
            Location = encounterData.Requestclients.FirstOrDefault()?.Street + ", " + encounterData.Requestclients.FirstOrDefault()?.City + ", " + encounterData.Requestclients.FirstOrDefault()?.State + ", " + encounterData.Requestclients.FirstOrDefault()?.Zipcode,
            DateOfBirth = date.ToString("yyyy-MM-dd"),
            Mobile = encounterData.User.Mobile,
            Email = encounterData.User.Email,
            Date = createdDate.ToString("yyyy-MM-dd")
        };
        if (encounterData.Encounterform != null)
        {
            encounterFormViewModel = new EncounterFormViewModel()
            {
                ReqId = encounterData.Id,
                FirstName = encounterData.Requestclients.FirstOrDefault()?.Firstname,
                LastName = encounterData.Requestclients.FirstOrDefault()?.Lastname,
                Location = encounterData.Requestclients.FirstOrDefault()?.Street + ", " + encounterData.Requestclients.FirstOrDefault()?.City + ", " + encounterData.Requestclients.FirstOrDefault()?.State + ", " + encounterData.Requestclients.FirstOrDefault()?.Zipcode,
                DateOfBirth = date.ToString("yyyy-MM-dd"),
                Mobile = encounterData.User.Mobile,
                Email = encounterData.User.Email,
                Date = createdDate.ToString("yyyy-MM-dd"),
                Id = encounterData.Encounterform.Id,
                HistoryOfPresentIllness = encounterData.Encounterform.Historyofpresentillness,
                MedicalHistory = encounterData.Encounterform.Medicalhistory,
                Medications = encounterData.Encounterform.Medications,
                Allergies = encounterData.Encounterform.Allergies,
                Temperature = encounterData.Encounterform.Temperature,
                HeartRate = encounterData.Encounterform.Heartrate,
                RespiratoryRate = encounterData.Encounterform.Respiratoryrate,
                BloodPressureDBP = encounterData.Encounterform.Bloodpressure.Split("/")[1],
                BloodPressureSBP = encounterData.Encounterform.Bloodpressure.Split("/")[0],
                O2 = encounterData.Encounterform.O2,
                Pain = encounterData.Encounterform.Pain,
                HEENT = encounterData.Encounterform.Heent,
                CV = encounterData.Encounterform.Cv,
                Chest = encounterData.Encounterform.Chest,
                ABD = encounterData.Encounterform.Abd,
                Extr = encounterData.Encounterform.Extr,
                Skin = encounterData.Encounterform.Skin,
                Neuro = encounterData.Encounterform.Neuro,
                Other = encounterData.Encounterform.Other,
                Diagnosis = encounterData.Encounterform.Diagnosis,
                TreatmentPlan = encounterData.Encounterform.Treatmentplan,
                MedicationDispensed = encounterData.Encounterform.Medicationdispensed,
                Procedures = encounterData.Encounterform.Procedures,
                FollowUp = encounterData.Encounterform.Followup,
                IsFinalized = encounterData.Encounterform.Isfinalized
            };
        }
        return encounterFormViewModel;
    }

    public void ConsultEncounter(int reqId)
    {
        _dashboardRepo.ChangeStatusOfRequest(reqId, 6);
        _dashboardRepo.AddCallType(1, reqId); // Consult
    }

    public void HouseCallEncounter(int reqId, string status)
    {
        Console.WriteLine("In Complete" + status + "<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
        if (status == "onroute")
        {
            _dashboardRepo.ChangeStatusOfRequest(reqId, 5);
        }
        else if (status == "complete")
        {
            _dashboardRepo.ChangeStatusOfRequest(reqId, 6);
        }
        _dashboardRepo.AddCallType(0, reqId); // House-call
    }

    public void SubmitEncounter(EncounterFormViewModel encounterForm)
    {

        string BloodPressureVal = encounterForm.BloodPressureSBP != null && encounterForm.BloodPressureDBP != null ? encounterForm.BloodPressureSBP + "/" + encounterForm.BloodPressureDBP : "0/0";

        Encounterform newEncounterForm = new()
        {
            Id = encounterForm.Id,
            RequestId = encounterForm.ReqId,
            Historyofpresentillness = encounterForm.HistoryOfPresentIllness,
            Medicalhistory = encounterForm.MedicalHistory,
            Medications = encounterForm.Medications,
            Allergies = encounterForm.Allergies,
            Temperature = encounterForm.Temperature,
            Heartrate = encounterForm.HeartRate,
            Respiratoryrate = encounterForm.RespiratoryRate,
            Bloodpressure = BloodPressureVal,
            O2 = encounterForm.O2,
            Pain = encounterForm.Pain,
            Heent = encounterForm.HEENT,
            Cv = encounterForm.CV,
            Chest = encounterForm.Chest,
            Abd = encounterForm.ABD,
            Extr = encounterForm.Extr,
            Skin = encounterForm.Skin,
            Neuro = encounterForm.Neuro,
            Other = encounterForm.Other,
            Diagnosis = encounterForm.Diagnosis,
            Treatmentplan = encounterForm.TreatmentPlan,
            Medicationdispensed = encounterForm.MedicationDispensed,
            Procedures = encounterForm.Procedures,
            Followup = encounterForm.FollowUp
        };
        _dashboardRepo.SubmitEncounter(newEncounterForm);
    }

    public IEnumerable<Request> FetchAllRequest()
    {
        return _dashboardRepo.FetchAllRequest();
    }

    public void CreateRequest(PatientRequestViewModel patientRequest, int AccountType = 1)
    {
        var userEmail = _patientRequestRepo.FindUserByEmail(patientRequest.Email);
        if (userEmail != null)
        {
            ProcessExistingUserRequestAsync(patientRequest, AccountType);
        }
        else
        {
            ProcessNewUserRequestAsync(patientRequest, AccountType);
        }
    }

    private void ProcessExistingUserRequestAsync(PatientRequestViewModel viewRequest, int AccountType = 1)
    {
        var existUserData = _patientRequestRepo.FindUserByEmailFromUser(viewRequest.Email);
        var RegionDetails = _patientRequestRepo.GetSingleRegion((int)viewRequest.State);
        int PhyId = 0;
        if (AccountType == 2)
        {
            Physician physicianData = _providerRepo.GetAllPhysician().FirstOrDefault(phy => phy.Aspnetuserid == viewRequest.CreatedById);
            PhyId = physicianData.Id;
        }
        AdminTable? CreatedUserDetails = _dashboardRepo.GetAdminFromAsp(viewRequest.CreatedById);
        var newRequestForExistedUser = new Request
        {
            Userid = existUserData.Id,
            Roomnoofpatient = viewRequest.Roomnoofpatient,
            Status = (short)(AccountType == 2 ? 2 : 1),
            Firstname = CreatedUserDetails?.Firstname ?? "",
            Lastname = CreatedUserDetails?.Lastname ?? "",
            Phonenumber = CreatedUserDetails?.Mobile ?? "",
            Email = CreatedUserDetails?.Email ?? "",
            Requesttypeid = 4,
            Createduserid = viewRequest.CreatedById,
            Physicianid = AccountType == 2 ? PhyId : null
        };
        _patientRequestRepo.AddRequestDataForExistedUser(newRequestForExistedUser);

        var patientInfo = new Requestclient
        {
            Requestid = newRequestForExistedUser.Id,
            Firstname = viewRequest.Firstname,
            Lastname = viewRequest.Lastname,
            Phonenumber = viewRequest.Mobile,
            Email = viewRequest.Email,
            Street = viewRequest.Street,
            City = viewRequest.City,
            State = RegionDetails.Name,
            Regionid = viewRequest.State,
            Zipcode = viewRequest.Zipcode,
            Strmonth = DateOnly.Parse(viewRequest.Birthdate).ToString("MMMM"),
            Intyear = DateOnly.Parse(viewRequest.Birthdate).Year,
            Intdate = DateOnly.Parse(viewRequest.Birthdate).Day
        };
        _patientRequestRepo.AddPatientInfoForExistedUser(patientInfo);

        if (viewRequest.Symptoms != null && !string.IsNullOrEmpty(viewRequest.Symptoms))
        {
            if(AccountType==1) _dashboardRepo.SaveAdditionalNotes(viewRequest.Symptoms ?? "", 0, newRequestForExistedUser.Id);
            else if(AccountType == 2) _dashboardRepo.SaveAdditionalNotes(viewRequest.Symptoms ?? "", 0, newRequestForExistedUser.Id , 2);
        }
    }

    private void ProcessNewUserRequestAsync(PatientRequestViewModel viewRequest, int AccountType = 1)
    {
        string email = viewRequest.Email;
        string[] parts = email.Split('@');
        string userName = parts[0];
        var RegionDetails = _patientRequestRepo.GetSingleRegion((int)viewRequest.State);
        AdminTable? CreatedUserDetails = _dashboardRepo.GetAdminFromAsp(viewRequest.CreatedById);
        int PhyId = 0;
        if (AccountType == 2)
        {
            Physician physicianData = _providerRepo.GetAllPhysician().FirstOrDefault(phy => phy.Aspnetuserid == viewRequest.CreatedById);
            PhyId = physicianData.Id;
        }
        var newUser = new Aspnetuser
        {
            Username = userName,
            Email = viewRequest.Email,
            Passwordhash = viewRequest.Passwordhash,
            Phonenumber = viewRequest.Mobile,
        };
        _patientRequestRepo.NewAspUserAdd(newUser);

        var newPatient = new User
        {
            Aspnetuserid = newUser.Id,
            Firstname = viewRequest.Firstname,
            Lastname = viewRequest.Lastname,
            Email = viewRequest.Email,
            Mobile = viewRequest.Mobile,
            Street = viewRequest.Street,
            City = viewRequest.City,
            State = RegionDetails?.Name,
            Regionid = viewRequest.State,
            Zipcode = viewRequest.Zipcode,
            Birthdate = DateOnly.Parse(viewRequest.Birthdate),
            Createddate = DateTime.Now
        };
        _patientRequestRepo.NewUserAdd(newPatient);

        Aspnetuserrole newRole = new()
        {
            Userid = newUser.Id,
            Roleid = 3 //Patient
        };
        _patientRequestRepo.NewRoleAdded(newRole);


        var newRequest = new Request
        {
            Userid = newPatient.Id,
            Roomnoofpatient = viewRequest.Roomnoofpatient,
            Status = (short)(AccountType == 2 ? 2 : 1),
            Firstname = CreatedUserDetails?.Firstname ?? "",
            Lastname = CreatedUserDetails?.Lastname ?? "",
            Phonenumber = CreatedUserDetails?.Mobile ?? "",
            Email = CreatedUserDetails?.Email ?? "",
            Requesttypeid = 4,
            Createduserid = viewRequest.CreatedById,
            Confirmationnumber = _patientRequestService.CreateConfirmation(viewRequest.State, viewRequest.Firstname, viewRequest?.Lastname),
            Physicianid = AccountType == 2 ? PhyId : null
        };
        _patientRequestRepo.NewRequestAdd(newRequest);

        var newPatientInfo = new Requestclient
        {
            Requestid = newRequest.Id,
            Firstname = viewRequest.Firstname,
            Lastname = viewRequest.Lastname,
            Phonenumber = viewRequest.Mobile,
            Email = viewRequest.Email,
            Street = viewRequest.Street,
            City = viewRequest.City,
            State = RegionDetails.Name,
            Regionid = viewRequest.State,
            Zipcode = viewRequest.Zipcode,
            Strmonth = DateOnly.Parse(viewRequest.Birthdate).ToString("MMMM"),
            Intyear = DateOnly.Parse(viewRequest.Birthdate).Year,
            Intdate = DateOnly.Parse(viewRequest.Birthdate).Day
        };
        _patientRequestRepo.NewPatientAdd(newPatientInfo);

        if (viewRequest.Symptoms != null && !string.IsNullOrEmpty(viewRequest.Symptoms))
        {
            if(AccountType==1) _dashboardRepo.SaveAdditionalNotes(viewRequest.Symptoms ?? "", 0, newRequest.Id);
            else if(AccountType == 2) _dashboardRepo.SaveAdditionalNotes(viewRequest.Symptoms ?? "", 0, newRequest.Id , 2);
        }

    }


}
