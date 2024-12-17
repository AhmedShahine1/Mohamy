namespace Mohamy.Core.DTO.AuthViewModel.UpdateModel
{
    public class GraduationCertificateDTO
    {
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public string Collage { get; set; }
        public string University { get; set; }
    }
}