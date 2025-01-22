using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.DTO.NotificationViewModel;
using Mohamy.RepositoryLayer.Interfaces;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;
using Mohamy.Core.DTO;
using System.IO;
using Mohamy.Core.Helpers;


namespace Mohamy.BusinessLayer.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            //if (FirebaseApp.DefaultInstance is null)
            //{
            //    FirebaseApp.Create(new AppOptions()
            //    {
            //        Credential = GoogleCredential.FromFile("FirebaseKeys/muham.json")
            //    });
            //}
        }

        public async Task SaveNotificationAsync(SaveNotificationDTO saveNotificationDTO)
        {
            saveNotificationDTO.Title = GetNotificationTitle(saveNotificationDTO.NotificationType);
            saveNotificationDTO.Message = GetNotificationMessage(saveNotificationDTO.NotificationType);

            var notification = new Core.Entity.Notification.Notification
            {
                UserId = saveNotificationDTO.UserId,
                Message = saveNotificationDTO.Message,
                NotificationType = saveNotificationDTO.NotificationType,
                ActionId = saveNotificationDTO.ActionId
            };

            await _unitOfWork.NotificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            var user = await _unitOfWork.UserRepository.FindAsync(u => u.Id == saveNotificationDTO.UserId);
            if (user == null) throw new ArgumentException("User not found");
            saveNotificationDTO.DeviceId = user.Device;
            //await SendNotificationAsync(saveNotificationDTO);
        }

        private async Task<ActionResult<BaseResponse>> SendNotificationAsync(SaveNotificationDTO saveNotificationDTO)
        {
            var response = new BaseResponse();
            try
            {
                if (saveNotificationDTO.DeviceId is not null)
                {
                    FirebaseAdmin.Messaging.Notification notification = new FirebaseAdmin.Messaging.Notification();
                    
                    notification = new FirebaseAdmin.Messaging.Notification()
                    {
                        Title = saveNotificationDTO.Title,
                        Body = saveNotificationDTO.Message
                    };
                    
                    Message message = new()
                    {
                        Data = new Dictionary<string, string>
                        {
                            { "title", notification.Title },
                            { "body", notification.Body },
                        },
                        Token = saveNotificationDTO.DeviceId,
                    };

                    await FirebaseMessaging.DefaultInstance.SendAsync(message);
                    response.status = true;
                    response.ErrorCode = 200;
                    response.Data = "Notification sent successfully";
                }
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = $"An error occurred while sending notification: {ex.Message}";
            }
            return response;
        }

        private string GetNotificationTitle(NotificationType notificationType)
        {
            return notificationType switch
            {
                NotificationType.Message => "New Message Received",
                NotificationType.OfferReceived => "Offer Received",
                NotificationType.OfferApproved => "Offer Approved",
                NotificationType.OfferRejected => "Offer Rejected",
                NotificationType.ConsultationStarted => "Consultation Started",
                NotificationType.ConsultationCompleted => "Consultation Completed",
                NotificationType.NewRating => "New Rating Added",
                _ => "Notification",
            };
        }

        private string GetNotificationMessage(NotificationType notificationType)
        {
            return notificationType switch
            {
                NotificationType.Message => "You have received a new message. Check your inbox.",
                NotificationType.OfferReceived => "An offer has been received. Review it now.",
                NotificationType.OfferApproved => "Your offer has been approved. Congratulations!",
                NotificationType.OfferRejected => "Unfortunately, your offer was rejected. Try again!",
                NotificationType.ConsultationStarted => "A consultation has started. Join now.",
                NotificationType.ConsultationCompleted => "The consultation has been successfully completed.",
                NotificationType.NewRating => "You have received a new rating. See how you did.",
                _ => "You have a new notification.",
            };
        }

        public async Task<IEnumerable<NotificationDTO>> GetNotificationsAsync(string userId)
        {
            var notifications = await _unitOfWork.NotificationRepository.FindAllAsync(n => n.UserId == userId);
            return notifications.OrderByDescending(n => n.CreatedAt).Select(n => new NotificationDTO
            {
                Id = n.Id,
                Message = n.Message,
                CreatedAt = n.CreatedAt,
                IsRead = n.IsRead,
                ActionId = n.ActionId,
                NotificationType = n.NotificationType
            });
        }

        public async Task ReadNotificationAsync(string notificationId)
        {
            var notification = await _unitOfWork.NotificationRepository.FindAsync(n => n.Id == notificationId);

            if (notification is null) throw new ArgumentException("Notification not found");

            notification.IsRead = true;
            notification.UpdatedAt = DateTime.Now;

            _unitOfWork.NotificationRepository.Update(notification);
            await _unitOfWork.SaveChangesAsync();
        }


        public async Task CreateNotificationAsync(string userId, string message)
        {
            var notification = new Core.Entity.Notification.Notification
            {
                UserId = userId,
                Message = message
            };

            await _unitOfWork.NotificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }

    }
}
