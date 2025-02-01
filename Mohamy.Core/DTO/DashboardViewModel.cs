namespace Mohamy.Core.DTO
{
    public class DashboardViewModel
    {
        public int TotalLawyers { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalAdmins { get; set; }
        public int TotalRequests { get; set; }
        public int CompletedConsultings { get; set; }
        public int InProgressConsultings { get; set; }
        public int CancelledConsultings { get; set; }
    }
}
