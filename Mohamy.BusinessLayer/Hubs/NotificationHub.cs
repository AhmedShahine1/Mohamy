using Microsoft.AspNetCore.SignalR;
using Mohamy.BusinessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mohamy.BusinessLayer.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly INotificationService _notificationService;

        public NotificationHub(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// Sends a notification message to a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user to send the notification to.</param>
        /// <param name="message">The notification message.</param>
        /// <returns>A Task that represents the asynchronous operation.</returns>
        public async Task SendNotification(string userId, string message)
        {
            await _notificationService.CreateNotificationAsync(userId, message);
            await Clients.User(userId).SendAsync("ReceiveNotification", message);
        }

        /// <summary>
        /// Sends a notification message to all connected users.
        /// </summary>
        /// <param name="message">The notification message.</param>
        /// <returns>A Task that represents the asynchronous operation.</returns>
        public async Task SendNotificationToAll(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }

        /// <summary>
        /// Retrieves all notifications for a user.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve notifications for.</param>
        /// <returns>A Task that represents the asynchronous operation.</returns>
        public async Task GetNotifications(string userId)
        {
            var notifications = await _notificationService.GetNotificationsAsync(userId);
            await Clients.Caller.SendAsync("ReceiveNotifications", notifications);
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
