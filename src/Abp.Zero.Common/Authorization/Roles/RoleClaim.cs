﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Abp.Authorization.Roles
{
    [Table("AbpRoleClaims")]
    public class RoleClaim : CreationAuditedEntity<long>, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }

        public virtual int RoleId { get; set; }

        public virtual string ClaimType { get; set; }

        public virtual string ClaimValue { get; set; }

        public RoleClaim()
        {
            
        }

        public RoleClaim(AbpRoleBase role, Claim claim)
        {
            TenantId = role.TenantId;
            RoleId = role.Id;
            ClaimType = claim.Type;
            ClaimValue = claim.Value;
        }
    }
}
