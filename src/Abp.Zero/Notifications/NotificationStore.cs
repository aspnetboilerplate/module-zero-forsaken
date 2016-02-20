using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationStore"/> class.
        /// </summary>
        public NotificationStore(
            IRepository<NotificationInfo, Guid> notificationRepository,
            IRepository<UserNotificationInfo, Guid> userNotificationRepository,
            IRepository<NotificationSubscriptionInfo, Guid> notificationSubscriptionRepository)
        {
            _notificationRepository = notificationRepository;
            _userNotificationRepository = userNotificationRepository;
            _notificationSubscriptionRepository = notificationSubscriptionRepository;
        }

        public virtual Task InsertSubscriptionAsync(NotificationSubscriptionInfo subscription)
        {
            return _notificationSubscriptionRepository.InsertAsync(subscription);
        }

        public virtual Task DeleteSubscriptionAsync(long userId, string notificationName, string entityTypeName, string entityId)
        {
            return _notificationSubscriptionRepository.DeleteAsync(s =>
                s.UserId == userId &&
                s.NotificationName == notificationName &&
                s.EntityTypeName == entityTypeName &&
                s.EntityId == entityId
                );
        }

        public virtual Task InsertNotificationAsync(NotificationInfo notification)
        {
            return _notificationRepository.InsertAsync(notification);
        }

        public virtual Task<NotificationInfo> GetNotificationOrNullAsync(Guid notificationId)
        {
            return _notificationRepository.FirstOrDefaultAsync(notificationId);
        }

        public virtual Task InsertUserNotificationAsync(UserNotificationInfo userNotification)
        {
            return _userNotificationRepository.InsertAsync(userNotification);
        }

        public virtual Task<List<NotificationSubscriptionInfo>> GetSubscriptionsAsync(string notificationName, string entityTypeName, string entityId)
        {
            return _notificationSubscriptionRepository.GetAllListAsync(s =>
                s.NotificationName == notificationName &&
                s.EntityTypeName == entityTypeName &&
                s.EntityId == entityId
                );
        }

        public virtual Task<List<NotificationSubscriptionInfo>> GetSubscriptionsAsync(int?[] tenantIds, string notificationName, string entityTypeName, string entityId)
        {
            return _notificationSubscriptionRepository.GetAllListAsync(s =>
                tenantIds.Contains(s.TenantId) &&
                s.NotificationName == notificationName &&
                s.EntityTypeName == entityTypeName &&
                s.EntityId == entityId
                );
        }

        public virtual Task<List<NotificationSubscriptionInfo>> GetSubscriptionsAsync(long userId)
        {
            return _notificationSubscriptionRepository.GetAllListAsync(s =>
                s.UserId == userId
                );
        }

        public virtual async Task<bool> IsSubscribedAsync(long userId, string notificationName, string entityTypeName, string entityId)
        {
            return (await _notificationSubscriptionRepository.CountAsync(s =>
                s.UserId == userId &&
                s.NotificationName == notificationName &&
                s.EntityTypeName == entityTypeName &&
                s.EntityId == entityId
                )) > 0;
        }

        [UnitOfWork]
        public virtual async Task UpdateUserNotificationStateAsync(Guid userNotificationId, UserNotificationState state)
        {
            var userNotification = await _userNotificationRepository.FirstOrDefaultAsync(userNotificationId);
            if (userNotification == null)
            {
                return;
            }

            userNotification.State = state;
        }

        [UnitOfWork]
        public virtual async Task UpdateAllUserNotificationStatesAsync(long userId, UserNotificationState state)
        {
            var userNotifications = await _userNotificationRepository.GetAllListAsync(un => un.UserId == userId);

            foreach (var userNotification in userNotifications)
            {
                userNotification.State = state;
            }
        }

        public virtual Task DeleteUserNotificationAsync(Guid userNotificationId)
        {
            return _userNotificationRepository.DeleteAsync(userNotificationId);
        }

        public virtual Task DeleteAllUserNotificationsAsync(long userId)
        {
            return _userNotificationRepository.DeleteAsync(un => un.UserId == userId);
        }

        [UnitOfWork]
        public virtual Task<List<UserNotificationInfoWithNotificationInfo>> GetUserNotificationsWithNotificationsAsync(long userId, UserNotificationState? state = null, int skipCount = 0, int maxResultCount = int.MaxValue)
        {
            var query = from userNotificationInfo in _userNotificationRepository.GetAll()
                        join notificationInfo in _notificationRepository.GetAll() on userNotificationInfo.NotificationId equals notificationInfo.Id
                        where userNotificationInfo.UserId == userId && (state == null || userNotificationInfo.State == state.Value)
                        orderby notificationInfo.CreationTime descending
                        select new { userNotificationInfo, notificationInfo };

            query = query.PageBy(skipCount, maxResultCount);

            var list = query.ToList();

            return Task.FromResult(list.Select(
                a => new UserNotificationInfoWithNotificationInfo(a.userNotificationInfo, a.notificationInfo)
                ).ToList());
        }

        public virtual Task<int> GetUserNotificationCountAsync(long userId, UserNotificationState? state = null)
        {
            return _userNotificationRepository.CountAsync(un => un.UserId == userId && (state == null || un.State == state.Value));
        }

        public virtual Task<UserNotificationInfoWithNotificationInfo> GetUserNotificationWithNotificationOrNullAsync(Guid userNotificationId)
        {
            var query = from userNotificationInfo in _userNotificationRepository.GetAll()
                        join notificationInfo in _notificationRepository.GetAll() on userNotificationInfo.NotificationId equals notificationInfo.Id
                        where userNotificationInfo.Id == userNotificationId
                        select new { userNotificationInfo, notificationInfo };

            var item = query.FirstOrDefault();
            if (item == null)
            {
                return Task.FromResult((UserNotificationInfoWithNotificationInfo)null);
            }

            return Task.FromResult(new UserNotificationInfoWithNotificationInfo(item.userNotificationInfo, item.notificationInfo));
        }
    }
}
