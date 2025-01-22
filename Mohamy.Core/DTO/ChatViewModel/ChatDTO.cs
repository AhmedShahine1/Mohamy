﻿using Microsoft.AspNetCore.Http;

namespace Mohamy.Core.DTO.ChatViewModel
{
    public class ChatDTO
    {
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Message { get; set; }
        public IFormFile? File { get; set; }
        public string? FileUrl { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
    }
}
