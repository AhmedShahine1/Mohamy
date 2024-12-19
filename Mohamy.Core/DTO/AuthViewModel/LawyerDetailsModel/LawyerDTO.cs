namespace Mohamy.Core.DTO.AuthViewModel
{
    public class LawyerDTO
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string? Description { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public double ConsultingPrice { get; set; }
        public double Rating { get; set; }
        public int NumberConsulting { get; set; }
        public string? ProfileImage { get; set; }
    }
}
