namespace HalloDocService.ViewModels
{
    public class PayrateViewModel
    {
        public int Id { get; set; }
        public int Physicianid { get; set; }
        public decimal? Nightshiftweekend { get; set; } = 0;
        public decimal? Shift { get; set; } = 0;
        public decimal? Housecallnightweekend { get; set; } = 0;
        public decimal? Phoneconsult { get; set; } = 0;
        public decimal? Phoneconsultnightweekend { get; set; } = 0;
        public decimal? Batchtesting { get; set; } = 0;
        public decimal? Housecall { get; set; } = 0;
    }
}