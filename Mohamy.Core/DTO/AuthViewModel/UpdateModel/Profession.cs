namespace Mohamy.Core.DTO.AuthViewModel.UpdateModel
{
    public class ProfessionDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class CreateProfessionDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
