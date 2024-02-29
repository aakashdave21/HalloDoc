function populateCancleCaseModal(patientName,patientLastName,reqId){
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
                                <option value="Illegal Person">Illegal Person</option>
                                <option value="Unhygenic">Unhygenic</option>
                                <option value="Wrong Details">Wrong Details</option>
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

            var modalInstance = new bootstrap.Modal(modal);
            modalInstance.show();
}

function cancleCaseSubmit() {
    let form = document.querySelector('#cancelCaseForm');
    let formData = new FormData(form);
    let reasonCancle = document.getElementById("reasonCancle");
    if(reasonCancle.value == "0"){
        $("#reasonValidation").html("Please Choose Reason For Cancellation");
        return;
    }

    $.ajax({
        url : '/admin/dashboard/canclecase',
        type : 'POST',
        data : formData,
        processData: false,
        contentType: false,
        cache: false,
        success : function () {
            location.href='/admin/dashboard'
        },
        error: function () {
            console.log("error");
        }
    })
}