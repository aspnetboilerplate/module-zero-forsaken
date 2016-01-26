using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;

namespace Abp.Notifications
{
    /// <summary>
    /// Implements <see cref="INotificationStore"/> using repositories.
    /// </summary>
    public class NotificationStore : INotificationStore, ITransientDependency
    {
        private readonly IRepository<NotificationInfo, Guid> _notificationRepository;
        private readonly IRepository<UserNotificationInfo, Guid> _userNotificationRepository;
        private readonly IRepository<NotificationSubscriptionInfo, Guid> _notificationSubscriptionRepository;

        public NotificationStore(
            IRepository<NotificationInfo, Guid> notificationRepository, 
            IRepository<UserNotificationInfo, Guid> userNotificationRepository,
            IRepository<NotificationSubscriptionInfo, Guid> notificationSubscriptionRepository)
        {
            _notificationRepository = notificationRepository;
            _userNotificationRepository = userNotificationRepository;
            _notificationSubscriptionRepository = notificationSubscriptionRepository;
        }

        public Task InsertSubscriptionAsync(NotificationSubscriptionInfo subscription)
        {
            return _notificationSubscriptionRepository.InsertAsync(subscription);
        }

        public Task DeleteSubscriptionAsync(NotificationSubscriptionInfo subscription)
        {
            return _notificationSubscriptionRepository.DeleteAsync(subscription);
        }

        public Task InsertNotificationAsync(NotificationInfo notification)
        {
            return _notificationRepository.InsertAsync(notification);
        }

        public Task<NotificationInfo> GetNotificationOrNullAsync(Guid notificationId)
        {
            return _notificationRepository.FirstOrDefaultAsync(notificationId);
        }

        public Task InsertUserNotificationAsync(UserNotificationInfo userNotification)
        {
            return _userNotificationRepository.InsertAsync(userNotification);
        }

        public Task<List<NotificationSubscriptionInfo>> GetSubscriptions(NotificationInfo notification)
        {
            return _notificationSubscriptionRepository.GetAllListAsync(s =>
                s.NotificationName == notification.NotificationName &&
                s.EntityTypeName == notification.EntityTypeName &&
                s.EntityId == notification.EntityId
                );
        }
    }
}
