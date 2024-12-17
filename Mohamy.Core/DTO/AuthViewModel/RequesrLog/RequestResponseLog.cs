namespace Mohamy.Core.DTO.AuthViewModel.RequesrLog
{
    public class RequestResponseLog
    {
        public int id { get; set; }
        public string RequestUrl { get; set; }
        public string HttpMethod { get; set; }
        public string RequestBody { get; set; }
        public string ResponseBody { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}