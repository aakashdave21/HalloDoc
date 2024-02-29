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
            }finally{
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
        error: function () {
            console.log("error");
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
    }finally{
        var modalInstance = new bootstrap.Modal(modal);
        modalInstance.show();
    }
}

const getPhysicianByRegion = async()=>{
    try {
         $("#physicianSelectDropdown").removeAttr("disabled");
         const regionValue = $("#regionSelectDropdown").val();
         const physicians = await $.get('/admin/dashboard/GetPhysicians',{RegionId : regionValue})
         let PhysicianDropdown = $("#physicianSelectDropdown")
         PhysicianDropdown.empty();
         PhysicianDropdown.append(`<option value="" selected disabled>Choose Physician</option>`);
         physicians.forEach(phy => {
            PhysicianDropdown.append(`<option value="${phy.id}">${phy.firstname}</option>`);
        });
    } catch (error) {
        console.error("Error loading Physician:", error)
    }
}

const assignCaseSubmit = ()=>{
    try {
        let form = document.querySelector('#assignCaseForm');
        let formData = new FormData(form);
        const regionDropdown = document.getElementById("regionSelectDropdown").value;
        const physicianDropdown = document.getElementById("physicianSelectDropdown").value;
        if(regionDropdown == "" || regionDropdown ==null){
            $("#regionValidation").html("Please Choose Region");
        }else{
            $("#regionValidation").empty();
        }
        if(physicianDropdown == "" || physicianDropdown == null){
            $("#physicianValidation").html("Please Choose Physician");
            return;
        }else{
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
            error: function () {
                console.log("error");
            }
        })

    } catch (error) {
        console.error("Error Submitting:", error)
    }
}