using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using IdentityServer4.Models;

namespace Abp.IdentityServer4
{
    [AutoMap(typeof(PersistedGrant))]
    [Table("AbpPersistedGrants")]
    public class PersistedGrantEntity : Entity<long>
    {
        public virtual string Key { get; set; }

        public virtual string Type { get; set; }

        public virtual string SubjectId { get; set; }

        public virtual string ClientId { get; set; }

        public virtual DateTime CreationTime { get; set; }

        public virtual DateTime? Expiration { get; set; }

        public virtual string Data { get; set; }
    }
}