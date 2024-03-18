using System;

namespace HalloDocService.ViewModels
{
    public class EncounterFormViewModel
    {
        public int Id {get; set;}
        public int ReqId {get; set;}
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public string DateOfBirth { get; set; }
        public string Date { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string HistoryOfPresentIllness { get; set; }
        public string MedicalHistory { get; set; }
        public string Medications { get; set; }
        public string Allergies { get; set; }
        public decimal? Temperature { get; set; }
        public int? HeartRate { get; set; }
        public int? RespiratoryRate { get; set; }
        public string BloodPressureSBP { get; set; }
        public string BloodPressureDBP { get; set; }
        public int? O2 { get; set; }
        public int? Pain { get; set; }
        public string HEENT { get; set; }
        public string CV { get; set; }
        public string Chest { get; set; }
        public string ABD { get; set; }
        public string Extr { get; set; }
        public string Skin { get; set; }
        public string Neuro { get; set; }
        public string Other { get; set; }
        public string Diagnosis { get; set; }
        public string TreatmentPlan { get; set; }
        public string MedicationDispensed { get; set; }
        public string Procedures { get; set; }
        public string FollowUp { get; set; }
    }
}
