//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Mohamy.BusinessLayer.Interfaces;
//using Mohamy.Core.DTO;
//using Mohamy.Core.DTO.ChatViewModel;
//using Mohamy.Core.Entity.ApplicationData;

//namespace Mohamy.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    [Authorize(Policy = "Admin")]
//    public class ChatController : Controller
//    {
//        private readonly IChatService _chatService;
//        private readonly IAccountService _accountService;
//        private readonly UserManager<ApplicationUser> _userManager;

//        public ChatController(IChatService chatService, IAccountService accountService, UserManager<ApplicationUser> userManager)
//        {
//            _chatService = chatService;
//            _accountService = accountService;
//            _userManager = userManager;
//        }

//        public async Task<IActionResult> GetMessages(string customerID,string LawyerID)
//        {
//            try
//            {
//                if (string.IsNullOrEmpty(customerID))
//                {
//                    return BadRequest("Customer ID is required.");
//                }

//                // Retrieve customer and lawyer details
//                var customer = await _accountService.GetUserById(customerID);
//                var lawyer = await _accountService.GetUserById(LawyerID);

//                var chatId = $"{lawyer.PhoneNumber}{customer.PhoneNumber}";
//                var messages = await _chatService.GetMessagesAsync(chatId);

//                // Create the ChatDTO
//                var chatDTO = new ChatDTO
//                {
//                    ChatId = chatId,
//                    LawyerName = lawyer.FullName,
//                    LawyerPhoneNumber = lawyer.PhoneNumber,
//                    CustomerName = customer.FullName,
//                    ProfileimageCustomer = await _accountService.GetUserProfileImage(customer.ProfileId),
//                };

//                return View(chatDTO);
//            }
//            catch (Exception ex)
//            {
//                var errorViewModel = new ErrorViewModel
//                {
//                    Message = "خطأ في جلب البيانات",
//                    StackTrace = ex.Message
//                };
//                return View("~/Views/Shared/Error.cshtml", errorViewModel);
//            }
//        }
//    }
//}
