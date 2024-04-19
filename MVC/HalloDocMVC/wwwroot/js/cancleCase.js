async function populateCancleCaseModal(patientName, patientLastName, reqId) {
    var modal = document.getElementById('cancleCaseModal');
    modal.innerHTML = `
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header" style="background-color: #01bce9;">
                        <h1 class="modal-title fs-5 text-light" id="exampleModalLabel">Confirmation Cancelation</h1>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <h5>Patient Name: <span class="patient-name-modal" style="color : #01bce9;">${patientName} ${patientLastName}</span></h5>
                        <form id="cancelCaseForm">
                            <input type="hidden" name="reqId" value="${reqId}">
                            <select class="form-select" name="reason" id="reasonCancle" aria-label="Default select example">
                                <option selected disabled value="0">Reason For Cancelation</option>
                            </select>
                            <span class="text-danger" id="reasonValidation"></span>
                            <div class="form-floating mt-2">
                                <textarea class="form-control" name="additionalNotes" placeholder="Leave a comment here" id="Symptoms" style="height: 100px"></textarea>
                                <label for="Symptoms" style="white-space: normal;">Please Provide Additional Notes</label>
                            </div>
                        </form>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn secondary-theme-btn" onclick="cancleCaseSubmit()">Confirm</button>
                        <button type="button" class="btn theme-btn" data-bs-dismiss="modal">Cancel</button>
                    </div>
                </div>
            </div>`;

    try {
        // GET Casetag From Database
        const casetags = await $.get("/admin/dashboard/GetCaseTag");
        const reasonCancleDropdown = $("#reasonCancle");
        reasonCancleDropdown.empty();
        reasonCancleDropdown.append(`<option value="" selected disabled>Choose Region</option>`);
        casetags.forEach(casetag => {
            reasonCancleDropdown.append(`<option value="${casetag.name}">${casetag.name}</option>`);
        });
    } catch (error) {
        console.error("Error loading regions:", error);
    } finally {
        var modalInstance = new bootstrap.Modal(modal);
        modalInstance.show();
    }
}

function cancleCaseSubmit() {
    let form = document.querySelector('#cancelCaseForm');
    let formData = new FormData(form);
    let reasonCancle = document.getElementById("reasonCancle");
    if (reasonCancle.value == "0") {
        $("#reasonValidation").html("Please Choose Reason For Cancellation");
        return;
    }

    $.ajax({
        url: '/admin/dashboard/canclecase',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        cache: false,
        success: function () {
            location.href = '/admin/dashboard'
        },
        error: function (xhr, status, error) {
            if (xhr.status == 401) {
                window.location.href = '/admin/login';
            }
        }
    })
}


// Assign Case Javascript
const populateAssignCaseModal = async (reqId) => {
    var modal = document.getElementById('assignCaseModal');
    modal.innerHTML = `
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-header" style="background-color: #01bce9;">
                <h1 class="modal-title fs-5 text-light" id="exampleModalLabel">Assign Request</h1>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
                <form id="assignCaseForm">
                    <input type="hidden" name="reqId" value="${reqId}">
                    <small class="text-muted">To assign this request, search and select physician.</small>
                    <select class="form-select" name="region" id="regionSelectDropdown" onchange="getPhysicianByRegion()" aria-label="Default select example">
                        <option>Loading...</option>
                    </select>
                    <span class="text-danger" id="regionValidation"></span>
                    <select class="form-select mt-2" name="physician" id="physicianSelectDropdown" aria-label="Default select example" disabled>
                        <option value="" selected>Select Physician</option>
                    </select>
                    <span class="text-danger" id="physicianValidation"></span>
                    <div class="form-floating mt-2">
                        <textarea class="form-control" name="description" placeholder="Leave a comment here" id="Symptoms" style="height: 100px"></textarea>
                        <label for="Symptoms" style="white-space: normal;">Description</label>
                    </div>
                </form>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn secondary-theme-btn" onclick="assignCaseSubmit()">Confirm</button>
                <button type="button" class="btn theme-btn" data-bs-dismiss="modal">Cancel</button>
            </div>
        </div>
    </div>`;

    try {
        // GET Regions From Database
        const regions = await $.get("/admin/dashboard/GetRegions");
        const regionDropdown = $("#regionSelectDropdown");
        regionDropdown.empty();
        regionDropdown.append(`<option value="" selected disabled>Choose Region</option>`);
        regions.forEach(region => {
            regionDropdown.append(`<option value="${region.id}">${region.name}</option>`);
        });
    } catch (error) {
        console.error("Error loading regions:", error);
    } finally {
        var modalInstance = new bootstrap.Modal(modal);
        modalInstance.show();
    }
}

const getPhysicianByRegion = async (physicianId = null) => {
    try {
        $("#physicianSelectDropdown").removeAttr("disabled");
        const regionValue = $("#regionSelectDropdown").val();
        const physicians = await $.get('/admin/dashboard/GetPhysicians', {
            RegionId: regionValue
        })
        let PhysicianDropdown = $("#physicianSelectDropdown")
        PhysicianDropdown.empty();
        PhysicianDropdown.append(`<option value="" selected disabled>Choose Physician</option>`);
        console.log(physicians);
        physicians.forEach(phy => {
            if (physicianId != phy.id) {
                PhysicianDropdown.append(`<option value="${phy.id}">${phy.id}. ${phy.firstname}</option>`);
            }
        });
    } catch (error) {
        console.error("Error loading Physician:", error)
    }
}

const assignCaseSubmit = () => {
    try {
        let form = document.querySelector('#assignCaseForm');
        let formData = new FormData(form);
        const regionDropdown = document.getElementById("regionSelectDropdown").value;
        const physicianDropdown = document.getElementById("physicianSelectDropdown").value;
        if (regionDropdown == "" || regionDropdown == null) {
            $("#regionValidation").html("Please Choose Region");
        } else {
            $("#regionValidation").empty();
        }
        if (physicianDropdown == "" || physicianDropdown == null) {
            $("#physicianValidation").html("Please Choose Physician");
            return;
        } else {
            $("#physicianValidation").empty();
        }
        $.ajax({
            url: '/admin/dashboard/assigncase',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            cache: false,
            success: function () {
                location.href = '/admin/dashboard'
            },
            error: function (xhr, status, error) {
                if (xhr.status == 401) {
                    window.location.href = '/admin/login';
                }
            }
        })

    } catch (error) {
        console.error("Error Submitting:", error)
    }
}



// Block Case Javascript
const populateBlockCaseModal = async (reqId, firstname, lastname) => {
    var modal = document.getElementById('blockCaseModal');
    modal.innerHTML = `<div class="modal-dialog modal-dialog-centered">
    <div class="modal-content">
        <div class="modal-header" style="background-color: #01bce9;">
            <h1 class="modal-title fs-5 text-light" id="exampleModalLabel">Confirm Block</h1>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
        </div>
        <div class="modal-body">
            <form id="blockCaseForm">
                <input type="hidden" name="reqId" value="${reqId}">
                <h5>Patient Name : <span class="patient-name-modal" style="color : #01bce9;">${firstname} ${lastname}</span></h5>
                <div class="form-floating mt-2">
                    <textarea class="form-control" name="reason" id="block-reason" placeholder="Leave a comment here" id="reason"
                        style="height: 100px"></textarea>
                    <label for="reason" style="white-space: normal;">Reason For Block Request</label>
                </div>
                <span class="text-danger" id="blockReasonValidation"></span>
            </form>
        </div>
        <div class="modal-footer">
            <button type="button" class="btn secondary-theme-btn" onclick="blockCaseSubmit()">Confirm</button>
            <button type="button" class="btn theme-btn" data-bs-dismiss="modal">Cancle</button>
        </div>
    </div>
</div>`

    var modalInstance = new bootstrap.Modal(modal);
    modalInstance.show();
}

const blockCaseSubmit = () => {
    try {
        let form = document.querySelector('#blockCaseForm');
        let formData = new FormData(form);

        if ($("#block-reason").val() == "") {
            $("#blockReasonValidation").html("Please Provide Reason For Block.");
            return;
        }
        $("#blockReasonValidation").empty();
        $.ajax({
            url: '/admin/dashboard/blockcase',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            cache: false,
            success: function () {
                location.href = '/admin/dashboard'
            },
            error: function (xhr, status, error) {
                if (xhr.status == 401) {
                    window.location.href = '/admin/login';
                }
            }
        })
    } catch (error) {
        console.error("Error Submitting:", error)
    }
}



// CLEAR CASE
const populateClearCase = async (reqId) => {
    var modal = document.getElementById('clearCaseModal');
    modal.innerHTML = `<div class="modal-dialog modal-dialog-centered">
                            <div class="modal-content p-md-5 p-1">
                                <div class="modal-body d-flex flex-column justify-content-center align-items-center">
                                    <div style="
                                        height: 150px;
                                        width: 100%;
                                        display: flex;
                                        justify-content: center;
                                        align-items: center;
                                    ">
                                        <i class="fa-solid fa-circle-info text-warning warning-icon"></i>
                                    </div>
                                    <h5>Confirmation for Clear Case</h5>
                                    <p class="mt-2">Are you sure want to clear this request? Once clear this request you are not able to
                                        see this request. </p>
                                    <div class="d-flex">
                                        <button type="button" class="btn secondary-theme-btn me-2" onclick='clearCase(${reqId})'>Clear</button>
                                        <button type="button" class="btn theme-btn" data-bs-dismiss="modal">Cancle</button>
                                    </div>
                                </div>
                            </div>
                        </div>`;
    var modalInstance = new bootstrap.Modal(modal);
    modalInstance.show();
}
const clearCase = (reqId) => {
    try {
        $.ajax({
            url: `/admin/dashboard/clearcase?RequestId=${reqId}`,
            type: 'POST',
            processData: false,
            contentType: false,
            cache: false,
            success: function () {
                location.href = '/admin/dashboard'
            },
            error: function (xhr, status, error) {
                if (xhr.status == 401) {
                    window.location.href = '/admin/login';
                }
            }
        })
    } catch (error) {
        console.error("Error Submitting:", error)
    }
}

// Transfer Case
const populateTransferCase = async (reqId, physicianId) => {
    var modal = document.getElementById('assignCaseModal');
    modal.innerHTML = `
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-header" style="background-color: #01bce9;">
                <h1 class="modal-title fs-5 text-light" id="exampleModalLabel">Transfer Request</h1>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
                <form id="transferCaseForm">
                    <input type="hidden" name="reqId" value="${reqId}">
                    <input type="hidden" name="phyId" value="${physicianId}">
                    <small class="text-muted">To assign this request, search and select physician.</small>
                    <select class="form-select" name="region" id="regionSelectDropdown" onchange="getPhysicianByRegion(${physicianId})" aria-label="Default select example">
                        <option>Loading...</option>
                    </select>
                    <span class="text-danger" id="regionTransferValidation"></span>
                    <select class="form-select mt-2" name="physician" id="physicianSelectDropdown" aria-label="Default select example" disabled>
                        <option value="" selected>Select Physician</option>
                    </select>
                    <span class="text-danger" id="physicianTransferValidation"></span>
                    <div class="form-floating mt-2">
                        <textarea class="form-control" name="description" placeholder="Leave a comment here" id="Symptoms" style="height: 100px"></textarea>
                        <label for="Symptoms" style="white-space: normal;">Description</label>
                    </div>
                </form>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn secondary-theme-btn" onclick="transferCaseSubmit()">Confirm</button>
                <button type="button" class="btn theme-btn" data-bs-dismiss="modal">Cancel</button>
            </div>
        </div>
    </div>`;

    try {
        // GET Regions From Database
        const regions = await $.get("/admin/dashboard/GetRegions");
        const regionDropdown = $("#regionSelectDropdown");
        regionDropdown.empty();
        regionDropdown.append(`<option value="" selected disabled>Choose Region</option>`);
        regions.forEach(region => {
            regionDropdown.append(`<option value="${region.id}">${region.name}</option>`);
        });
    } catch (error) {
        console.error("Error loading regions:", error);
    } finally {
        var modalInstance = new bootstrap.Modal(modal);
        modalInstance.show();
    }
}

const transferCaseSubmit = () => {
    try {
        let form = document.querySelector('#transferCaseForm');
        let formData = new FormData(form);
        const regionTransferDropdown = document.getElementById("regionSelectDropdown").value;
        const physicianTransferDropdown = document.getElementById("physicianSelectDropdown").value;
        if (regionTransferDropdown == "" || regionTransferDropdown == null) {
            $("#regionTransferValidation").html("Please Choose Region");
        } else {
            $("#regionTransferValidation").empty();
        }
        if (physicianTransferDropdown == "" || physicianTransferDropdown == null) {
            $("#physicianTransferValidation").html("Please Choose Physician");
            return;
        } else {
            $("#physicianTransferValidation").empty();
        }
        $.ajax({
            url: '/admin/dashboard/transfercase',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            cache: false,
            success: function () {
                location.href = '/admin/dashboard'
            },
            error: function (xhr, status, error) {
                if (xhr.status == 401) {
                    window.location.href = '/admin/login';
                }
            }
        })

    } catch (error) {
        console.error("Error Submitting:", error)
    }
}

// Provider Transfer Case
const populatePhysicianTransferCase = async (reqId) => {
    var modal = document.getElementById('assignCaseModal');
    modal.innerHTML = `
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-header" style="background-color: #01bce9;">
                <h1 class="modal-title fs-5 text-light" id="exampleModalLabel">Transfer Request To Admin</h1>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
                    <small class="text-muted">This Request will be transferred to Admin.</small>
                    <div class="form-floating mt-2">
                        <textarea class="form-control" id="ProviderDescription" placeholder="Description"  style="height: 150px; resize:none;"></textarea>
                        <label for="ProviderDescription" style="white-space: normal;">Description</label>
                    </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn secondary-theme-btn" onclick="transferReqToAdmin(${reqId})">Confirm</button>
                <button type="button" class="btn theme-btn" data-bs-dismiss="modal">Cancel</button>
            </div>
        </div>
    </div>`;
    var modalInstance = new bootstrap.Modal(modal);
    modalInstance.show();
}

const transferReqToAdmin = (reqId) => {
    const description = $("#ProviderDescription").val();
    $.ajax({
        url: '/Provider/dashboard/transfercase',
        type: 'POST',
        data: {reqId,description},
        success: function () {
            location.reload();
        },
        error: function (xhr, status, error) {
            if (xhr.status == 401) {
                window.location.href = "/Provider/login"
            }
        }
    })
}

// Send Agreement
const populateSendAgreement = (reqId, Email, Phone, Role = 1) => {
    var modal = document.getElementById('sendAgreementModal');
    modal.innerHTML = `
    <div class="modal-dialog modal-dialog-centered">
    <div class="modal-content">
        <div class="modal-header" style="background-color: #01bce9;">
            <h1 class="modal-title fs-5 text-light" id="exampleModalLabel">Send Agreement</h1>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
        </div>
        <form id="sendAgreementForm">
            <div class="modal-body">
                <small class="text-muted">To Send Agreement please make sure you are updating correct contact
                    information below the responsible party</small>
                    <input type="hidden" name="request_id" value="${reqId}" />
                <div class=" mb-3">
                    <div class="form-floating">
                        <input type="email" class="form-control" name="Email" value="${Email}" id="email" placeholder="email">
                        <label for="email">Email</label>
                    </div>
                    <span id="invalid-email" class="text-danger"></span>
                </div>
                <div class=" mb-3">
                    <div class="form-floating">
                        <input id="phone" class="form-control" name="Mobile" value="${Phone}" type="tel" placeholder="phone"/>
                        <label for="phone">Phone</label>
                    </div>
                    <span  id="invalid-phone" class="text-danger" id="mobile-valid"></span>
                </div>
            </div>
            <div class="modal-footer">
                    <button type="button" class="btn secondary-theme-btn" id="submitAgreement" onclick="submitSendAgreement(${Role})">Confirm</button>
                    <button type="button" class="btn theme-btn" id="cancleAgreement" data-bs-dismiss="modal">Cancle</button>
                </div>
        </form>
        
    </div>
</div>`;

    var modalInstance = new bootstrap.Modal(modal);
    modalInstance.show();
}

function submitSendAgreement(Role) {
    try {
        let form = document.querySelector('#sendAgreementForm');
        let formData = new FormData(form);
        if ($("#email").val() == "") {
            $("#invalid-email").html("Email is Required");
            return;
        }
        if ($("#phone").val() == "") {
            $("#invalid-phone").html("Phone is Required");
            return;
        }
        $("#invalid-email").html("");
        $("#invalid-phone").html("");

        $.ajax({
            url: Role == 1 ? '/admin/dashboard/sendAgreement' : '/Provider/dashboard/sendAgreement',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            cache: false,
            success: function () {
                location.reload();
            },
            error: function (xhr, status, error) {
                if (xhr.status == 401) {
                    Role == 1 ? window.location.href = '/admin/login' : window.location.href = '/Provider/login';
                }
            }
        })
        $("#submitAgreement").prop('disabled', true);
        $("#cancleAgreement").prop('disabled', true);
        $("#sendAgreementForm .modal-body").html(`<div class="spinner-grow text-info d-flex m-auto" role="status">
        <span class="visually-hidden">Loading...</span>
      </div>`);

    } catch (error) {
        console.error("Error Submitting:", error)
    }
}

const populateEncounter = (reqId,Role=1) => {

    try {
        var modal = document.getElementById('encounterModal');
        let URL = Role == 2 ? '/Provider/dashboard/GetRequestStatusEncounter' : '/admin/dashboard/GetRequestStatusEncounter';
        sendAjaxRequest(URL, 'GET', {
                requestId: reqId
            }, "application/x-www-form-urlencoded; charset=UTF-8", true)
            .then(function (response) {
                if (response.requestStatus === 5) {
                    modal.innerHTML = `
                <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header" style="background-color: #01bce9;">
                        <h1 class="modal-title fs-5 text-light" id="exampleModalLabel">Encounter</h1>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body" id="encounter-body">
                    <small class="text-muted">A physician is currently on-site. If the consultation has been completed, please click on the "Complete" button below.</small>
                    <button class="btn theme-btn mt-2" onclick="saveEncounterChanges('complete','${reqId}','${Role}')">Complete House-Call</button>
                    </div>
                </div>
            </div>`;
                }

            })
        var buttonClicked = '';


        modal.innerHTML = `
            <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header" style="background-color: #01bce9;">
                    <h1 class="modal-title fs-5 text-light" id="exampleModalLabel">Encounter</h1>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body" id="encounter-body">
                    <button class="btn theme-btn" id="houseCall">House-Call</button>
                    <button class="btn theme-btn" id="consult">Consult</button>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn secondary-theme-btn" id="saveEncounterChanges">Save</button>
                    <button type="button" class="btn theme-btn"data-bs-dismiss="modal">Cancle</button>
                </div>
            </div>
        </div>`;



        var modalInstance = new bootstrap.Modal(modal);
        modalInstance.show();

        $("#encounter-body button").click(function () {
            $("#encounter-body button").removeClass('secondary-theme-btn');
            $(this).addClass('secondary-theme-btn');
            buttonClicked = $(this).attr('id');
        })

        $("#saveEncounterChanges").click(function () {
            if (buttonClicked === "") {
                ToastError("Please Choose Call Type");
                return;
            }
            saveEncounterChanges(buttonClicked, reqId, Role);
            modalInstance.hide();

        });

    } catch (error) {
        console.log(error);
    }


}

const saveEncounterChanges = (buttonValue, requestId, Role=1) => {

    if (buttonValue === "consult") {
        $.ajax({
            type: "POST",
            url: `/${Role==2 ? 'Provider' : 'admin'}/dashboard/ConsultEncounter`,
            data: {
                requestId: requestId
            },
            success: function (response) {
                window.location.href = `/${Role==2 ? 'Provider' : 'admin'}/dashboard/Encounter?requestId=${requestId}`
            },
            error: function (xhr, status, error) {
                if (xhr.status == 401) {
                    window.location.href = `/${Role==2 ? 'Provider' : 'admin'}/login`;
                } else {
                    ToastError("Internal Server Error");
                    console.log(xhr.responseText || error);
                }
            }
        });
    }
    if (buttonValue === "houseCall") {
        sendAjaxRequest(`/${Role==2 ? 'Provider' : 'admin'}/dashboard/houseCallEncounter`, 'POST', {
                requestId: requestId,
                status: "onroute"
            }, "application/x-www-form-urlencoded; charset=UTF-8", true)
            .then(function (response) {
                populateEncounter(requestId,Role);
                console.log(response);
            }).catch(function (error) {
                console.error(error);
            })
    }
    if (buttonValue === "complete") {
        sendAjaxRequest(`/${Role==2 ? 'Provider' : 'Admin'}/dashboard/houseCallEncounter`, 'POST', {
                requestId: requestId,
                status: "complete"
            }, "application/x-www-form-urlencoded; charset=UTF-8", true)
            .then(function (response) {
                location.href = `/${Role==2 ? 'Provider' : 'admin'}/dashboard/Encounter?requestId=${requestId}`
            }).catch(function (error) {
                ToastError("Internal Server Error");
                console.error(error);
            })
    }
}

const populateAcceptModal = (reqId) => {
    var modal = document.getElementById('AcceptPopUp');
    modal.innerHTML = `
    <div class="modal-dialog modal-dialog-centered">
    <div class="modal-content">
        <div class="modal-header" style="background-color: #01bce9;">
            <h1 class="modal-title fs-5 text-light" id="exampleModalLabel">Accept Request</h1>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
        </div>
            <div class="modal-body">
                <small class="text-muted">Are you sure want to Accept this Request?</small>
            </div>
            <div class="modal-footer">
                    <button type="button" class="btn secondary-theme-btn" id="submitAgreement" onclick="submitAcceptRequest(${reqId})">Confirm</button>
                    <button type="button" class="btn theme-btn" id="cancleAgreement" data-bs-dismiss="modal">Cancle</button>
                </div>    
    </div>
</div>`;

    var modalInstance = new bootstrap.Modal(modal);
    modalInstance.show();
}


const submitAcceptRequest = (reqId) => {
    try {
        let form = document.querySelector('#acceptRequestForm');
        $.ajax({
            url: '/provider/dashboard/AcceptRequest',
            type: 'GET',
            data: {
                reqId
            },
            success: function () {
                location.href = '/provider/dashboard'
            },
            error: function (xhr, status, error) {
                if (xhr.status == 401) {
                    window.location.href = '/provider/login';
                }
            }
        })
    } catch (error) {
        console.error("Error Submitting:", error)
    }
}

function populateDownloadEncounter(Id){
    var modal = document.getElementById('encounterModal');
    modal.innerHTML = `
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header" style="background-color: #01bce9;">
                    <h1 class="modal-title fs-5 text-light" id="exampleModalLabel">Encounter</h1>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body" id="encounter-body">
                    <p class="text-muted">Encounter Form is finalized Successfully. You can download it from here.</p>
                    <form id="downloadForm" action="/Provider/Dashboard/DownloadEncounter" method="post">
                        <input type="hidden" name="Id" value="${Id}">
                        <button type="submit" class="btn theme-btn mt-2">Download</button>
                    </form>
                </div>
            </div>
        </div>`;

        var modalInstance = new bootstrap.Modal(modal);
        modalInstance.show();
}