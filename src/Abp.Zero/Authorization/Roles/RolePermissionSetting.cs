﻿using System;

namespace Abp.Authorization.Roles
{
    /// <summary>
    /// Used to store setting for a permission for a role.
    /// </summary>
    public class RolePermissionSetting : PermissionSetting
    {
        /// <summary>
        /// Role id.
        /// </summary>
        public virtual Guid RoleId { get; set; }
    }
}